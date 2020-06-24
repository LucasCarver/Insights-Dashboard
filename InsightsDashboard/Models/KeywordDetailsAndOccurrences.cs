using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsightsDashboard.Models
{
    public class KeywordDetailsAndOccurrences
    {
        public KeyValuePair<string, int> KeywordDetails { get; set; }
        public Dictionary<string, MainEntry> Occurrences { get; set; }
    }
}
