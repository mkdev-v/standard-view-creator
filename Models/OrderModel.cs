using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardViewCreator.Models
{
    public class OrderModel
    {
        public string LogicalName { get; set; }
        public int Priority { get; set; }
        public string Descending { get; set; }
    }
}
