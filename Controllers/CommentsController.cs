using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickDeals.Core.IRepositories;
using QuickDeals.DTOs;
using QuickDeals.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace QuickDeals.Controllers
{   
    [Authorize]
    public class CommentsController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public CommentsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        
        [HttpPost("AddComment")]
        public async Task<ActionResult> AddComment(AddCommentDto addComment)
        {
            if (string.IsNullOrEmpty(addComment.Comment)) return BadRequest("Please write your comment!!");

            var username = User.GetUsername();
            if (string.IsNullOrEmpty(username)) return Unauthorized("You're unauthorized");

            var deal = await unitOfWork.DealRepository.GetDeal(addComment.DealId);
            if (deal == null) return BadRequest("Deal not found");

            var commentDto = await unitOfWork.CommentRepository.AddComment(addComment);
            commentDto.Username = username;

            if (commentDto == null) return BadRequest("Something went wrong while adding deal");

            if(await unitOfWork.SaveAsync()) return Ok(commentDto); ;

            return BadRequest("Unable to process your request");
        }
    }
}
