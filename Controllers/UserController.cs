using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickDeals.Core.Models;
using QuickDeals.DTOs;
using QuickDeals.Extensions;
using QuickDeals.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuickDeals.Controllers
{
    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly DataContext context;
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;

        public UserController(DataContext context, UserManager<AppUser> userManager, IMapper mapper)
        {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        [HttpGet("GetUsers")]
        public async Task<ActionResult> GetAllUser()
        {
            var claims = User.FindFirst(ClaimTypes.Name)?.Value;
            if (!string.IsNullOrEmpty(claims))
            {
                return Ok(await context.Users.ToListAsync());
            }
            return Unauthorized();
        }

        [HttpGet("GetUser/{username}")]
        public async Task<ActionResult> GetUserDetail(string username)
        {
            if (username.ToLower() != User.GetUsername())
            {
                return Unauthorized();
            }
            var user = await userManager.FindByNameAsync(User.GetUsername());
            if (user != null) return Ok(user);
            return NotFound();
        }

        [HttpDelete("{username}")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            if (username.ToLower() != User.GetUsername())
            {
                return Unauthorized();
            }
            var user = await userManager.FindByNameAsync(User.GetUsername());

            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded) return Ok("Your account has been removed successfully");
                return BadRequest("Error occured while delete user");
            }

            return NotFound();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUserInfo(UserUpdateDto userUpdateDto)
        {
            var appUser = await userManager.FindByNameAsync(User.GetUsername());
            if (appUser == null) return Unauthorized();

            mapper.Map(userUpdateDto, appUser);

            context.Entry(appUser).State = EntityState.Modified;

            if (await context.SaveChangesAsync() > 0) return NoContent();
            return BadRequest("Unable to update your profile!");
        }
    }
}
