using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.DTOs
{
    public class RegisterDto
    {
        [Required] public string Username { get; set; }

        [StringLength(8, MinimumLength = 4)]
        [Required] public string Password { get; set; }
    }
}
