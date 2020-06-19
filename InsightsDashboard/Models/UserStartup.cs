using System;
using System.Collections.Generic;

namespace InsightsDashboard.Models
{
    public partial class UserStartup
    {
        public UserStartup()
        {
            StartupComments = new HashSet<StartupComments>();
        }

        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyWebsite { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string Scout { get; set; }
        public string Source { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string Country { get; set; }
        public string TwoLineSummary { get; set; }
        public string Alignment { get; set; }
        public string Theme { get; set; }
        public string Technology { get; set; }
        public string Landscape { get; set; }
        public int? Uniqueness { get; set; }
        public int? Team { get; set; }
        public string Raised { get; set; }
        public string UserId { get; set; }
        public string Identifier { get; set; }

        public virtual AspNetUsers User { get; set; }
        public virtual ICollection<StartupComments> StartupComments { get; set; }
    }
}
