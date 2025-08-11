using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardViewCreator
{
    internal class ViewInfo
    {
        public Guid Guid { get; set; }
        public string Entity { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string FetchXml { get; set; }
        public string LayoutXml { get; set; }
        public int? QueryType { get; set; }
    }
}
