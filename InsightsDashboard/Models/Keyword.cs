using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsightsDashboard.Models
{
    public class Keyword
    {
        public string Name { get; set; }
        public string Raised { get; set; }

        public Keyword(string name)
        {
            Name = name;
        }
    }
}
