using System;
using System.Collections.Generic;

namespace InsightsDashboard.Models
{
    public partial class StartupComments
    {
        public int Id { get; set; }
        public int? Rating { get; set; }
        public string Comment { get; set; }
        public string UserId { get; set; }
        public int StartupId { get; set; }

        public virtual UserStartup Startup { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
