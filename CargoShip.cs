using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontenery
{
    public class CargoShip
    {
        public int maxSpeed { get; private set; }
        public int maxContainerCount { get; private set; }
        public double maxWeight { get; private set; }
        public List<IContainer> containers = new List<IContainer>();

        public CargoShip(int maxSpeed, int maxContainerCount, double maxWeight)
        {
            this.maxSpeed = maxSpeed;
            this.maxContainerCount = maxContainerCount;
            this.maxWeight = maxWeight;
        }

        public void AddContainer(IContainer container)
        {
            if (containers.Count >= maxContainerCount)
            {
                throw new Exception("Cannot add more containers.");
            }
            if (CurrentWeight() + container.ownWeight + container.loadMass > maxWeight)
            {
                throw new Exception("Cannot add container. Ship is overloaded.");
            }
            containers.Add(container);
        }

        public void AddManyContainers(List<IContainer> newContainers)
        {
            foreach (var container in newContainers)
            {
                if (containers.Count >= maxContainerCount)
                {
                    throw new InvalidOperationException("Cannot add more containers. Ship has reached its maximum container capacity.");
                }
                if (CurrentWeight() + container.ownWeight + container.loadMass > maxWeight)
                {
                    throw new InvalidOperationException($"Cannot add container with serial number {container.serialNumber}. Ship will be overloaded.");
                }
                containers.Add(container);
            }
        }

        public void RemoveContainer(string serialNumber)
        {
            var container = containers.FirstOrDefault(c => c.serialNumber == serialNumber);
            if (container != null)
            {
                containers.Remove(container);
            }
            else
            {
                throw new InvalidOperationException($"Container with serial number {serialNumber} not found.");
            }
        }

        public void UnloadAllContainers()
        {
            containers.Clear();
        }

        public void ReplaceContainer(string serialNumber, IContainer newContainer)
        {
            int index = containers.FindIndex(c => c.serialNumber == serialNumber);
            if (index != -1)
            {
                if (CurrentWeight() - containers[index].ownWeight - containers[index].loadMass + newContainer.ownWeight + newContainer.loadMass > maxWeight)
                {
                    throw new OverfillException("Cannot replace container. Ship will be overloaded.");
                }

                containers[index] = newContainer;
            }
            else
            {
                throw new InvalidOperationException($"Container with serial number {serialNumber} not found.");
            }
        }

        public void TransferContainerToAnotherShip(string serialNumber, CargoShip otherShip)
        {
            var container = containers.FirstOrDefault(c => c.serialNumber == serialNumber);
            if (container == null)
            {
                throw new InvalidOperationException($"Container with serial number {serialNumber} not found.");
            }

            otherShip.AddContainer(container);
            containers.Remove(container);

        }

        public void PrintShipInfo()
        {
            Console.WriteLine($"Ship Details:");
            Console.WriteLine($"- Max Speed: {maxSpeed} knots");
            Console.WriteLine($"- Max Container Count: {maxContainerCount}");
            Console.WriteLine($"- Max Weight: {maxWeight} tons");
            Console.WriteLine($"- Current Container Count: {containers.Count}");
            Console.WriteLine($"- Current Total Weight: {CurrentWeight() / 1000.0} tons"); // Dzielone przez 1000, aby przeliczyć na tony

            if (containers.Any())
            {
                Console.WriteLine("Containers on board:");
                foreach (var container in containers)
                {
                    container.PrintContainerInfo();
                }
            }
            else
            {
                Console.WriteLine("No containers on board.");
            }
        }

        private double CurrentWeight()
        {
            return containers.Sum(container => container.ownWeight + container.loadMass);
        }
    }
}
