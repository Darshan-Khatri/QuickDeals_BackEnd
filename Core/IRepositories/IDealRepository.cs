using QuickDeals.Core.Models;
using QuickDeals.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Core.IRepositories
{
    public interface IDealRepository
    {
        public Deal PostDeal(RegisterDealDto dealDto);

        public Task<IList<RegisterDealDto>> GetDeals();

        public Task<IList<DealDto>> GetDealsWithRating();
    }
}
