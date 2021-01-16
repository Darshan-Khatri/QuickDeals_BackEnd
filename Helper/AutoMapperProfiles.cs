using AutoMapper;
using QuickDeals.Core.Models;
using QuickDeals.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterDto, AppUser>();

            CreateMap<UserUpdateDto, AppUser>();

            CreateMap<DealDto, Deal>();
            
            CreateMap<AppUser, MemberDto>();

            CreateMap<Deal, DealDto>();
        }
    }
}
