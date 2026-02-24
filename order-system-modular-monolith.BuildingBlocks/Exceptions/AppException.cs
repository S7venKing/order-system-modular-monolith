using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.BuildingBlocks.Exceptions
{
    public abstract class AppException : Exception
    {
        public string Code { get; }

        protected AppException(string code, string message)
            : base(message)
        {
            Code = code;
        }
    }
}
