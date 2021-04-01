using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Calculator
{

    class SignType
    {
        public string SignCharacter { get; set; }

        public SignType(string ch)
        {
            SignCharacter = ch;
        }
    }

    class CalculationSign : SignType
    {
        public bool Priority { get; set; }
        public Func<double, double, SignType> Act { get; set; }

        public CalculationSign(string ch, bool p, Func<double, double, SignType> action) : base(ch)
        {
            Priority = p;
            Act = action;
        }
    }

    class NumberSign : SignType
    {
        public double Number { get; set; }
        

        public NumberSign(string ch, double num) : base(ch)
        {
            Number = num;
        }
    }

    class InvalidOperation : SignType 
    {
        public InvalidOperation(string ch) : base(ch) { }
    }


    class Calculation
    {
        private List<SignType> AllSigns { get; set; }
        private List<SignType> WindowContent { get; set; }
        private Label ErrorLog { get; set; }

        private InvalidOperation divisionByZero { get; set; }
        public Calculation(Label e)
        {
            ErrorLog = e;
            AllSigns = new List<SignType>();
            WindowContent = new List<SignType>();

            for (int i = 0; i < 10; i++)
                AllSigns.Add(new NumberSign(Convert.ToString(i), i));

            AllSigns.Add(new CalculationSign("/", true, delegate(double x, double y) {
                if (x == 0)
                    return divisionByZero;
               
                return new NumberSign((x / y).ToString(), x / y);

            }));
            AllSigns.Add(new CalculationSign("*", true, delegate (double x, double y) { return new NumberSign((x * y).ToString(), x * y); }));
            AllSigns.Add(new CalculationSign("+", false, delegate (double x, double y) { return new NumberSign((x + y).ToString(), x + y); }));
            AllSigns.Add(new CalculationSign("-", false, delegate (double x, double y) { return new NumberSign((x - y).ToString(), x - y); }));
            AllSigns.Add(new CalculationSign(".", false, delegate (double x, double y) { return new NumberSign("0", 0); }));

            divisionByZero = new InvalidOperation("Nie dzieli się przez zero");
            
        }

        private void connectNumbers()
        {
            List<SignType> newList = new List<SignType>();

            foreach(SignType x in WindowContent)
            {
                if (newList.Count > 0)
                {
                    SignType last = newList.Last();
                    try
                    {
                        if (last is NumberSign && x is NumberSign)
                        {
                            NumberSign newNum = new NumberSign(last.SignCharacter + x.SignCharacter, (double)decimal.Parse(last.SignCharacter + x.SignCharacter));
                            newList[newList.Count - 1] = newNum;
                        }
                        else
                            newList.Add(x);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Coś poszło nie tak: {e.Message}");
                        ErrorLog.Content = e.Message + "\tTry \"Clear\" button";
                    }
                }
                else
                    newList.Add(x);
            }

            WindowContent = newList;
            //ErrorLog.Content = WindowContent.Last().SignCharacter;
        }

        private void connectDoubles()
        {
            CalculationSign dot = (CalculationSign)AllSigns.Single(x => x.SignCharacter == ".");
            while (WindowContent.Contains(dot))
            {
                int id = WindowContent.IndexOf(dot);
                double firstPart = (WindowContent[id - 1] as NumberSign).Number;
                double secPart = (WindowContent[id + 1] as NumberSign).Number;

                double newNum = (double)((decimal)firstPart + (decimal)secPart / (decimal)Math.Pow(10, WindowContent[id + 1].SignCharacter.Count()));
                //double newNum = firstPart + double.Parse($"0.{(WindowContent[id + 1] as NumberSign).Number}");

                WindowContent[id - 1] = new NumberSign(newNum.ToString(), newNum);
                WindowContent.RemoveAt(id + 1);
                WindowContent.RemoveAt(id);
                //double newNum =  + double.Parse();
            }
        }

        public void AddSign(string ch)
        {   
            SignType toAdd = (SignType)AllSigns.Single(x => x.SignCharacter == ch);

            if (toAdd == null)
                return;

            if (WindowContent.Count > 0)
            {
                SignType last = (SignType)WindowContent.Last();

                if (ch == "." && last is NumberSign)
                {
                    NumberSign n = last as NumberSign;
                    if (n.Number != (int)n.Number)
                        return;
                }

                if (toAdd is CalculationSign && last is CalculationSign)
                {
                    if(toAdd.SignCharacter == ".")
                    {
                        WindowContent.Add((SignType)AllSigns.Single(x => x.SignCharacter == "0"));
                        WindowContent.Add(toAdd);
                    }
                }
                else
                {
                    WindowContent.Add(toAdd);

                    if (toAdd is NumberSign)
                    {
                        connectNumbers();
                        connectDoubles();
                    }
                        
                }
                    
                
            }
            else if (toAdd.SignCharacter == ".")
            {
                WindowContent.Add((SignType)AllSigns.Single(x => x.SignCharacter == "0"));
                WindowContent.Add(toAdd);
            }
            else if(toAdd is NumberSign || toAdd.SignCharacter == "-")
                WindowContent.Add(toAdd);
        }

        public void Clear()
        {
            WindowContent.Clear();
        }

        public string GetScreenText()
        {
            string result = "";

            if (WindowContent.Count > 0)
            {
                connectNumbers();
                WindowContent.ForEach(x => result += ((x is CalculationSign && x.SignCharacter != "." ? " " + x.SignCharacter + " " : x.SignCharacter)));
            }               
            else
                return "0";

            return result;
        }
        
        public void Calculate()
        {
            ErrorLog.Content = $"Last operation: {GetScreenText()}";
            while(WindowContent.Count(x => x is CalculationSign) > 0){
                CalculationSign cur;

                try
                {
                    if (WindowContent.Count(x => x is CalculationSign && (x as CalculationSign).Priority == true) > 0)
                        cur = WindowContent.First(x => x is CalculationSign && (x as CalculationSign).Priority == true) as CalculationSign;
                    else
                        cur = WindowContent.First(x => x is CalculationSign) as CalculationSign;

                    int index = WindowContent.IndexOf(cur);

                    if(cur.SignCharacter == "-" && index == 0)
                    {
                        double curNum = (WindowContent[index + 1] as NumberSign).Number;

                        WindowContent[index + 1] = new NumberSign((-curNum).ToString(), -curNum);
                        WindowContent.RemoveAt(index);
                        continue;
                    }

                    SignType num = cur.Act((WindowContent[index - 1] as NumberSign).Number, (WindowContent[index + 1] as NumberSign).Number);


                    if (!(num is InvalidOperation))
                    {
                        WindowContent[index - 1] = num;
                        WindowContent.RemoveAt(index + 1);
                        WindowContent.RemoveAt(index);
                    }
                    else
                    {
                        Clear();
                        ErrorLog.Content = "Nie dzieli się przez zero. Wyczyszczono obliczenia";
                        break;
                    }
                }
                catch(Exception e)
                {
                    ErrorLog.Content = $"Coś poszło nie tak: {e.Message}";
                }
                
                
            }
        }
    }
}
