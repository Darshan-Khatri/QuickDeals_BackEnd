using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickDeals.Core.Models;
using QuickDeals.DTOs;
using QuickDeals.Extensions;
using QuickDeals.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Controllers
{

    public class DealsController : BaseApiController
    {
        private readonly IMapper mapper;
        private readonly DataContext context;

        public DealsController(IMapper mapper, DataContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<ActionResult> CreateDeal(DealDto dealDto)
        {
            var userId = User.GetUserId();
            if (userId == 0) return Unauthorized();

            var deal = new Deal
            {
                Title = dealDto.Title,
                Content = dealDto.Content,
                Price = dealDto.Price,
                Url = dealDto.Url,
                Created = dealDto.Created,
                Category = dealDto.Category,
                AppUserId = userId
            };

            var result = await context.Deals.AddAsync(deal);
            return Ok(dealDto);
            
        }
    }
}
