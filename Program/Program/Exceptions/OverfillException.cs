using System;

namespace ContainerManagementSystem.Exceptions
{
    // Exception thrown when loading exceeds the container's capacity.
    public class OverfillException : Exception
    {
        public OverfillException(string message) : base(message) { }
    }
}
