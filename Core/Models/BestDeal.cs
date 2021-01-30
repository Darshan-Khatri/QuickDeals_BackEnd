using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Core.Models
{
    public class BestDeal
    {
        public int BestDealId { get; set; }

        public DateTime ApprovalDate { get; set; } = DateTime.UtcNow;
        public bool IsApproved { get; set; }

        //*************Deal table foreign key relation*************************
        public Deal Deal { get; set; }
        public int DealId { get; set; }
        //*********************************************************************

        //***************AppUser table foreign table relation***********************
        public AppUser AdminUser { get; set; }
        public int AppUserId { get; set; }
        //**************************************************************************

        //The above relationship would give answers of questions like 
        //Who(admin) has approved this deal?
        //This deal is approved by which admin?
    }
}
