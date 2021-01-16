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
        public async Task<ActionResult> AddLike(int dealId)
        {
            var HasRatingObject = await context.Ratings
                .AnyAsync(x => x.DealId == dealId && x.UserId == User.GetUserId());

            if (HasRatingObject)
            {
                var IsLiked = await context.Ratings
                    .AnyAsync(x => x.DealId == dealId && x.UserId == User.GetUserId() && x.Like == true);
                if (IsLiked) return BadRequest("You have already liked this post");

                var ratingObject = await context.Ratings
                                        .SingleOrDefaultAsync(x => x.DealId == dealId && x.UserId == User.GetUserId());

                ratingObject.Like = true;

                if (await unitOfWork.SaveAsync()) return NoContent();
                return BadRequest("Can't like this deal");
            }

            if (!HasRatingObject)
            {
                var rating = new Rating
                {
                    Like = true,
                    DisLike = false,
                    DealId = dealId,
                    UserId = User.GetUserId()
                };

                var result = await context.Ratings.AddAsync(rating);

                if (await unitOfWork.SaveAsync()) return NoContent();
            }
            return BadRequest("Opps! somthing went wrong while Adding your like");
        }


        [HttpPost("AddDislike/{dealId}")]
        public async Task<ActionResult> AddDisLike(int dealId)
        {
            var HasRatingObject = await context.Ratings
                .AnyAsync(x => x.DealId == dealId && x.UserId == User.GetUserId());

            if (HasRatingObject)
            {
                var IsDisLiked = await context.Ratings
                    .AnyAsync(x => x.DealId == dealId && x.UserId == User.GetUserId() && x.DisLike == true);
                if (IsDisLiked) return BadRequest("You have already disliked this post");

                var ratingObject = await context.Ratings
                                        .SingleOrDefaultAsync(x => x.DealId == dealId && x.UserId == User.GetUserId());

                ratingObject.DisLike = true;

                if (await unitOfWork.SaveAsync()) return NoContent();
                return BadRequest("Can't dislike this deal");
            }

            if (!HasRatingObject)
            {
                var rating = new Rating
                {
                    Like = false,
                    DisLike = true,
                    DealId = dealId,
                    UserId = User.GetUserId()
                };

                var result = await context.Ratings.AddAsync(rating);

                if (await unitOfWork.SaveAsync()) return NoContent();
            }
            return BadRequest("Opps! somthing went wrong while Adding your dislike");
        }

    }
}
