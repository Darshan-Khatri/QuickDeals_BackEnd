using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Helper
{
    public class DealParams : PaginationParams
    {
        public string Category { get; set; }
        public string Price { get; set; } = "lowToHigh";
        public string Rating { get; set; }
    }
}
