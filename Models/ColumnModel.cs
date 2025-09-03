using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardViewCreator.Models
{
    public class ColumnModel
    {
        public string DisplayName { get; set; }
        public string LogicalName { get; set; }
        public string Order { get; set; }
        public string Width { get; set; }
    }
}
