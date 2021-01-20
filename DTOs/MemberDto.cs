using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.DTOs
{
    public class MemberDto
    {
        public string Title { get; set; }
        public string Username { get; set; }
        public ICollection<RegisterDealDto> Deals { get; set; }
    }
}
