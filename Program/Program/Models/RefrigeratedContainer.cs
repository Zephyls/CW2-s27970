using ContainerManagementSystem.Exceptions;

namespace ContainerManagementSystem.Models
{
    // Refrigerated container class.
    public class RefrigeratedContainer : Container
    {
        public string ProductType { get; private set; }
        public double RequiredTemperature { get; private set; }
        public double ContainerTemperature { get; private set; }
        public double Height { get; private set; }  // Height in centimeters.
        public double Depth { get; private set; }   // Depth in centimeters.

        public RefrigeratedContainer(double tareWeight, double maxLoad,
                                     string productType, double requiredTemp, double containerTemp,
                                     double height, double depth)
            : base("C", tareWeight, maxLoad)
        {
            ProductType = productType;
            RequiredTemperature = requiredTemp;
            ContainerTemperature = containerTemp;
            Height = height;
            Depth = depth;
        }

        public override void Load(double weight)
        {
            if (weight > MaxLoad)
                throw new OverfillException($"Loading exceeds capacity for refrigerated container {SerialNumber}.");

            // Check temperature: the container's temperature must not be lower than the product's required temperature.
            if (ContainerTemperature < RequiredTemperature)
                throw new OverfillException($"Temperature {ContainerTemperature}°C is too low for product {ProductType} (requires {RequiredTemperature}°C) in container {SerialNumber}.");

            CurrentLoad = weight;
        }

        public override string ToString()
        {
            return base.ToString() + $" | Product: {ProductType}, Temp: {ContainerTemperature}°C (req: {RequiredTemperature}°C)";
        }
    }
}
