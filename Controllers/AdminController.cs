using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> userManager;

        public AdminController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
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

        [HttpGet("GetRoles/{username}")]
        public async Task<IActionResult> GetRoles(string username)
        {
            if (username == null || username.Length == 0) return BadRequest("Something went wrong");
            return Ok(await unitOfWork.AdminRepository.GetUserRole(username));
        }

        [HttpPost("edit-user-role/{username}")]
        public async Task<ActionResult> EditUserRole(string username, [FromQuery] string roles)
        {
            var selectedRole = roles.Split(",").ToArray();

            var user = await userManager.FindByNameAsync(username);
            if (user == null) BadRequest("Something went wrong");

            var userRole = await unitOfWork.AdminRepository.GetUserRole(username);
            if (userRole == null || userRole.Count == 0) BadRequest("Something went wrong");

            var resultAdd = await userManager.AddToRolesAsync(user, selectedRole.Except(userRole));
            if (!resultAdd.Succeeded) BadRequest("Something went wrong while editing role");

            var resultRemove = await userManager.RemoveFromRolesAsync(user, userRole.Except(selectedRole));
            if (!resultRemove.Succeeded) BadRequest("Something went wrong while editing role");

            return Ok(await userManager.GetRolesAsync(user));
        }

    }
}
