using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
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

        public async Task<IList<DealDto>> GetDealsWithRating()
        {
            //Below query is example of eager loading.
            return await context.Deals
                            .ProjectTo<DealDto>(mapper.ConfigurationProvider)
                            .OrderByDescending(x => x.Created)
                            .ToListAsync();
        }

        public async Task<IList<DealDto>> GetBestDeals()
        {
            /*This query is best example of lazy loading and why sometime we need to perform query with lazy loading rather than eager loading.
             1- In this query we want those deal which has more than 2 likes 
             2- What we are doing is, we have written sub-query in first part which gets all deals with any rating.
             3- Then we are writing another query which filters the query which has more than 2 likes,
             NOTE:- TILL THIS MOMENT WE ARE NOT EXECUTING ANY OF OUR SUB-QUERY TO DATABASE, basically what we are doing is we are chaining or developing our queries
             4- After our query gets completed we are executing it on database by writing ToListAsync() at the end of query.
             5- By doing this we are going to database only once so it improves our performance to great extent.

            ******So this is another used case of when to use LazyLoading query*************************
             => When you want to perform sub-query.
             */

            //We are fetching only deal which is not present in BestDeal table and also it has rating > 2
            //This is how you specify/write join conditions.
            var LazyLoadingQuery = context.Deals
                            .Where(x => x.BestDeals.All(y => y.DealId != x.DealId))
                            .ProjectTo<DealDto>(mapper.ConfigurationProvider);

            return await LazyLoadingQuery.Where(x => x.Likes > 2).ToListAsync();
        }

        public async Task<DealDto> GetDeal(int dealId)
        {
            return await context.Deals.Where(x => x.DealId == dealId)
                                    .ProjectTo<DealDto>(mapper.ConfigurationProvider)
                                    .FirstOrDefaultAsync();
        }

        ////This is how you specify/write join conditions.
        public async Task<IList<DealDto>> FrontPageDeals()
        {
            return await context.Deals
                            .Where(x => x.BestDeals.Any(y => y.DealId == x.DealId && y.IsApproved))
                            .ProjectTo<DealDto>(mapper.ConfigurationProvider)
                            .ToListAsync();
        }
    }
}
