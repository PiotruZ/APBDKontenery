using Kontenery;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Kontenery
{
    class Program
    {
        private static List<CargoShip> ships = new List<CargoShip>();
        private static List<IContainer> containers = new List<IContainer>();

        static void Main(string[] args)
        {
            // Stworzenie statku
            var exampleShip = new CargoShip(maxSpeed: 30, maxContainerCount: 5, maxWeight: 200000);

            // Lista dozwolonych typów produktów dla kontenera chłodniczego
            var allowedProductTypesForRefrigeratedContainer = new List<string> { "Ice cream", "Fish", "Vegetables" };

            // Stworzenie kontenerów
            var refrigeratedContainer = new RefrigeratedContainer(height: 250, ownWeight: 2200, depth: 400, maxLoadCapacity: 30000, temperature: -18, allowedProductTypes: allowedProductTypesForRefrigeratedContainer);
            var liquidContainer = new LiquidContainer(height: 250, ownWeight: 2200, depth: 400, maxLoadCapacity: 30000, isHazardous: false);
            var gasContainer = new GasContainer(height: 250, ownWeight: 2100, depth: 400, maxLoadCapacity: 25000, pressure: 100);

            // Ustawienie typu produktu przed załadowaniem ładunku na kontener chłodniczy
            refrigeratedContainer.pendingProductType = "Ice cream";

            // Załadowanie ładunku do kontenerów
            refrigeratedContainer.LoadCargo(10000);
            liquidContainer.LoadCargo(20000);
            gasContainer.LoadCargo(15000);

            // Załadowanie kontenera na statek
            exampleShip.AddContainer(refrigeratedContainer);

            // Załadowanie listy kontenerów na statek
            var exampleContainers = new List<IContainer> { liquidContainer, gasContainer };
            exampleShip.AddManyContainers(exampleContainers);

            // Wypisanie informacji o statku i jego ładunku
            exampleShip.PrintShipInfo();

            Console.WriteLine
                (
                               "------------------------------------------------------------------------------------------------------------------"
                                          );
            // Usunięcie kontenera ze statku
            exampleShip.RemoveContainer(liquidContainer.serialNumber);

            // Rozładowanie kontenera
            gasContainer.UnloadCargo();

            // Zastąpienie kontenera na statku o danym numerze innym kontenerem
            var newLiquidContainer = new LiquidContainer(height: 250, ownWeight: 2300, depth: 400, maxLoadCapacity: 30000, isHazardous: true);
            newLiquidContainer.LoadCargo(10000);
            exampleShip.ReplaceContainer(gasContainer.serialNumber, newLiquidContainer);

            // Możliwość przeniesienie kontenera między dwoma statkami
            var anotherShip = new CargoShip(maxSpeed: 25, maxContainerCount: 5, maxWeight: 150000);
            exampleShip.TransferContainerToAnotherShip(newLiquidContainer.serialNumber, anotherShip);

            // Wypisanie informacji o danym kontenerze
            refrigeratedContainer.PrintContainerInfo();
            liquidContainer.PrintContainerInfo();
            gasContainer.PrintContainerInfo();
            newLiquidContainer.PrintContainerInfo();

            // Wypisanie informacji o statku i jego ładunku
            exampleShip.PrintShipInfo();

            // Wypisanie informacji o drugim statku i jego ładunku
            anotherShip.PrintShipInfo();

            bool running = true;
            while (running)
            {
                Console.Clear();
                DisplayShips();
                DisplayContainers();
                DisplayMenu();

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddShip();
                        break;
                    case "2":
                        RemoveShip();
                        break;
                    case "3":
                        LoadCargoIntoContainer();
                        break;
                    case "4":
                        UnloadContainer();
                        break;
                    case "5":
                        AddContainer();
                        break;
                    case "6":
                        LoadContainerOntoShip();
                        break;
                    case "7":
                        UnloadContainerFromShip();
                        break;
                    case "8":
                        TransferContainerToAnotherShip();
                        break;
                    case "9":
                        ReplaceContainerOnShip();
                        break;
                    case "10":
                        PrintContainerInfo();
                        break;
                    case "11":
                        PrintShipInfo();
                        break;
                    case "12":
                        LoadListOfContainersOntoShip();
                        break;
                    case "13":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Nieznana opcja.");
                        break;
                }

                if (running)
                {
                    Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
                    Console.ReadKey();
                }
            }

            static void DisplayShips()
            {
                Console.WriteLine("Lista kontenerowców:");
                if (ships.Count == 0)
                {
                    Console.WriteLine("Brak");
                }
                else
                {
                    for (int i = 0; i < ships.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. Statek {i + 1} (speed={ships[i].maxSpeed}, maxContainerNum={ships[i].maxContainerCount}, maxWeight={ships[i].maxWeight})");
                    }
                }
            }

            static void DisplayContainers()
            {
                Console.WriteLine("Lista kontenerów:");
                if (containers.Count == 0)
                {
                    Console.WriteLine("Brak");
                }
                else
                {
                    for (int i = 0; i < containers.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. Kontener {containers[i].serialNumber}");
                    }
                }
            }

            static void DisplayMenu()
            {
                Console.WriteLine("Możliwe akcje:");
                Console.WriteLine("1. Dodaj kontenerowiec");
                Console.WriteLine("2. Usun kontenerowiec");
                Console.WriteLine("3. Dodaj kontener");
                Console.WriteLine("4. Załaduj ładunek na kontener");
                Console.WriteLine("5. Rozładuj ładunek z kontenera");
                Console.WriteLine("6. Załaduj kontener na statek");
                Console.WriteLine("7. Rozładuj kontener ze statku");
                Console.WriteLine("8. Przenieś kontener między statkami");
                Console.WriteLine("9. Zastąp kontener na statku");
                Console.WriteLine("10. Wyświetl informacje o kontenerze");
                Console.WriteLine("11. Wyświetl informacje o statku");
                Console.WriteLine("12. Załaduj listę kontenerów na statek");
                Console.WriteLine("13. Wyjście");
                Console.Write("Wybierz opcję: ");
            }

            static void AddShip()
            {
                Console.Write("Podaj maksymalną prędkość statku: ");
                int maxSpeed = int.Parse(Console.ReadLine());
                Console.Write("Podaj maksymalną liczbę kontenerów: ");
                int maxContainerCount = int.Parse(Console.ReadLine());
                Console.Write("Podaj maksymalną wagę (w tonach): ");
                double maxWeight = double.Parse(Console.ReadLine());

                var ship = new CargoShip(maxSpeed, maxContainerCount, maxWeight);
                ships.Add(ship);
                Console.WriteLine("Dodano nowy statek.");
            }

            static void RemoveShip()
            {
                Console.Write("Podaj numer statku do usunięcia: ");
                int index = int.Parse(Console.ReadLine()) - 1;
                if (index >= 0 && index < ships.Count)
                {
                    ships.RemoveAt(index);
                    Console.WriteLine("Usunięto statek.");
                }
                else
                {
                    Console.WriteLine("Nieprawidłowy numer statku.");
                }
            }

            static void PrintShipInfo()
            {
                Console.Write("Podaj numer statku, o którym wyświetlić informacje: ");
                int index = int.Parse(Console.ReadLine()) - 1;
                if (index >= 0 && index < ships.Count)
                {
                    ships[index].PrintShipInfo();
                }
                else
                {
                    Console.WriteLine("Nieprawidłowy numer statku.");
                }
            }

            static void AddContainer()
            {
                Console.WriteLine("Wybierz typ kontenera:");
                Console.WriteLine("1. RefrigeratedContainer");
                Console.WriteLine("2. LiquidContainer");
                Console.WriteLine("3. GasContainer");
                string typeChoice = Console.ReadLine();

                switch (typeChoice)
                {
                    case "1":
                        AddRefrigeratedContainer();
                        break;
                    case "2":
                        AddLiquidContainer();
                        break;
                    case "3":
                        AddGasContainer();
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór.");
                        break;
                }
            }

            static void AddRefrigeratedContainer()
            {
                Console.Write("Podaj wysokość: ");
                double height = Convert.ToDouble(Console.ReadLine());
                Console.Write("Podaj wagę własną: ");
                double ownWeight = Convert.ToDouble(Console.ReadLine());
                Console.Write("Podaj głębokość: ");
                double depth = Convert.ToDouble(Console.ReadLine());
                Console.Write("Podaj maksymalną ładowność: ");
                double maxLoadCapacity = Convert.ToDouble(Console.ReadLine());
                Console.Write("Podaj temperaturę: ");
                double temperature = Convert.ToDouble(Console.ReadLine());
                Console.Write("Podaj dozwolony typ produktu: ");
                string productType = Console.ReadLine();

                var container = new RefrigeratedContainer(height, ownWeight, depth, maxLoadCapacity, temperature, new List<string> { productType });
                containers.Add(container);
                Console.WriteLine("Dodano kontener chłodniczy.");
            }

            static void AddLiquidContainer()
            {
                Console.Write("Podaj wysokość: ");
                double height = Convert.ToDouble(Console.ReadLine());
                Console.Write("Podaj wagę własną: ");
                double ownWeight = Convert.ToDouble(Console.ReadLine());
                Console.Write("Podaj głębokość: ");
                double depth = Convert.ToDouble(Console.ReadLine());
                Console.Write("Podaj maksymalną ładowność: ");
                double maxLoadCapacity = Convert.ToDouble(Console.ReadLine());
                Console.Write("Czy transportuje ładunek niebezpieczny? (tak/nie): ");
                bool isHazardous = Console.ReadLine().Trim().ToLower() == "tak";

                var container = new LiquidContainer(height, ownWeight, depth, maxLoadCapacity, isHazardous);
                containers.Add(container);
                Console.WriteLine("Dodano kontener na płyny.");
            }

            static void AddGasContainer()
            {
                Console.Write("Podaj wysokość: ");
                double height = Convert.ToDouble(Console.ReadLine());
                Console.Write("Podaj wagę własną: ");
                double ownWeight = Convert.ToDouble(Console.ReadLine());
                Console.Write("Podaj głębokość: ");
                double depth = Convert.ToDouble(Console.ReadLine());
                Console.Write("Podaj maksymalną ładowność: ");
                double maxLoadCapacity = Convert.ToDouble(Console.ReadLine());
                Console.Write("Podaj ciśnienie gazu w atmosferach: ");
                double pressure = Convert.ToDouble(Console.ReadLine());

                var container = new GasContainer(height, ownWeight, depth, maxLoadCapacity, pressure);
                containers.Add(container);
                Console.WriteLine("Dodano kontener na gaz.");
            }

            static void LoadCargoIntoContainer()
            {
                Console.Write("Podaj numer kontenera, do którego chcesz załadować ładunek: ");
                int containerIndex = Convert.ToInt32(Console.ReadLine()) - 1;

                if (containerIndex < 0 || containerIndex >= containers.Count)
                {
                    Console.WriteLine("Nieprawidłowy numer kontenera.");
                    return;
                }

                Console.Write("Podaj masę ładunku do załadowania (w kg): ");
                double mass = Convert.ToDouble(Console.ReadLine());

                try
                {
                    containers[containerIndex].LoadCargo(mass);
                    Console.WriteLine("Ładunek załadowany.");
                }
                catch (OverfillException ex)
                {
                    Console.WriteLine($"Nie można załadować ładunku: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił błąd: {ex.Message}");
                }
            }

            static void UnloadContainer()
            {
                Console.Write("Podaj numer kontenera do rozładowania: ");
                int containerIndex = Convert.ToInt32(Console.ReadLine()) - 1;

                if (containerIndex < 0 || containerIndex >= containers.Count)
                {
                    Console.WriteLine("Nieprawidłowy numer kontenera.");
                    return;
                }

                containers[containerIndex].UnloadCargo();
                Console.WriteLine("Kontener rozładowany.");
            }


            static void LoadContainerOntoShip()
            {
                if (ships.Count == 0 || containers.Count == 0)
                {
                    Console.WriteLine("Brak dostępnych statków lub kontenerów.");
                    return;
                }

                Console.Write("Podaj numer statku: ");
                int shipIndex = Convert.ToInt32(Console.ReadLine()) - 1;
                if (shipIndex < 0 || shipIndex >= ships.Count)
                {
                    Console.WriteLine("Nieprawidłowy numer statku.");
                    return;
                }

                Console.Write("Podaj numer kontenera: ");
                int containerIndex = Convert.ToInt32(Console.ReadLine()) - 1;
                if (containerIndex < 0 || containerIndex >= containers.Count)
                {
                    Console.WriteLine("Nieprawidłowy numer kontenera.");
                    return;
                }

                try
                {
                    ships[shipIndex].AddContainer(containers[containerIndex]);
                    Console.WriteLine("Kontener załadowany na statek.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Nie udało się załadować kontenera: {ex.Message}");
                }
            }

            static void UnloadContainerFromShip()
            {

                Console.Write("Podaj numer statku, z którego chcesz rozładować kontener: ");
                int shipIndex = Convert.ToInt32(Console.ReadLine()) - 1;

                if (shipIndex < 0 || shipIndex >= ships.Count)
                {
                    Console.WriteLine("Nieprawidłowy numer statku.");
                    return;
                }

                var selectedShip = ships[shipIndex];

                Console.Write("Podaj numer seryjny kontenera do rozładowania: ");
                string serialNumber = Console.ReadLine();

                try
                {
                    selectedShip.RemoveContainer(serialNumber);
                    Console.WriteLine("Kontener został rozładowany.");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił błąd podczas rozładowywania kontenera: {ex.Message}");
                }
            }

            static void LoadListOfContainersOntoShip()
            {
                if (ships.Count == 0)
                {
                    Console.WriteLine("Brak dostępnych statków.");
                    return;
                }
                if (containers.Count == 0)
                {
                    Console.WriteLine("Brak dostępnych kontenerów.");
                    return;
                }

                Console.Write("Podaj numer statku, na który chcesz załadować kontenery: ");
                int shipIndex = Convert.ToInt32(Console.ReadLine()) - 1;

                if (shipIndex < 0 || shipIndex >= ships.Count)
                {
                    Console.WriteLine("Nieprawidłowy numer statku.");
                    return;
                }

                CargoShip selectedShip = ships[shipIndex];
                List<IContainer> containersToLoad = new List<IContainer>();

                Console.WriteLine("Podaj numery kontenerów do załadunku (zakończ wprowadzanie pustym wierszem):");
                while (true)
                {
                    string input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        break;
                    }

                    int containerIndex = int.Parse(input) - 1;
                    if (containerIndex >= 0 && containerIndex < containers.Count)
                    {
                        containersToLoad.Add(containers[containerIndex]);
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowy numer kontenera.");
                    }
                }

                try
                {
                    selectedShip.AddManyContainers(containersToLoad);
                    Console.WriteLine("Kontenery zostały załadowane na statek.");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            static void ReplaceContainerOnShip()
            {
                if (ships.Count == 0)
                {
                    Console.WriteLine("Brak dostępnych kontenerowców.");
                    return;
                }

                Console.Write("Podaj numer statku, na którym chcesz zastąpić kontener: ");
                int shipIndex = Convert.ToInt32(Console.ReadLine()) - 1;

                if (shipIndex < 0 || shipIndex >= ships.Count)
                {
                    Console.WriteLine("Nieprawidłowy numer statku.");
                    return;
                }

                var selectedShip = ships[shipIndex];

                Console.Write("Podaj numer seryjny kontenera do zastąpienia: ");
                string oldSerialNumber = Console.ReadLine();

                Console.WriteLine("Wybierz nowy kontener do załadowania:");
                for (int i = 0; i < containers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Kontener {containers[i].serialNumber}");
                }

                Console.Write("Podaj numer nowego kontenera: ");
                int newContainerIndex = Convert.ToInt32(Console.ReadLine()) - 1;

                if (newContainerIndex < 0 || newContainerIndex >= containers.Count)
                {
                    Console.WriteLine("Nieprawidłowy wybór kontenera.");
                    return;
                }

                var newContainer = containers[newContainerIndex];

                try
                {
                    selectedShip.ReplaceContainer(oldSerialNumber, newContainer);
                    Console.WriteLine("Kontener został zastąpiony.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Nie udało się zastąpić kontenera: {ex.Message}");
                }
            }


            static void TransferContainerToAnotherShip()
            {
                Console.Write("Podaj numer statku źródłowego: ");
                int sourceShipIndex = Convert.ToInt32(Console.ReadLine()) - 1;
                Console.Write("Podaj numer statku docelowego: ");
                int targetShipIndex = Convert.ToInt32(Console.ReadLine()) - 1;

                if (sourceShipIndex < 0 || sourceShipIndex >= ships.Count || targetShipIndex < 0 || targetShipIndex >= ships.Count)
                {
                    Console.WriteLine("Nieprawidłowy numer statku.");
                    return;
                }

                Console.Write("Podaj numer seryjny kontenera do przeniesienia: ");
                string serialNumber = Console.ReadLine();

                try
                {
                    ships[sourceShipIndex].TransferContainerToAnotherShip(serialNumber, ships[targetShipIndex]);
                    Console.WriteLine("Kontener został przeniesiony.");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił błąd podczas przenoszenia kontenera: {ex.Message}");
                }
            }


            static void PrintContainerInfo()
            {
                if (containers.Count == 0)
                {
                    Console.WriteLine("Brak kontenerów do wyświetlenia.");
                    return;
                }

                Console.WriteLine("Wybierz kontener do wyświetlenia informacji:");
                for (int i = 0; i < containers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Kontener {containers[i].serialNumber}");
                }

                Console.Write("Podaj numer kontenera: ");
                int containerIndex = Convert.ToInt32(Console.ReadLine()) - 1;

                if (containerIndex < 0 || containerIndex >= containers.Count)
                {
                    Console.WriteLine("Nieprawidłowy wybór kontenera.");
                    return;
                }

                containers[containerIndex].PrintContainerInfo();
            }


        }
    }
}
