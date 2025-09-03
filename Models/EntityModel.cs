using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardViewCreator.Models
{
    public class EntityModel
    {
        public string DisplayName { get; set; }
        public string LogicalName { get; set; }
        public string PrimaryNameAttribute { get; set; }
        public int? ObjectTypeCode { get; set; }
        public bool? IsValidForAdvancedFind { get; set; }
        public string PrimaryIdAttribute { get; set; }

        public string[] ToStringArray()
        {
            return new[]
            {
                DisplayName,
                LogicalName,
                PrimaryNameAttribute,
                IsValidForAdvancedFind.ToString()
            };
        }
    }
}
