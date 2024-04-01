using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontenery
{
    public class GasContainer : BaseContainer, IHazardNotifier
    {
        public double pressure { get; private set; }

        public GasContainer(double height, double ownWeight, double depth, double maxLoadCapacity, double pressure)
            : base("G", height, ownWeight, depth, maxLoadCapacity)
        {
            this.pressure = pressure;
        }

        public override void LoadCargo(double mass)
        {
            if (mass > maxLoadCapacity)
            {
                var errorMessage = $"Attempted to load {mass}kg into a gas container, exceeding the max capacity of {maxLoadCapacity}kg.";
                SendHazardNotification(errorMessage);
                throw new OverfillException(errorMessage);
            }
            base.LoadCargo(mass);
        }

        public override void UnloadCargo()
        {
            loadMass *= 0.05;
        }

        public void SendHazardNotification(string message)
        {
            Console.WriteLine($"Hazard Notification for {serialNumber}: {message}");
        }

        public override void PrintContainerInfo()
        {
            base.PrintContainerInfo(); // Wywołuje implementację z klasy bazowej
            Console.WriteLine($"- Pressure: {pressure} atm");
        }
    }
}


