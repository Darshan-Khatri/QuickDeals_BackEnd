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
        public Deal PostDeal(DealDto dealDto);

        public Task<IList<DealDto>> GetDeals();
    }
}
