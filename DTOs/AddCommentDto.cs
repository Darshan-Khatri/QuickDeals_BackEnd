using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.DTOs
{
    public class AddCommentDto
    {
        public string Comment { get; set; }

        public string Username { get; set; }

        public int DealId { get; set; }
    }
}
