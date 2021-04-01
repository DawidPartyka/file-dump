using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PluginTest
{
    //Interface IExtensionApplication pozwala na dodanie akcji podczas uruchomienia naszej wtyczki
    public class MyApplication : Autodesk.AutoCAD.Runtime.IExtensionApplication
    {
        //metoda ta zostanie uruchomiona podczas wczytania wtyczki
        public void Initialize()
        {
            //pobieramy kontroler wstegi
            RibbonControl ribbonControl = ComponentManager.Ribbon;
            if (ribbonControl == null)
            {
                return;
            }

            #region Tab

            //tworzymy nową zakladke wstegi i dodajemy ja do kontrolera
            RibbonTab tab = new RibbonTab();
            tab.Title = "My Application";
            tab.Id = "MY_TAB_ID";

            ribbonControl.Tabs.Add(tab);

            #endregion

            #region panel

            //tworzymy nowy panel w naszej zakladce
            RibbonPanelSource panelSource = new RibbonPanelSource();
            panelSource.Title = "My panel";

            RibbonPanel panel = new RibbonPanel();
            panel.Source = panelSource;
            tab.Panels.Add(panel);

            #endregion

            #region button

            //tworzymy przycisk
            RibbonButton testButton = new RibbonButton();
            testButton.Text = "My button";
            testButton.ShowText = true;
            testButton.ShowImage = true;
            //wczytujemy ikonke z folderu w ktorym znajduje sie nasza wtyczka
            var iconPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "icon.bmp");
            var icon = new BitmapImage(new Uri(iconPath));
            //przypisujemy ikonke do przycisku
            testButton.Image = icon;
            testButton.LargeImage = icon;
            testButton.Orientation = System.Windows.Controls.Orientation.Vertical;
            testButton.Size = RibbonItemSize.Large;

            //podpinamy akcje ktora zostanie wykonana w trakcie wcisniecia przycisku
            RibbonCommandHandler click = new RibbonCommandHandler();
            //TestButtonClick jest metoda ktora zostanie wywolana kiedy zostanie wcisniety przycisk
            click.Click += TestButtonClick;
            testButton.CommandHandler = click;

            //dodajmey przycisk do panelu
            panel.Source.Items.Add(testButton);

            #endregion

            #region button Export

            RibbonButton exportButton = new RibbonButton();
            exportButton.Text = "Export";
            exportButton.ShowText = true;
            exportButton.ShowImage = true;
            //wczytujemy ikonke z folderu w ktorym znajduje sie nasza wtyczka
            var exiconPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "iconOut.bmp");
            var exicon = new BitmapImage(new Uri(exiconPath));
            //przypisujemy ikonke do przycisku
            exportButton.Image = exicon;
            exportButton.LargeImage = exicon;
            exportButton.Orientation = System.Windows.Controls.Orientation.Vertical;
            exportButton.Size = RibbonItemSize.Large;

            //podpinamy akcje ktora zostanie wykonana w trakcie wcisniecia przycisku
            RibbonCommandHandler exportClick = new RibbonCommandHandler();
            //TestButtonClick jest metoda ktora zostanie wywolana kiedy zostanie wcisniety przycisk
            exportClick.Click += ClickExport;
            exportButton.CommandHandler = exportClick;

            //dodajmey przycisk do panelu
            panel.Source.Items.Add(exportButton);

            #endregion

            #region button Import

            RibbonButton importButton = new RibbonButton();
            importButton.Text = "Import";
            importButton.ShowText = true;
            importButton.ShowImage = true;
            //wczytujemy ikonke z folderu w ktorym znajduje sie nasza wtyczka
            var imiconPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "iconIn.bmp");
            var imicon = new BitmapImage(new Uri(imiconPath));
            //przypisujemy ikonke do przycisku
            importButton.Image = imicon;
            importButton.LargeImage = imicon;
            importButton.Orientation = System.Windows.Controls.Orientation.Vertical;
            importButton.Size = RibbonItemSize.Large;

            //podpinamy akcje ktora zostanie wykonana w trakcie wcisniecia przycisku
            RibbonCommandHandler importClick = new RibbonCommandHandler();
            //TestButtonClick jest metoda ktora zostanie wywolana kiedy zostanie wcisniety przycisk
            importClick.Click += ClickImport;
            importButton.CommandHandler = importClick;

            //dodajmey przycisk do panelu
            panel.Source.Items.Add(importButton);

            #endregion
        }

        private void ClickImport()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            if (doc != null)
            {
                //ponizsza metoda wysyla do edytora polecenie ktore nalezy wykonac
                doc.SendStringToExecute("ImportDocumentJson\n", true, false, true);
            }
        }

        private void ClickExport()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            if (doc != null)
            {
                doc.SendStringToExecute("ExportDocumentJson\n", true, false, true);
            }
        }

        private void TestButtonClick()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            if (doc != null)
            {
                //ponizsza metoda wysyla do edytora polecenie ktore nalezy wykonac
                doc.SendStringToExecute("HelloWorld\n", true, false, true);
            }
        }

        public void Terminate()
        {
        }
    }

    public class RibbonCommandHandler : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public delegate void OnClickHandler();
        public event OnClickHandler Click = null;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (Click != null)
                Click();
        }
    }
}
