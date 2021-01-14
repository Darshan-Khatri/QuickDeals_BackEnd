using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickDeals.Core.Models;
using QuickDeals.DTOs;
using QuickDeals.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IMapper mapper;
        private readonly DataContext context;

        public AccountController(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, IMapper mapper, DataContext context) 
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var userNameExists = await userManager.FindByNameAsync(registerDto.Username);
            if (userNameExists != null) return BadRequest("Username already exists, Use another username!");
                
            var user = mapper.Map<AppUser>(registerDto);

            user.UserName = registerDto.Username.ToLower();

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) BadRequest("Unable to create your account");

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> Login([FromBody]LoginDto loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Username)) return NotFound();

            var user = await userManager.FindByNameAsync(loginDto.Username);

            if (user == null) return BadRequest("Username is invalid");

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return BadRequest("Invalid password");

            var LoginObject = new LoginDto
            {
                Username = user.UserName,
                Password = user.PasswordHash
            };

            return Ok(LoginObject);
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(await context.Users.ToListAsync());
        }
    }
}
