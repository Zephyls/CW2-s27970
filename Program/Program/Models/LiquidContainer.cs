using ContainerManagementSystem.Interfaces;
using ContainerManagementSystem.Exceptions;

namespace ContainerManagementSystem.Models
{
    // Liquid container class.
    public class LiquidContainer : Container, IHazardNotifier
    {
        public bool IsHazardous { get; private set; }

        public LiquidContainer(double tareWeight, double maxLoad, bool isHazardous)
            : base("L", tareWeight, maxLoad)
        {
            IsHazardous = isHazardous;
        }

        public override void Load(double weight)
        {
            // If the container carries hazardous material, only load up to 50% capacity; otherwise, up to 90%.
            double limit = IsHazardous ? MaxLoad * 0.5 : MaxLoad * 0.9;
            if (weight > limit)
            {
                NotifyHazard($"Overfill attempt for container {SerialNumber} with {weight} kg (limit {limit} kg).");
                throw new OverfillException($"Cannot load {weight} kg into container {SerialNumber}.");
            }
            CurrentLoad = weight;
        }

        public void NotifyHazard(string message)
        {
            System.Console.WriteLine($"[HAZARD] {message}");
        }
    }
}
