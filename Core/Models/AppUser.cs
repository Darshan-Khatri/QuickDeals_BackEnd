using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Core.Models
{
    public class AppUser : IdentityUser<int>
    {
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string Title { get; set; } = "New User";

        public ICollection<AppUserRole> UserRoles { get; set; }
        public ICollection<Deal> Deals { get; set; }


        public ICollection<Rating> DealRating { get; set; }
    }
}
