using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsightsDashboard.Models
{
    public class UserStartupComments
    {
        public UserStartup Startups { get; set; }
        public List<StartupComments> Comments { get; set; }
    }
}
