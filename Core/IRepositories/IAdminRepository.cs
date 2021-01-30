using QuickDeals.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Core.IRepositories
{
    public interface IAdminRepository
    {
        public Task<BestDeal> ApproveDeal(int dealId);

        public Task InsertIntoBestDealTable(BestDeal bestDeal);
    }
}
