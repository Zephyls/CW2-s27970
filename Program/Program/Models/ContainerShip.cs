using System;
using System.Collections.Generic;
using System.Linq;

namespace ContainerManagementSystem.Models
{
    // Class representing a container ship.
    public class ContainerShip
    {
        public string Name { get; set; }
        public double MaxSpeed { get; set; }          // Maximum speed in knots.
        public int MaxContainerCount { get; set; }      // Maximum number of containers.
        public double MaxTotalWeight { get; set; }      // Maximum total container weight (tons).
        public List<Container> Containers { get; private set; }

        public ContainerShip(string name, double maxSpeed, int maxContainerCount, double maxTotalWeight)
        {
            Name = name;
            MaxSpeed = maxSpeed;
            MaxContainerCount = maxContainerCount;
            MaxTotalWeight = maxTotalWeight;
            Containers = new List<Container>();
        }

        // Calculate the current total weight on the ship (including container tare weights).
        private double CurrentTotalWeight()
        {
            return Containers.Sum(c => c.TareWeight + c.CurrentLoad);
        }

        public void AddContainer(Container container)
        {
            if (Containers.Count >= MaxContainerCount)
                throw new InvalidOperationException("The container count on the ship has reached its limit.");

            // Convert maximum weight from tons to kilograms.
            double maxWeightKg = MaxTotalWeight * 1000;
            if (CurrentTotalWeight() + container.TareWeight + container.CurrentLoad > maxWeightKg)
                throw new InvalidOperationException("Total container weight exceeds the ship's limit.");

            Containers.Add(container);
        }

        public bool RemoveContainer(string serialNumber)
        {
            var container = Containers.FirstOrDefault(c => c.SerialNumber == serialNumber);
            if (container != null)
            {
                Containers.Remove(container);
                return true;
            }
            return false;
        }

        public bool UnloadContainer(string serialNumber)
        {
            var container = Containers.FirstOrDefault(c => c.SerialNumber == serialNumber);
            if (container != null)
            {
                container.Unload();
                return true;
            }
            return false;
        }

        public bool ReplaceContainer(string serialNumber, Container newContainer)
        {
            var oldContainer = Containers.FirstOrDefault(c => c.SerialNumber == serialNumber);
            if (oldContainer != null)
            {
                Containers.Remove(oldContainer);
                try
                {
                    AddContainer(newContainer);
                    return true;
                }
                catch (Exception ex)
                {
                    // If adding the new container fails, revert to the old container.
                    Containers.Add(oldContainer);
                    throw new InvalidOperationException("Container replacement failed: " + ex.Message);
                }
            }
            return false;
        }

        public Container GetContainer(string serialNumber)
        {
            return Containers.FirstOrDefault(c => c.SerialNumber == serialNumber);
        }

        public override string ToString()
        {
            string info = $"{Name} (Speed: {MaxSpeed} knots, Max containers: {MaxContainerCount}, Max total weight: {MaxTotalWeight} tons)";
            if (Containers.Count == 0)
                info += "\n  No containers.";
            else
            {
                info += "\n  Container list:";
                foreach (var c in Containers)
                    info += "\n   - " + c.ToString();
            }
            return info;
        }
    }
}
