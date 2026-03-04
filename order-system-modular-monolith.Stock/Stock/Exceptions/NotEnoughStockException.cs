using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.Stock.Exceptions
{
    public class NotEnoughStockException : Exception
    {
        public NotEnoughStockException()
        {
        }

        public NotEnoughStockException(string message)
            : base(message)
        {
            message = $"Not enough stock for product: {message}";
        }

        public NotEnoughStockException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }


}
