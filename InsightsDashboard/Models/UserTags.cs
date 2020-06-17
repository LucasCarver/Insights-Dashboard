using System;
using System.Collections.Generic;

namespace InsightsDashboard.Models
{
    public partial class UserTags
    {
        public string UserId { get; set; }
        public int? UserTagId { get; set; }

        public virtual AspNetUsers User { get; set; }
        public virtual Tags UserTag { get; set; }
    }
}
