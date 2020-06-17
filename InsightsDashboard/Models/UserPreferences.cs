using System;
using System.Collections.Generic;

namespace InsightsDashboard.Models
{
    public partial class UserPreferences
    {
        public int PrefId { get; set; }
        public string UserId { get; set; }
        public string StartUpDescription { get; set; }
        public string FollowedAlignment { get; set; }
        public string FollowedCountry { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
