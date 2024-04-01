using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontenery
{
    public class LiquidContainer : BaseContainer, IHazardNotifier
    {
        public bool isHazardous { get; private set; }

        public LiquidContainer(double height, double ownWeight, double depth, double maxLoadCapacity, bool isHazardous)
            : base("L", height, ownWeight, depth, maxLoadCapacity)
        {
            this.isHazardous = isHazardous;
        }

        public override void LoadCargo(double mass)
        {
            double maxAllowed = isHazardous ? maxLoadCapacity * 0.5 : maxLoadCapacity * 0.9;
            if (mass > maxAllowed)
            {
                if (isHazardous)
                {
                    var errorMessage = $"Attempted to load {mass}kg into a hazardous liquid container, exceeding the 50% capacity limit of {maxAllowed}kg.";
                    SendHazardNotification(errorMessage);
                }

                throw new OverfillException($"Attempted to overload the container. Max allowed (90% of load capacity): {maxAllowed}kg, attempted: {mass}kg.");
            }
            else
            {
                base.LoadCargo(mass);
            }
        }

        public void SendHazardNotification(string message)
        {
            Console.WriteLine($"Hazard Notification for {serialNumber}: {message}");
        }

        public override void PrintContainerInfo()
        {
            base.PrintContainerInfo();
            Console.WriteLine($"- Is Hazardous: {isHazardous}");
        }
    }
}

