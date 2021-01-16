using AutoMapper;
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
    public class DealRepository : IDealRepository
    {
        private readonly IMapper mapper;
        private readonly DataContext context;

        public DealRepository(IMapper mapper, DataContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }
        public Deal PostDeal(DealDto dealDto)
        {
            var deal = new Deal
            {
                Title = dealDto.Title,
                Content = dealDto.Content,
                Price = dealDto.Price,
                Url = dealDto.Url,
                Category = dealDto.Category
            };

            return deal;
        }

        public async Task<IList<DealDto>> GetDeals()
        {
            var deals = await context.Deals.OrderByDescending(x => x.Created).ToListAsync();

            return mapper.Map<IList<DealDto>>(deals);
        }
    }
}
