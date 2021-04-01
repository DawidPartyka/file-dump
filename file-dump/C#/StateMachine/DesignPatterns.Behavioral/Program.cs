using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.Behavioral
{
    class Program
    {
        static void Main(string[] args)
        {
            //Maszyna stanów
            List<double> nominals = new List<double> { 0.10, 0.20, 0.50, 1, 2, 5 };
            List<DrinkLane> drinksInMachine = new List<DrinkLane>();
            drinksInMachine.Add(new DrinkLane("Cola", 3.70, "A08", 3));
            drinksInMachine.Add(new DrinkLane("Fanta", 2.30, "B12", 6));
            drinksInMachine.Add(new DrinkLane("Sprite", 1.50, "A06", 1));
            drinksInMachine.Add(new DrinkLane("Dr Pepper", 3.00, "C15", 0));

            VendingStateMachine machine = new VendingStateMachine(new VendingMachineData(nominals, drinksInMachine));
            machine.ChangeState(new WaitingForAction(machine.MachineData));

            while (true)
            {
                machine.ExecuteStateLogic();
            }
        }
    }
}
