using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Core.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public DateTime date { get; set; } = DateTime.UtcNow;
        public string comment { get; set; }

        //This is required because we have to store the Id of user who has commented on deal. 
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }

        public Deal Deal { get; set; }
        public int DealId { get; set; }

    }
}
