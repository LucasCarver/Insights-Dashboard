﻿using System;
using System.Collections.Generic;

namespace InsightsDashboard.Models
{
    public partial class SeamlessMaster
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public string Comment { get; set; }
        public int? Rating { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}