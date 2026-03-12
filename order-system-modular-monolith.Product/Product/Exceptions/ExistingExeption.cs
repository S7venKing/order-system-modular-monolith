using System;

namespace order_system_modular_monolith.Product.Exceptions
{
    public class ExistingException : Exception
    {
        public ExistingException()
        {
        }

        public ExistingException(string message)
            : base(message)
        {
        }

        public ExistingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}