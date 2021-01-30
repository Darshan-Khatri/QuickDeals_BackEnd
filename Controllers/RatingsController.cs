using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickDeals.Core.IRepositories;
using QuickDeals.Core.Models;
using QuickDeals.Extensions;
using QuickDeals.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Controllers
{
    [Authorize]
    public class RatingsController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly DataContext context;

        public RatingsController(IUnitOfWork unitOfWork, DataContext context)
        {
            this.unitOfWork = unitOfWork;
            this.context = context;
        }

        [HttpPost("AddLike/{dealId}")]
        public async Task<ActionResult<int>> AddLike(int dealId)
        {
            return  Ok(await LikeDislikeHelper(dealId, "like"));   
        }


        [HttpPost("AddDislike/{dealId}")]
        public async Task<ActionResult> AddDisLike(int dealId)
        {
            return Ok(await LikeDislikeHelper(dealId, "dislike"));
        }

        private async Task<ActionResult> LikeDislikeHelper(int dealId, string LikeDislike)
        {

            var HasRatingObject = await context.Ratings
                .AnyAsync(x => x.DealId == dealId && x.UserId == User.GetUserId());

            if (HasRatingObject)
            {
                if (LikeDislike == "dislike")
                {
                    var IsDisLiked = await context.Ratings
                        .AnyAsync(x => x.DealId == dealId && x.UserId == User.GetUserId() && x.DisLike == true);
                    if (IsDisLiked) return BadRequest("You have already disliked this post");

                    var ratingObject_ = await context.Ratings
                                            .SingleOrDefaultAsync(x => x.DealId == dealId && x.UserId == User.GetUserId());

                    ratingObject_.DisLike = true;

                    if (await unitOfWork.SaveAsync()) return Ok(await unitOfWork.RatingRepository.GetDisLikeCount(dealId));
                    return BadRequest("Can't dislike this deal");
                }
                var IsLiked = await context.Ratings
                    .AnyAsync(x => x.DealId == dealId && x.UserId == User.GetUserId() && x.Like == true);
                if (IsLiked) return BadRequest("You have already liked this post");

                var ratingObject = await context.Ratings
                                        .SingleOrDefaultAsync(x => x.DealId == dealId && x.UserId == User.GetUserId());

                ratingObject.Like = true;

                if (await unitOfWork.SaveAsync()) return Ok(await unitOfWork.RatingRepository.GetLikeCount(dealId));
                return BadRequest("Can't like this deal");
            }

            if (!HasRatingObject)
            {
                Rating rating;
                if (LikeDislike == "dislike")
                {
                    rating = new Rating
                    {
                        Like = false,
                        DisLike = true,
                        DealId = dealId,
                        UserId = User.GetUserId()
                    };
                }
                else
                {
                    rating = new Rating
                    {
                        Like = true,
                        DisLike = false,
                        DealId = dealId,
                        UserId = User.GetUserId()
                    };
                }
                var result = await context.Ratings.AddAsync(rating);

                if (await unitOfWork.SaveAsync())
                {
                    if (LikeDislike == "dislike") return Ok(await unitOfWork.RatingRepository.GetDisLikeCount(dealId));
                    return Ok(await unitOfWork.RatingRepository.GetLikeCount(dealId));
                }
            }
            return BadRequest("Opps! somthing went wrong while Adding your rating");
        }


        [HttpGet("LikeRating/{dealId}")]
        public async Task<ActionResult> GetLikeCount(int dealId)
        {
            return Ok(await unitOfWork.RatingRepository.GetLikeCount(dealId));
        }

        [HttpGet("DislikeRating/{dealId}")]
        public async Task<ActionResult> GetDisLikeCount(int dealId)
        {
            return Ok(await unitOfWork.RatingRepository.GetDisLikeCount(dealId));
        }

    }
}
