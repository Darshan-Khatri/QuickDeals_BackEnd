using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QuickDeals.Core.IRepositories;
using QuickDeals.Core.Models;
using QuickDeals.DTOs;
using QuickDeals.Helper;
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

        public async Task<PagedList<DealDto>> GetDealsPagination(PaginationParams paginationParams)
        {
            var queryable = context.Deals
                            .ProjectTo<DealDto>(mapper.ConfigurationProvider)
                            .OrderByDescending(x => x.Created);

            return await PagedList<DealDto>.CreateAsync
                (queryable, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<PagedList<DealDto>> GetDealsPaginationWithFilter(DealParams dealParams)
        {
            var queryable = context.Deals
                            .ProjectTo<DealDto>(mapper.ConfigurationProvider).AsNoTracking();

            var dealCategoryArray = new List<string>
            {
                "Electronics", "HouseHold", "Education","Other"
            };
            //Filter by category
            if (!string.IsNullOrEmpty(dealParams.Category) && dealCategoryArray.Contains(dealParams.Category))
                queryable = queryable.Where(x => x.Category == dealParams.Category);

            //filter by rating
            queryable = dealParams.Rating switch
            {
                "1" => queryable.Where(x => (x.Likes - x.DisLikes) >= 1),
                "2" => queryable.Where(x => (x.Likes - x.DisLikes) >= 2),
                "3" => queryable.Where(x => (x.Likes - x.DisLikes) >= 3),
                _ => queryable
            };

            //filter by price
            queryable = dealParams.Price switch
            {
                "less than 100" => queryable.Where(x => x.Price <= 100),
                "101-300" => queryable.Where(x => x.Price >= 101 && x.Price <= 300),
                "301-500" => queryable.Where(x => x.Price >= 301 && x.Price <= 500),
                ">500" => queryable.Where(x => x.Price > 500),
                _ => queryable
            };

            //sort by date
            if(dealParams.Date == "new")  queryable = queryable.OrderByDescending(x => x.Created);
            if(string.IsNullOrEmpty(dealParams.Date) || dealParams.Date != "new") queryable = queryable.OrderBy(x => x.Created);


            return await PagedList<DealDto>.CreateAsync
                (queryable, dealParams.PageNumber, dealParams.PageSize);
        }

        public async Task<IList<DealDto>> GetBestDeals()
        {
            /*This query is best example of lazy loading and why sometime we need to perform query with lazy loading rather than eager loading.
             1- In this query we want those deal which has more than 2 likes 
             2- What we are doing is, we have written sub-query in first part which gets all deals with any rating that are not part of best deals table.
             3- Then we are writing another query which filters the query which has more than 2 likes,
             NOTE:- TILL THIS MOMENT WE ARE NOT EXECUTING ANY OF OUR SUB-QUERY TO DATABASE, basically what we are doing is we are chaining or developing our queries
             4- After query gets completed we are executing it on database by writing ToListAsync() at the end of query.
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
