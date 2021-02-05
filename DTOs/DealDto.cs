using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.DTOs
{
    public class DealDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public string Category { get; set; }
        public string Creator { get; set; }
        public double Price { get; set; }
        public int Likes { get; set; }
        public int DisLikes { get; set; }
        public DateTime Created { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }
        public ICollection<CommentDto> Comments { get; set; }
    }
}
