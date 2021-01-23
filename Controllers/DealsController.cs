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
        private readonly IPhotoService photoService;

        public DealsController(DataContext context, IUnitOfWork unitOfWork, IPhotoService photoService)
        {
            this.context = context;
            this.unitOfWork = unitOfWork;
            this.photoService = photoService;
        }

        [HttpPost("PostNewDeal")]
        public async Task<ActionResult> CreateDeal([FromForm]IFormFile file ,[FromForm]RegisterDealDto dealDto)
        {
            var username = await unitOfWork.UserRepository.GetUserByUsername(User.GetUsername());
            if (username == null) return Unauthorized();

            var result = await photoService.AddPhotoAsync(file);
            if (result.Error != null) { return BadRequest("Problem occured during photo upload to server."); }
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            var deal = unitOfWork.DealRepository.PostDeal(dealDto);
            deal.AppUserId = User.GetUserId();
            if (photo == null) return BadRequest("Photo object is null");
            deal.Photos.Add(photo);

            if (deal == null) return BadRequest("Deal object is null");

            await context.Deals.AddAsync(deal);

            if (await unitOfWork.SaveAsync()) return NoContent();
            return BadRequest("Error while posting new deal!!");
        }
        
        [HttpGet("GetDeals")]
        public async Task<ActionResult<IList<RegisterDealDto>>> GetDeals()
        {
            return Ok(await unitOfWork.DealRepository.GetDealsWithRating());
        }
        
        [HttpPost("add-photo/{dealID}")]
        private async Task<ActionResult> AddPhoto(IFormFile file, int dealId)
        {
            var result = await photoService.AddPhotoAsync(file);

            if (result.Error != null) { return null; }

            var photo = new Photo
            {
                Id = dealId,
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (photo == null) return null;
            return Ok(photo);
        }
    }
}
