using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardViewCreator
{
    public class SolutionInfo
    {
        public string SolutionId { get; set; }
        public string DisplayName { get; set; }
        public string Version { get; set; }
        public string Publisher { get; set; }

        public override string ToString() => DisplayName;
    }
}
