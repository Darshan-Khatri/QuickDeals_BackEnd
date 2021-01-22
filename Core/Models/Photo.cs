using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Core.Models
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }

        public Deal Deal { get; set; }
        public int DealId { get; set; }
    }
}
