﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickDeals.Core.IRepositories;
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
    [Authorize]
    public class DealsController : BaseApiController
    {

        private readonly DataContext context;
        private readonly IUnitOfWork unitOfWork;

        public DealsController(DataContext context, IUnitOfWork unitOfWork)
        {
            this.context = context;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("PostNewDeal")]
        public async Task<ActionResult> CreateDeal(RegisterDealDto dealDto)
        {
            var username = await unitOfWork.UserRepository.GetUserByUsername(User.GetUsername());
            if (username == null) return Unauthorized();

            var deal = unitOfWork.DealRepository.PostDeal(dealDto);
            deal.AppUserId = User.GetUserId();
            if (deal == null) return BadRequest("Deal object is null");

            await context.Deals.AddAsync(deal);

            if (await unitOfWork.SaveAsync()) return NoContent();
            return BadRequest("Error while posting new deal!!");
        }

        //[HttpGet("GetDeals")]
        //public async Task<ActionResult<IList<RegisterDealDto>>> GetDeals()
        //{
        //    return Ok(await unitOfWork.DealRepository.GetDeals());
        //}
        
        [HttpGet("GetDeals")]
        public async Task<ActionResult<IList<RegisterDealDto>>> GetDeals()
        {
            return Ok(await unitOfWork.DealRepository.GetDealsWithRating());
        }

        [HttpGet("GetDealsDto")]
        public async Task<IActionResult> getDeals()
        {
            return Ok(await unitOfWork.DealRepository.GetDealsWithRating());
        }

        [HttpGet("BestDeals")]
        public async Task<IActionResult> GetQualifiedDeals()
        {
            return Ok(await unitOfWork.DealRepository.GetBestDeals());
        }
    }
}
