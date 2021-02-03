using QuickDeals.Core.Models;
using QuickDeals.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Core.IRepositories
{
    public interface IUserRepository
    {
        Task<MemberDto> GetUserByUsername(string username);
        Task<AppUser> GetUserByUserId(int userId);
        Task<IEnumerable<MemberDto>> GetAllUser();
        void UpdateUser(AppUser user);
        Task<AppUser> GetUserByUsernameWithDeals(string username);
    }
}
