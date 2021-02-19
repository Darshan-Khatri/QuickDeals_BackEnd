using QuickDeals.Core.Models;
using QuickDeals.DTOs;
using QuickDeals.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Core.IRepositories
{
    public interface IDealRepository
    {
        public Deal PostDeal(RegisterDealDto dealDto);

        public Task<IList<DealDto>> GetDealsWithRating();
        public Task<PagedList<DealDto>> GetDealsPagination(PaginationParams paginationParams);
        public Task<PagedList<DealDto>> GetDealsPaginationWithFilter(DealParams dealParams);

        public Task<IList<DealDto>> GetBestDeals();

        public Task<DealDto> GetDeal(int dealId);

        public Task<IList<DealDto>> FrontPageDeals();
    }
}
