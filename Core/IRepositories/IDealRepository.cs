using QuickDeals.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Core.IRepositories
{
    public interface IDealRepository
    {
        Task<DealDto> CreateDeal(DealDto dealDto);   
    }
}
