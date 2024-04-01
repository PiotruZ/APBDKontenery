using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontenery
{
    public class RefrigeratedContainer : BaseContainer, IHazardNotifier
    {
        private static readonly Dictionary<string, double> productTemperatureRequirements = new Dictionary<string, double>
        {
            {"Bananas", 13.3},
            {"Chocolate", 18},
            {"Fish", 2},
            {"Meat", -15},
            {"Ice cream", -18},
            {"Frozen pizza", -30},
            {"Cheese", 7.2},
            {"Sausages", 5},
            {"Butter", 20.5},
            {"Eggs", 19}
        };

        private List<string> allowedProductTypes;

        public double temperature { get; private set; }
        public string? currentProductType { get; private set; }

        // Właściwość, która pozwala na ustawienie typu produktu, który zostanie załadowany do kontenera. Będzie używany do sprawdzenia, czy typ produktu jest dozwolony i możliwy do załadowania. 
        // Trzeba o tym pamiętać podczas udostępniania operacji ładowania kontenera podczas interakcji z użytkownikiem.
        public string pendingProductType { get; set; }

        public RefrigeratedContainer(double height, double ownWeight, double depth, double maxLoadCapacity, double temperature, List<string> allowedProductTypes)
            : base("C", height, ownWeight, depth, maxLoadCapacity)
        {
            this.temperature = temperature;
            this.allowedProductTypes = allowedProductTypes ?? throw new ArgumentNullException(nameof(allowedProductTypes));
            currentProductType = null; // Początkowo kontener jest pusty
        }


        public override void LoadCargo(double mass)
        {
            if (!allowedProductTypes.Contains(pendingProductType))
            {
                throw new InvalidOperationException($"Product type '{pendingProductType}' is not allowed in this container.");
            }

            if (!string.IsNullOrEmpty(currentProductType) && currentProductType != pendingProductType)
            {
                throw new InvalidOperationException("This container already contains a different product type.");
            }

            if (mass > maxLoadCapacity)
            {
                throw new OverfillException("Attempting to load cargo exceeds the container's max load capacity.");
            }

            currentProductType = pendingProductType;
            base.LoadCargo(mass);
        }

        public override void UnloadCargo()
        {
            base.UnloadCargo();
            currentProductType = null; // Kontener jest teraz pusty
        }

        public void SetTemperature(double newTemperature)
        {
            double requiredTemperature = GetRequiredTemperatureForProduct(currentProductType);

            if (newTemperature < requiredTemperature)
            {
                var errorMessage = $"Attempted to set temperature to {newTemperature}°C, which is below the required minimum for {currentProductType}.";
                throw new InvalidOperationException(errorMessage);
            }

            temperature = newTemperature;
        }

        private double GetRequiredTemperatureForProduct(string productType)
        {
            if (productTemperatureRequirements.TryGetValue(productType, out double requiredTemperature))
            {
                return requiredTemperature;
            }

            throw new ArgumentException($"Required temperature for product type '{productType}' is not defined.");
        }

        public void SendHazardNotification(string message)
        {
            Console.WriteLine($"Hazard Notification for {serialNumber}: {message}");
        }

        public override void PrintContainerInfo()
        {
            base.PrintContainerInfo();
            Console.WriteLine($"- Temperature: {temperature}°C");
            Console.WriteLine($"- Allowed product types: {string.Join(", ", allowedProductTypes)}");
            if (!string.IsNullOrEmpty(currentProductType))
            {
                Console.WriteLine($"- Loaded product type: {currentProductType}");
            }
            else
            {
                Console.WriteLine("- No product currently loaded.");
            }
        }
    }
}



