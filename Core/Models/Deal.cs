using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Core.Models
{
    public class Deal
    {
        public int DealId { get; set; }

        [Required] public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string Url { get; set; }
        public string Category { get; set; }
        [Required] public double Price { get; set; }

        public ICollection<Photo> Photos { get; set; }

        /*It contains the foreign key of AppUser because one user can create many deals*/
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        //********************************************************************************

        /*One Deal can get many likes and dislikes****************************************/
        public ICollection<Rating> DealRating { get; set; }
        //********************************************************************************
    }
}
