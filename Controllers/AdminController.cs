using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickDeals.Core.IRepositories;
using QuickDeals.Core.Models;
using QuickDeals.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public AdminController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("BestDeals")]
        public async Task<IActionResult> GetQualifiedDeals()
        {
            return Ok(await unitOfWork.DealRepository.GetBestDeals());
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("Approve/{dealId}")]
        public async Task<ActionResult> ApproveDeal(int dealId)
        {
            
            var bestDeal = await unitOfWork.AdminRepository.ApproveDeal(dealId);
            return await ApplyChangesToDB(bestDeal);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("Reject/{dealId}")]
        public async Task<ActionResult> RejectDeal(int dealId)
        {
            var bestDeal = await unitOfWork.AdminRepository.RejectDeal(dealId);
            return await ApplyChangesToDB(bestDeal);
        }

        private async Task<ActionResult> ApplyChangesToDB(BestDeal bestDeal)
        {
            if (bestDeal == null) return BadRequest("bestDeal object is null");
            bestDeal.AppUserId = User.GetUserId();


            await unitOfWork.AdminRepository.InsertIntoBestDealTable(bestDeal);
            if (await unitOfWork.SaveAsync()) return NoContent();
            return BadRequest("Something went wrong while adding bestDeal");
        }
    }
}
