using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginTest
{
    public class MyPoint2d
    {
        [JsonProperty("X")]
        public double X { get; set; }
        [JsonProperty("Y")]
        public double Y { get; set; }

        [JsonConstructor]
        public MyPoint2d(double x, double y)
        {
            X = x;
            Y = y;
        }

        public MyPoint2d(Point2d point)
        {
            X = point.X;
            Y = point.Y;
        }

        public MyPoint2d(Point3d point)
        {
            X = point.X;
            Y = point.Y;
        }
    }

    public class MyPolyline
    {
        [JsonProperty("Points")]
        public List<MyPoint2d> Points { get; set; }

        [JsonConstructor]
        public MyPolyline(List<MyPoint2d> points)
        {
            Points = points;
        }

        public MyPolyline(List<Point2d> points)
        {
            Points = new List<MyPoint2d>();

            foreach (Point2d p in points)
            {
                Points.Add(new MyPoint2d(p));
            }
        }
    }
    public class MyLine
    {
        public MyPoint2d StartPoint { get; set; }
        public MyPoint2d EndPoint { get; set; }

        [JsonConstructor]
        public MyLine(Point2d st, Point2d ed)
        {
            StartPoint = new MyPoint2d(st);
            EndPoint = new MyPoint2d(ed);
        }

        public MyLine(MyPoint2d st, MyPoint2d ed)
        {
            StartPoint = st;
            EndPoint = ed;
        }
    }

    public class MyCircle
    {
        public MyPoint2d Center { get; set; }
        public double Radius { get; set; }

        [JsonConstructor]
        public MyCircle(Point2d cent, double rad)
        {
            Center = new MyPoint2d(cent);
            Radius = rad;
        }

        public MyCircle(Point3d cent, double rad)
        {
            Center = new MyPoint2d(cent);
            Radius = rad;
        }
    }
    
    public class MyDoc
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<MyLine> Lines { get; set; }
        public List<MyPolyline> Polylines { get; set; }
        public List<MyCircle> Circles { get; set; }

        public MyDoc()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;

            Id = 0;
            Name = Path.GetFileName(doc.Name);
            Lines = new List<MyLine>();
            Polylines = new List<MyPolyline>();
            Circles = new List<MyCircle>();
        }

        public void FromJson(string content)
        {
            try
            {
                MyDoc deserialized = JsonConvert.DeserializeObject<MyDoc>(content);
                Id = deserialized.Id;
                Lines = deserialized.Lines;
                Polylines = deserialized.Polylines;
                Circles = deserialized.Circles;
            }
            catch (SystemException e)
            {
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                ed.WriteMessage($"Coś poszło nie tak {e.Message}");
            }
            
        }

        public void AddPolyline(List<Point2d> points)
        {
            Polylines.Add(new MyPolyline(points));
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
    
}
