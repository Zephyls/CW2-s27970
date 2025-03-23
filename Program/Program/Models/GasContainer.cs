using ContainerManagementSystem.Interfaces;
using ContainerManagementSystem.Exceptions;

namespace ContainerManagementSystem.Models
{
    // Gas container class.
    public class GasContainer : Container, IHazardNotifier
    {
        public double Pressure { get; private set; }  // Pressure in atmospheres.

        public GasContainer(double tareWeight, double maxLoad, double pressure)
            : base("G", tareWeight, maxLoad)
        {
            Pressure = pressure;
        }

        public override void Load(double weight)
        {
            if (weight > MaxLoad)
            {
                NotifyHazard($"Overfill attempt for gas container {SerialNumber}.");
                throw new OverfillException($"Loading {weight} kg exceeds the maximum capacity of container {SerialNumber}.");
            }
            CurrentLoad = weight;
        }

        // When unloading a gas container, 5% of the load remains.
        public override void Unload()
        {
            CurrentLoad *= 0.05;
        }

        public void NotifyHazard(string message)
        {
            System.Console.WriteLine($"[HAZARD] {message}");
        }
    }
}
