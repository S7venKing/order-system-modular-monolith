using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.BuildingBlocks.Exceptions
{
    public class DomainException : Exception
    {
        public string Code { get; }

        public DomainException(string code, string message)
            : base(message)
        {
            Code = code;
        }
    }
}
