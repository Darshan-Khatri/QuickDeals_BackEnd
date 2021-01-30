using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuickDeals.Core.IRepositories;
using QuickDeals.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Persistance.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DataContext context;

        public AdminRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<BestDeal> ApproveDeal(int dealId)
        {
            var deal = await context.Deals.FindAsync(dealId);

            var bestDeal = new BestDeal
            {
                DealId = deal.DealId,
                IsApproved = true,
            };

            return bestDeal;
        }

        public async Task InsertIntoBestDealTable(BestDeal bestDeal)
        {
            await context.BestDeals.AddAsync(bestDeal);
        }

        public async Task<BestDeal> RejectDeal(int dealId)
        {
            var deal = await context.Deals.FindAsync(dealId);

            var bestDeal = new BestDeal
            {
                DealId = deal.DealId,
                IsApproved = false,
            };

            return bestDeal;
        }
    }
}
