using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Calculation calc { get; set; }
        private int charactersOnScreen { get; set; }
        private int fontDecrease { get; set; }
        private int nextFontDecrease { get; set; }
        private int maxChar { get; set; }
        private int startFontSize { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            calc = new Calculation(ErrorsGoHere);
            startFontSize = 48;
            charactersOnScreen = 14;
            fontDecrease = 14;
            nextFontDecrease = fontDecrease;
            maxChar = 28;
            UpdateText();
        }

        private void UpdateText()
        {
            string txt = calc.GetScreenText();
            CalculationScreen.Content = txt;
            CheckFontSize();
        }

        private void Equals(object sender, RoutedEventArgs e)
        {
            calc.Calculate();
            UpdateText();
        }

        private void ClearSelf(object sender, RoutedEventArgs e)
        {
            calc.Clear();
            UpdateText();

            CalculationScreen.FontSize = startFontSize;
            nextFontDecrease = fontDecrease;
        }

        private void CheckFontSize()
        {
            if (CalculationScreen.Content.ToString().Count() > nextFontDecrease)
            {
                double fts = CalculationScreen.FontSize * 0.665;

                CalculationScreen.FontSize = (int)Math.Ceiling(fts) < 18 ? 18 : Math.Ceiling(fts);

                nextFontDecrease += (int)Math.Ceiling((double)charactersOnScreen / 2);
            }
        }

        private void NumBut(object sender, RoutedEventArgs e)
        {   
            if(CalculationScreen.Content.ToString().Count() < maxChar)
            {
                Button but = sender as Button;
                calc.AddSign(but.Content.ToString());

                UpdateText();
            }
        }
    }
}
