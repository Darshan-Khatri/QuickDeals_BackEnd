using AutoMapper;
using Microsoft.AspNetCore.Identity;
using QuickDeals.Core.IRepositories;
using QuickDeals.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Persistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;

        public UnitOfWork(DataContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public IUserRepository UserRepository => new UserRepository(context, mapper, userManager);

        public IDealRepository DealRepository => new DealRepository(mapper, context);

        public IRatingRepository RatingRepository => new RatingRepository(context);

        public IAdminRepository AdminRepository => new AdminRepository(context, userManager);

        public async Task<bool> SaveAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
