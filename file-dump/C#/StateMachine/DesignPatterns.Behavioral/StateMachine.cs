using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns
{
    public interface IState
    {
        void Enter();
        void Exit();
        IState Execute();
    }

    public class Drink
    {
        private string name;
        private double price;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public double Price
        {
            get { return price; }
            set { price = value; }
        }
        public Drink(string n, double p)
        {
            Name = n;
            Price = p;
        }

        public string DrinkInfo()
        {
            return $"Napój: {Name}:\tKoszt: {Price}";
        }
    }

    public class DrinkLane : Drink
    {
        private int amount;
        private string code;

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public DrinkLane(string drink, double price, string code, int amount) : base(drink, price)
        {
            Amount = amount;
            Code = code;
        }

        public void DecreaseDrinkAmount()
        {
            if(Amount > 0)
                Amount--;
        }

        public string ShowInfo()
        {
            return $"{Code}\n{DrinkInfo()}\tW maszynie: {Amount} sztuk";
        }
    }

    public class VendingMachineData
    {
        public List<double> AcceptedNominals;
        public List<DrinkLane> DrinkLanes;
        public double CurrencyHolded = 0;

        public VendingMachineData(List<double> nom, List<DrinkLane> dr)
        {
            AcceptedNominals = nom;
            DrinkLanes = dr;
        }
        public void ShowMachineItems()
        {
            Console.WriteLine("Zawartość automatu:\n");

            foreach (var x in DrinkLanes)
                Console.WriteLine(x.ShowInfo() + '\n');
        }

        public void ShowAcceptedCoins()
        {
            Console.WriteLine("Automat akceptuje nominały: ");

            foreach (double coin in AcceptedNominals)
                Console.Write($"{coin}PLN\n");

            Console.WriteLine();
        }

        public void CurrentMoneyInside()
        {
            Console.WriteLine($"Obecnie w maszynie znajduje się: {CurrencyHolded} zł");
        }

        public void MachineInfo()
        {
            Console.WriteLine("Witaj w automacie z napojami\n");
            ShowAcceptedCoins();
            ShowMachineItems();
            CurrentMoneyInside();
        }

        public string ReturnCash()
        {
            double returned = CurrencyHolded;
            CurrencyHolded = 0;

            return $"Zwrócono {returned} PLN";
        }

        public Dictionary<double, int> ReturnChange()
        {
            Dictionary<double, int> change = new Dictionary<double, int>();
            List<double> nominalsSorted = AcceptedNominals.OrderByDescending(x => x).ToList();

            foreach(double x in nominalsSorted)
            {       
                if(x <= CurrencyHolded)
                {
                    change.Add(x, 0);
                    change[x] = Convert.ToInt32(Math.Floor(CurrencyHolded / x));
                    CurrencyHolded -= (x * change[x]);
                }
            }

            return change;
        }

        public bool AddCoin(double coin)
        {
            if (AcceptedNominals.Contains(coin))
            {
                CurrencyHolded += coin;
                return true;
            }

            return false;
        }
    }

    public class AddCoinState : IState
    {
        private VendingMachineData machineData;

        public AddCoinState(VendingMachineData data)
        {
            machineData = data;
        }
        public void Enter()
        {
            Console.Clear();
            machineData.ShowAcceptedCoins();
            machineData.CurrentMoneyInside();
            Console.WriteLine("Proszę wpisać kwotę jaką wrzucasz do automatu:");
        }

        public IState Execute()
        {
            double coin;
            if (!double.TryParse(Console.ReadLine(), out coin))
            {
                Console.WriteLine("Niepoprawna kwota.\nSpróbuj jeszcze raz.\n");
                return Execute();
            }
            else
            {
                if (machineData.AddCoin(coin))
                    return new WaitingForAction(machineData);
                else
                {
                    Console.WriteLine("Niepoprawna kwota.\nSpróbuj jeszcze raz.\n");
                    return Execute();
                }
            }     
        }

        public void Exit()
        {
            Console.WriteLine("Powrót z stanu wrzucania monet");
        }
    }

    public class ReturnChangeState : IState
    {
        private VendingMachineData machineData;
        public ReturnChangeState(VendingMachineData data)
        {
            machineData = data;
        }

        public void Enter()
        {
            Console.WriteLine("Z automatu wypadają monety");
        }

        public IState Execute()
        {
            Console.WriteLine("Zwrócono monety: ");
            double totalChange = 0;

            foreach(KeyValuePair<double, int> x in machineData.ReturnChange())
            {
                Console.WriteLine($"{x.Key}PLN - {x.Value} monet");
                totalChange += (x.Key * x.Value);
            }

            Console.WriteLine($"Łącznie zwrócono: {totalChange} PLN");
            Console.WriteLine("Proszę odebrać resztę.\n(Wciśnij dowolny klawisz by odebrać resztę)");
            Console.ReadKey();

            return new WaitingForAction(machineData);
        }
        public void Exit()
        {
            Console.WriteLine("Wychodzę ze stanu wydawania reszty");
        }
    } 

    public class SellDrinkState : IState
    {
        private VendingMachineData machineData;
        private DrinkLane drinkSold;
        public SellDrinkState(VendingMachineData data, DrinkLane drink)
        {
            machineData = data;
            drinkSold = drink;
        }

        public void Enter()
        {
            Console.WriteLine($"Z automatu wypadła puszka {drinkSold.Name}");
        }

        public IState Execute()
        {
            machineData.DrinkLanes.Single(x => x == drinkSold).DecreaseDrinkAmount();
            machineData.CurrencyHolded -= drinkSold.Price;

            if(machineData.CurrencyHolded > 0)
            {
                return new ReturnChangeState(machineData);
            }
            
            return new WaitingForAction(machineData);
        }

        public void Exit()
        {
            Console.WriteLine("Wychodzę ze stanu sprzedaży napoju");
        }
    }

    public class EnterDrinkCode : IState
    {
        private VendingMachineData machineData;
        public EnterDrinkCode(VendingMachineData data)
        {
            machineData = data;
        }

        public void Enter()
        {
            Console.Clear();
            machineData.ShowMachineItems();
            machineData.CurrentMoneyInside();
        }

        public IState Execute()
        {
            Console.WriteLine("#################################\n" +
                              "# Proszę wpisać kod napoju      #\n" +
                              "# lub wpisz:                    #\n" +
                              "# 1 - wróć do menu wyboru akcji #\n" +
                              "# 2 - zwróć pieniądze           #\n" +
                              "#################################");

            string input = Console.ReadLine();
            if(machineData.DrinkLanes.Where(x => x.Code == input).Count() == 1)
            {
                DrinkLane drink = machineData.DrinkLanes.Single(x => x.Code == input);
                
                if(drink.Amount > 0)
                {
                    if (machineData.CurrencyHolded >= drink.Price)
                        return new SellDrinkState(machineData, drink);
                    else
                    {
                        Console.WriteLine("Nie odpowiednia kwota dla tego napoju.\n" +
                                          "Proszę wrzucić kolejną monetę.\n\nPrzejdź dalej... (dowolny klawisz)");

                        Console.ReadKey();
                        return new AddCoinState(machineData);
                    }
                }
                else
                {
                    Console.WriteLine("Wybrany napój został wyprzedany\nPowrót do menu wyboru akcji\n(Wciśnij dowolny klawisz)");
                    Console.ReadKey();

                    return new WaitingForAction(machineData);
                }
                
            }
            else if(input == "1")
            {
                return new WaitingForAction(machineData);
            }
            else if(input == "2")
            {
                return new ReturnMoney(machineData);
            }
            else
            {
                Console.WriteLine("\nBłędny kod.\n Spróbuj jeszcze raz\n");
                return Execute();
            }
        }

        public void Exit()
        {
            Console.WriteLine("Powrót z stanu wyboru napoju");
        }
    }
    
    public class ReturnMoney : IState
    {
        private VendingMachineData machineData;
        public ReturnMoney(VendingMachineData data)
        {
            Console.Clear();
            machineData = data;
        }

        public void Enter()
        {
            Console.WriteLine("Wciśnięto przycisk zwrotu pieniędzy");
        }

        public IState Execute()
        {
            if (machineData.CurrencyHolded > 0)
                Console.WriteLine(machineData.ReturnCash());
            else
                Console.WriteLine("Brak kwoty do zwrócenia");

            Console.WriteLine("Wciśnij dowolny klawisz by kontynuować");

            Console.ReadKey();

            return new WaitingForAction(machineData);
        }

        public void Exit()
        {
            Console.WriteLine("\nWychodzę ze stanu zwrotu pieniędzy");
        }
    }

    public class WaitingForAction : IState
    {
        private VendingMachineData machineData;

        public WaitingForAction(VendingMachineData data)
        {
            machineData = data;
        }

        public void Enter()
        {
            Console.Clear();
            machineData.MachineInfo();
        }

        public IState Execute()
        {
            Console.WriteLine("\n##########################\n" +
                              "# Dostępne akcje:        #\n" +
                              "# 1. Wrzuć monetę        #\n" +
                              "# 2. Wprowadź kod napoju #\n" +
                              "# 3. Zwróć pieniądze     #\n" +
                              "##########################\n");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    return new AddCoinState(machineData);
                    
                case ConsoleKey.D2:
                    return new EnterDrinkCode(machineData);
                    
                case ConsoleKey.D3:
                    return new ReturnMoney(machineData);
                    
                default:
                    Console.WriteLine("Niepoprawny wybór.\nSpróbuj jeszcze raz.\n");
                    return Execute();
            }
        }

        public void Exit()
        {
            Console.WriteLine("\nWychodzę ze stanu oczekiwania na akcję");
        }
    }

    public class VendingStateMachine
    {
        private IState currentState;
        private IState previousState;
        public VendingMachineData MachineData;

        public VendingStateMachine(VendingMachineData data)
        {
            MachineData = data;
        }

        public void ChangeState(IState state)
        {
            if (currentState != null)
            {
                currentState.Exit();
                previousState = currentState;
            }
            currentState = state;
            currentState.Enter();
        }

        public void SwitchToPreviousState()
        {
            ChangeState(previousState);
        }

        public void ExecuteStateLogic()
        {
            var newState = currentState.Execute();
            if (newState != null)
            {
                currentState.Exit();
                newState.Enter();
                previousState = currentState;
                currentState = newState;
            }
        }
    }
}
