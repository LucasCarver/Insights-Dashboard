using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsightsDashboard.Models
{
    public class CustomStartupVM
    {
        public List<UserStartup> UserStartupList { get; set; }
        public List<string> UserNameList { get; set; }
    }
}
