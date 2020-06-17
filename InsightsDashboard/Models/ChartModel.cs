using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace InsightsDashboard.Models
{
    public class ChartModel
    {
        public string companyName { get; set; }
        public int Raised { get; set; }
    public ChartModel(string companyName, int Raised)

    {
        this.companyName = companyName;
        this.Raised = Raised;
    }
    }
}
