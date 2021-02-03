using AutoMapper;
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
        private readonly IUnitOfWork unitOfWork;

        public UserController(DataContext context, UserManager<AppUser> userManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("GetUsers")]
        public async Task<ActionResult> GetAllUser()
        {
            var userNameInToken = User.GetUsername();
            if (!string.IsNullOrEmpty(userNameInToken))
            {
                return Ok(await unitOfWork.UserRepository.GetAllUser());
                //return Ok(await context.Users.Include(d => d.Deals).ToListAsync());
            }
            return Unauthorized();
        }

        [HttpGet("GetUser/{username}")]
        public async Task<ActionResult> GetUserDetail(string username)
        {
            var user = await unitOfWork.UserRepository.GetUserByUsername(username);
            //var user = await unitOfWork.UserRepository.GetUserByUsernameWithDeals(username);
            if (user != null) return Ok(user);
            return NotFound();
        }

        [HttpGet("GetUserByUserId/{userId}")]
        public async Task<ActionResult> GetUserDetail(int userId)
        {
            if (userId != User.GetUserId())
            {
                return Unauthorized();
            }

            var user = await unitOfWork.UserRepository.GetUserByUserId(User.GetUserId());
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
