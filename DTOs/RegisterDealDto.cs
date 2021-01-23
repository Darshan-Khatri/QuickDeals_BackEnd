using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.DTOs
{
    public class RegisterDealDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public DateTime Created { get; set; }

        public IFormFile Photo { get; set; }
    }
}
