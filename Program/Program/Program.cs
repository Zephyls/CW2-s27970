using System;
using System.Collections.Generic;
using System.Linq;
using ContainerManagementSystem.Models;
using ContainerManagementSystem.Operations;
using ContainerManagementSystem.Exceptions;

namespace ContainerManagementSystem
{
    class Program
    {
        // List of free containers (not yet loaded onto a ship).
        static List<Container> freeContainers = new List<Container>();
        // List of container ships.
        static List<ContainerShip> ships = new List<ContainerShip>();

        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                PrintStatus();
                PrintMenu();
                Console.Write("Enter your choice (number): ");
                string choice = Console.ReadLine();
                Console.WriteLine();
                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddContainerShip();
                            break;
                        case "2":
                            RemoveContainerShip();
                            break;
                        case "3":
                            AddContainer();
                            break;
                        case "4":
                            LoadContainerOntoShip();
                            break;
                        case "5":
                            UnloadContainerFromShip();
                            break;
                        case "6":
                            ReplaceContainerOnShip();
                            break;
                        case "7":
                            TransferContainerBetweenShips();
                            break;
                        case "8":
                            PrintContainerInfo();
                            break;
                        case "9":
                            PrintShipInfo();
                            break;
                        case "0":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                Console.Clear();
            }
        }

        static void PrintStatus()
        {
            Console.WriteLine("==== Ship List ====");
            if (ships.Count == 0)
                Console.WriteLine("  No ships available.");
            else
            {
                foreach (var ship in ships)
                    Console.WriteLine(ship.ToString());
            }
            Console.WriteLine("\n==== Free Container List ====");
            if (freeContainers.Count == 0)
                Console.WriteLine("  No free containers available.");
            else
            {
                foreach (var container in freeContainers)
                    Console.WriteLine(" - " + container.ToString());
            }
            Console.WriteLine();
        }

        static void PrintMenu()
        {
            Console.WriteLine("Available operations:");
            Console.WriteLine("1. Add container ship");
            Console.WriteLine("2. Remove container ship");
            Console.WriteLine("3. Add container");
            Console.WriteLine("4. Load container onto ship");
            Console.WriteLine("5. Unload container from ship");
            Console.WriteLine("6. Replace container on ship");
            Console.WriteLine("7. Transfer container between ships");
            Console.WriteLine("8. View container information (by serial number)");
            Console.WriteLine("9. View ship information and load");
            Console.WriteLine("0. Exit");
        }

        static void AddContainerShip()
        {
            Console.Write("Enter ship name: ");
            string name = Console.ReadLine();
            Console.Write("Enter maximum speed (knots): ");
            double speed = double.Parse(Console.ReadLine());
            Console.Write("Enter maximum container count: ");
            int maxCount = int.Parse(Console.ReadLine());
            Console.Write("Enter maximum total weight (tons): ");
            double maxWeight = double.Parse(Console.ReadLine());

            ContainerShip ship = new ContainerShip(name, speed, maxCount, maxWeight);
            ships.Add(ship);
            Console.WriteLine("Ship added: " + name);
        }

        static void RemoveContainerShip()
        {
            Console.Write("Enter ship name to remove: ");
            string name = Console.ReadLine();
            var ship = ships.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (ship != null)
            {
                ships.Remove(ship);
                Console.WriteLine("Ship removed: " + name);
            }
            else
            {
                Console.WriteLine("Ship not found: " + name);
            }
        }

        static void AddContainer()
        {
            Console.WriteLine("Select container type:");
            Console.WriteLine("1. Liquid container");
            Console.WriteLine("2. Gas container");
            Console.WriteLine("3. Refrigerated container");
            Console.Write("Choice: ");
            string typeChoice = Console.ReadLine();

            Console.Write("Enter tare weight (kg): ");
            double tare = double.Parse(Console.ReadLine());
            Console.Write("Enter maximum load (kg): ");
            double maxLoad = double.Parse(Console.ReadLine());

            Container container = null;
            switch (typeChoice)
            {
                case "1":
                    Console.Write("Does the container contain hazardous material? (y/n): ");
                    bool hazardous = Console.ReadLine().ToLower() == "y";
                    container = new LiquidContainer(tare, maxLoad, hazardous);
                    break;
                case "2":
                    Console.Write("Enter pressure (atmospheres): ");
                    double pressure = double.Parse(Console.ReadLine());
                    container = new GasContainer(tare, maxLoad, pressure);
                    break;
                case "3":
                    Console.Write("Enter product type (e.g., milk, banana, ...): ");
                    string product = Console.ReadLine();
                    Console.Write("Enter required temperature (°C): ");
                    double reqTemp = double.Parse(Console.ReadLine());
                    Console.Write("Enter current container temperature (°C): ");
                    double contTemp = double.Parse(Console.ReadLine());
                    Console.Write("Enter height (cm): ");
                    double height = double.Parse(Console.ReadLine());
                    Console.Write("Enter depth (cm): ");
                    double depth = double.Parse(Console.ReadLine());
                    container = new RefrigeratedContainer(tare, maxLoad, product, reqTemp, contTemp, height, depth);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    return;
            }
            freeContainers.Add(container);
            Console.WriteLine("Container created: " + container.SerialNumber);
        }

        static void LoadContainerOntoShip()
        {
            if (freeContainers.Count == 0)
            {
                Console.WriteLine("No free container available for loading.");
                return;
            }
            Console.Write("Enter serial number of the container to load: ");
            string serial = Console.ReadLine();
            var container = freeContainers.FirstOrDefault(c => c.SerialNumber.Equals(serial, StringComparison.OrdinalIgnoreCase));
            if (container == null)
            {
                Console.WriteLine("Container with serial number not found: " + serial);
                return;
            }
            Console.Write("Enter ship name to load the container onto: ");
            string shipName = Console.ReadLine();
            var ship = ships.FirstOrDefault(s => s.Name.Equals(shipName, StringComparison.OrdinalIgnoreCase));
            if (ship == null)
            {
                Console.WriteLine("Ship not found: " + shipName);
                return;
            }
            Console.Write("Enter load weight (kg): ");
            double weight = double.Parse(Console.ReadLine());
            try
            {
                container.Load(weight);
                ship.AddContainer(container);
                freeContainers.Remove(container);
                Console.WriteLine($"Container {serial} loaded onto ship {shipName} with {weight} kg of load.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while loading container: " + ex.Message);
            }
        }

        static void UnloadContainerFromShip()
        {
            Console.Write("Enter the ship name containing the container to unload: ");
            string shipName = Console.ReadLine();
            var ship = ships.FirstOrDefault(s => s.Name.Equals(shipName, StringComparison.OrdinalIgnoreCase));
            if (ship == null)
            {
                Console.WriteLine("Ship not found: " + shipName);
                return;
            }
            Console.Write("Enter serial number of the container to unload: ");
            string serial = Console.ReadLine();
            if (ship.UnloadContainer(serial))
            {
                Console.WriteLine($"Container {serial} on ship {shipName} has been unloaded (note: gas containers retain 5% load).");
            }
            else
                Console.WriteLine("Container not found on ship " + shipName + ": " + serial);
        }

        static void ReplaceContainerOnShip()
        {
            Console.Write("Enter ship name for container replacement: ");
            string shipName = Console.ReadLine();
            var ship = ships.FirstOrDefault(s => s.Name.Equals(shipName, StringComparison.OrdinalIgnoreCase));
            if (ship == null)
            {
                Console.WriteLine("Ship not found: " + shipName);
                return;
            }
            Console.Write("Enter serial number of the container to replace: ");
            string oldSerial = Console.ReadLine();
            var oldContainer = ship.GetContainer(oldSerial);
            if (oldContainer == null)
            {
                Console.WriteLine($"Container {oldSerial} not found on ship {shipName}.");
                return;
            }
            Console.WriteLine("Create a new container as a replacement:");
            AddContainer(); // Creates a new container and adds it to freeContainers.
            Console.Write("Enter the serial number of the new container just created: ");
            string newSerial = Console.ReadLine();
            var newContainer = freeContainers.FirstOrDefault(c => c.SerialNumber.Equals(newSerial, StringComparison.OrdinalIgnoreCase));
            if (newContainer == null)
            {
                Console.WriteLine("New container with serial number not found: " + newSerial);
                return;
            }
            Console.Write("Enter load weight for the new container (kg): ");
            double weight = double.Parse(Console.ReadLine());
            try
            {
                newContainer.Load(weight);
                if (ship.ReplaceContainer(oldSerial, newContainer))
                {
                    freeContainers.Remove(newContainer);
                    Console.WriteLine($"Container {oldSerial} successfully replaced with {newSerial} on ship {shipName}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while replacing container: " + ex.Message);
            }
        }

        static void TransferContainerBetweenShips()
        {
            Console.Write("Enter source ship name: ");
            string sourceName = Console.ReadLine();
            var sourceShip = ships.FirstOrDefault(s => s.Name.Equals(sourceName, StringComparison.OrdinalIgnoreCase));
            if (sourceShip == null)
            {
                Console.WriteLine("Ship not found: " + sourceName);
                return;
            }
            Console.Write("Enter serial number of the container to transfer: ");
            string serial = Console.ReadLine();
            Console.Write("Enter destination ship name: ");
            string destName = Console.ReadLine();
            var destShip = ships.FirstOrDefault(s => s.Name.Equals(destName, StringComparison.OrdinalIgnoreCase));
            if (destShip == null)
            {
                Console.WriteLine("Ship not found: " + destName);
                return;
            }
            if (ShipOperations.TransferContainer(serial, sourceShip, destShip))
                Console.WriteLine($"Container {serial} has been transferred from {sourceName} to {destName}.");
            else
                Console.WriteLine("Container transfer failed.");
        }

        static void PrintContainerInfo()
        {
            Console.Write("Enter serial number of the container to view: ");
            string serial = Console.ReadLine();
            // Look in free container list.
            var container = freeContainers.FirstOrDefault(c => c.SerialNumber.Equals(serial, StringComparison.OrdinalIgnoreCase));
            if (container != null)
            {
                Console.WriteLine("Container information:");
                Console.WriteLine(container.ToString());
                return;
            }
            // If not found, search on all ships.
            foreach (var ship in ships)
            {
                container = ship.GetContainer(serial);
                if (container != null)
                {
                    Console.WriteLine($"Container information on ship {ship.Name}:");
                    Console.WriteLine(container.ToString());
                    return;
                }
            }
            Console.WriteLine("Container with serial number not found: " + serial);
        }

        static void PrintShipInfo()
        {
            Console.Write("Enter ship name to view information: ");
            string name = Console.ReadLine();
            var ship = ships.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (ship != null)
                Console.WriteLine(ship.ToString());
            else
                Console.WriteLine("Ship not found: " + name);
        }
    }
}
