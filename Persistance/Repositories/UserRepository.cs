using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickDeals.Core.IRepositories;
using QuickDeals.Core.Models;
using QuickDeals.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Persistance.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;

        public UserRepository(DataContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<MemberDto>> GetAllUser()
        {
            var users = context.Users.Select(u => new MemberDto
            {
                Title = u.Title,
                Username = u.UserName,
                Deals = mapper.Map<ICollection<RegisterDealDto>>(u.Deals)
            });

            return await users.ToListAsync();
        }

        public async Task<AppUser> GetUserByUserId(int userId)
        {
            return await context.Users.FindAsync(userId);
        }

        public async Task<AppUser> GetUserByUsername(string username)
        {
            //return await context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            return await userManager.FindByNameAsync(username);
        }

        public async Task<AppUser> GetUserByUsernameWithDeals(string username)
        {
            var user = await context.Users
                                .Select(a => new AppUser { UserName = a.UserName, Deals = a.Deals })
                                .SingleOrDefaultAsync(u => u.UserName == username);
            return user;
        }

        public void UpdateUser(AppUser user)
        {
            context.Entry(user).State = EntityState.Modified;
        }
    }
}
