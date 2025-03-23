using System;

namespace ContainerManagementSystem.Models
{
    public abstract class Container
    {
        private static int counter = 0;
        public string SerialNumber { get; private set; }
        public double TareWeight { get; private set; }  // Weight of the empty container.
        public double MaxLoad { get; private set; }     // Maximum load capacity in kg.
        public double CurrentLoad { get; protected set; }

        public Container(string type, double tareWeight, double maxLoad)
        {
            TareWeight = tareWeight;
            MaxLoad = maxLoad;
            SerialNumber = GenerateSerial(type);
        }

        private string GenerateSerial(string type)
        {
            counter++;
            // Format: KON-{type}-{number}
            return $"KON-{type}-{counter}";
        }

        // Load method (must check capacity and throw an exception if exceeded)
        public abstract void Load(double weight);

        // Unload the container (by default, empties the entire load).
        public virtual void Unload()
        {
            CurrentLoad = 0;
        }

        public override string ToString()
        {
            return $"{SerialNumber} | Load: {CurrentLoad}/{MaxLoad} kg, Tare: {TareWeight} kg";
        }
    }
}
