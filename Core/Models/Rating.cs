using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Core.Models
{
    public class Rating
    {
        public bool Like { get; set; }
        public bool DisLike { get; set; }

        public Deal Deal { get; set; }
        public int DealId { get; set; }

        public AppUser User { get; set; }
        public int UserId { get; set; }
    }
}
