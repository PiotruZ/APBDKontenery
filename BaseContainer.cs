using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontenery
{
    public abstract class BaseContainer : IContainer
    {
        private static readonly Dictionary<string, int> lastSerialNumbers = new Dictionary<string, int>();

        public string serialNumber { get; private set; }
        public double loadMass { get; set; }
        public double height { get; private set; }
        public double ownWeight { get; private set; }
        public double depth { get; private set; }
        public double maxLoadCapacity { get; private set; }

        protected BaseContainer(string containerType, double height, double ownWeight, double depth, double maxLoadCapacity)
        {
            this.height = height;
            this.ownWeight = ownWeight;
            this.depth = depth;
            this.maxLoadCapacity = maxLoadCapacity;
            serialNumber = GenerateSerialNumber(containerType);
        }

        private string GenerateSerialNumber(string containerType)
        {
            if (!lastSerialNumbers.ContainsKey(containerType))
            {
                lastSerialNumbers[containerType] = 0;
            }

            lastSerialNumbers[containerType]++;
            return $"KON-{containerType}-{lastSerialNumbers[containerType]}";
        }

        public virtual void LoadCargo(double mass)
        {
            if (mass > maxLoadCapacity)
            {
                throw new OverfillException("Cargo exceeds maximum load capacity.");
            }
            loadMass = mass;
        }

        public virtual void UnloadCargo()
        {
            loadMass = 0;
        }

        public virtual void PrintContainerInfo()
        {
            Console.WriteLine($"Container {serialNumber}:");
            Console.WriteLine($"- Type: {this.GetType().Name}");
            Console.WriteLine($"- Load Mass: {loadMass} kg");
            Console.WriteLine($"- Height: {height} cm");
            Console.WriteLine($"- Own Weight: {ownWeight} kg");
            Console.WriteLine($"- Depth: {depth} cm");
            Console.WriteLine($"- Max Load Capacity: {maxLoadCapacity} kg");
        }
    }

}
