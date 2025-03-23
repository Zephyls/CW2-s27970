using System;
using ContainerManagementSystem.Models;

namespace ContainerManagementSystem.Operations
{
    // Static class for transferring a container between two ships.
    public static class ShipOperations
    {
        public static bool TransferContainer(string serialNumber, ContainerShip source, ContainerShip destination)
        {
            var container = source.GetContainer(serialNumber);
            if (container == null)
                return false;
            try
            {
                destination.AddContainer(container);
                source.RemoveContainer(serialNumber);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Container transfer failed: " + ex.Message);
                return false;
            }
        }
    }
}
