using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/*
 *
 *  Tworząc nowy projekt nalezy zaimportowac (za pomoca NuGet) AutoCAD.NET, AutoCAD.Core oraz AutoCAD.Model, 
 *  pamietajac by wybrac wersje biblioteki zgodna z docelowa wersja AutoCADa.
 *  
 *  Po skompilowani projektu nalezy otworzyc program AutoCAD, stworzyc lub otworzyc projekt i wpisac polecenie NETLOAD.
 *  W oknie dialogowym nalezy wybrac skompilowany plik dll.
 *  
 *  Po zaladowaniu add-inu do AutoCADa mozna korzystac z naszych polecen.
 *  
 */


namespace PluginTest
{
    public class Commands
    {
        [CommandMethod("ExportDocumentJson")]
        public void ExportDocumentJson()
        {
            CommunicationAPI com = new CommunicationAPI("https://localhost:44323/api/");

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            //prosimy uzytkownika o zaznaczenie obiektow (jednego lub wiecej)
            ed.WriteMessage("Proszę zaznaczyć linie i polilinie do wyeksportowania");
            var sel = ed.GetSelection();

            MyDoc doc = new MyDoc();

            if (sel.Status == PromptStatus.OK)
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    //dla kazdego wybranego obiektu wyswietalmy jego id i typ do konsoli edytora
                    foreach (ObjectId objectId in sel.Value.GetObjectIds())
                    {
                        try
                        {
                            var obj = tr.GetObject(objectId, OpenMode.ForRead);

                            // Can't convert all objects to Polyline
                            switch (objectId.ObjectClass.Name)
                            {
                                case "AcDbCircle":
                                    Circle c = obj as Circle;
                                    doc.Circles.Add(new MyCircle(c.Center, c.Radius));
                                    break;
                                case "AcDbLine":
                                    Line l = obj as Line;
                                    doc.Lines.Add(new MyLine(new MyPoint2d(l.StartPoint), new MyPoint2d(l.EndPoint)));
                                    break;

                                case "AcDbPolyline":
                                    List<Point2d> points = new List<Point2d>();
                                    Polyline lwp = obj as Polyline;

                                    if (lwp != null)
                                    {
                                        int vn = lwp.NumberOfVertices;
                                        for (int i = 0; i < vn; i++)
                                        {
                                            points.Add(lwp.GetPoint2dAt(i));
                                        }
                                    }

                                    doc.AddPolyline(points);
                                    break;
                            }
                        }
                        catch (Autodesk.AutoCAD.Runtime.Exception e)
                        {
                            ed.WriteMessage($"\nCoś poszło nie tak: {e.Message}\nNie obsłużono obiektu: {objectId.ObjectClass.Name}");
                        }
                    }
                }

                Result res = com.CreateDoc(doc);

                string msg;
                if (res.Success)
                {
                    MyDoc received = new MyDoc();
                    received.FromJson(res.Message);
                    msg = $"Pomyślnie utworzono projekt: id - {received.Id}";
                }
                else
                    msg = $"Coś poszło nie tak: Kod statusu - {res.Message}";

                ed.WriteMessage(msg);
            }
        }

        // Dla linii
        private void drawFromDoc(List<MyLine> lines)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable blockTable;
                blockTable = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                BlockTableRecord blockTableRecord;
                blockTableRecord = tr.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                for (int i = 0; i < lines.Count; i++)
                {
                    MyLine ent = lines[i];

                    Line acLine = new Line(new Point3d(ent.StartPoint.X, ent.StartPoint.Y, 0), new Point3d(ent.EndPoint.X, ent.EndPoint.Y, 0));

                    blockTableRecord.AppendEntity(acLine);
                    tr.AddNewlyCreatedDBObject(acLine, true);      
                }

                tr.Commit();
            }
        }

        // Dla polilinii
        private void drawFromDoc(List<MyPolyline> polys)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable blockTable;
                blockTable = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                BlockTableRecord blockTableRecord;
                blockTableRecord = tr.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                foreach (MyPolyline p in polys)
                {
                    using (Polyline polyline = new Polyline())
                    {
                        for (int i = 0; i < p.Points.Count; i++)
                        {
                            polyline.AddVertexAt(i, new Point2d(p.Points[i].X, p.Points[i].Y), 0, 0, 0);
                        }

                        blockTableRecord.AppendEntity(polyline);
                        tr.AddNewlyCreatedDBObject(polyline, true);
                    }
                }

                tr.Commit();
            }
        }

        // Dla okręgów
        private void drawFromDoc(List<MyCircle> circles)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable blockTable;
                blockTable = tr.GetObject(db.BlockTableId, OpenMode.ForWrite) as BlockTable;

                BlockTableRecord blockTableRecord;
                blockTableRecord = tr.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                foreach (MyCircle c in circles)
                {
                    using (Circle newCircle = new Circle())
                    {
                        newCircle.SetDatabaseDefaults();
                        newCircle.Center = new Point3d(c.Center.X, c.Center.Y, 0);
                        newCircle.Radius = c.Radius;

                        blockTableRecord.AppendEntity(newCircle);
                        tr.AddNewlyCreatedDBObject(newCircle, true);
                    }
                }

                tr.Commit();
            }
        }

        [CommandMethod("ImportDocumentJson")]
        public void ImportDocumentJson()
        {
            CommunicationAPI com = new CommunicationAPI("https://localhost:44323/api/");

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;

            var id = ed.GetInteger("Podaj id projektu do zaimportowania");

            if (id.Status != PromptStatus.OK)
                return;

            Result import = com.GetDoc(id.Value);
            if (import.Success)
            {
                MyDoc doc = new MyDoc();
                doc.FromJson(import.Message);

                ed.WriteMessage($"Wczytane linie: {doc.Lines.Count}\nWczytane polilinie: {doc.Polylines.Count}");

                if (doc.Lines.Count > 0)
                    drawFromDoc(doc.Lines);

                if (doc.Polylines.Count > 0)
                    drawFromDoc(doc.Polylines);

                if (doc.Circles.Count > 0)
                    drawFromDoc(doc.Circles);
            }
            else
                ed.WriteMessage($"Coś poszło nie tak: Kod statusu - {import.Message}");
        }
    }
}
