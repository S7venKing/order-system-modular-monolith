using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.BuildingBlocks.Domain
{
    public class Behaviors
    {

        public interface IAuditable
        {
            DateTime CreatedAt { get; set; }
            long CreatedBy { get; set; }
            DateTime? LastModified { get; set; }
            long? LastModifiedBy { get; set; }
        }

        public interface ISoftDelete
        {
            bool IsDeleted { get; set; }
        }

        public interface IVersioned
        {
            long Version { get; set; }
        }
    }
}
