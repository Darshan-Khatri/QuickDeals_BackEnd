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
        public Deal PostDeal(RegisterDealDto dealDto)
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

        public async Task<IList<RegisterDealDto>> GetDeals()
        {
            var deals = await context.Deals.OrderByDescending(x => x.Created).ToListAsync();
            return mapper.Map<IList<RegisterDealDto>>(deals);
        }

        public async Task<IList<DealDto>> GetDealsWithRating()
        {
            return (await context.Deals
                                .Select(x => new DealDto
                                {
                                    Id = x.DealId,
                                    Title = x.Title,
                                    Price = x.Price,
                                    Creator = x.AppUser.UserName,
                                    Likes = x.DealRating.Count(c => c.Like == true),
                                    DisLikes = x.DealRating.Count(c => c.DisLike == true),
                                    Content = x.Content,
                                    Created = x.Created,
                                    Url = x.Url,
                                    Category = x.Category
                                }).ToListAsync());
        }
    }
}
