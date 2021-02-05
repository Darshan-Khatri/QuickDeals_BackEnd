using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.DTOs
{
    public class CommentDto
    {
        public string Username { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentDate { get; set; }
        public int DealId { get; set; }
    }
}
