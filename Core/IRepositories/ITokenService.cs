using QuickDeals.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Core.IRepositories
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
