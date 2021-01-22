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

            CreateMap<RegisterDealDto, Deal>();

            CreateMap<AppUser, MemberDto>();

            CreateMap<Deal, RegisterDealDto>();

            CreateMap<Deal, DealDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DealId))
                .ForMember(dest => dest.Likes, opt => opt.MapFrom(src =>
                    src.DealRating.Count(c => c.Like)))
                .ForMember(dest => dest.DisLikes, opt => opt.MapFrom(src =>
                    src.DealRating.Count(c => c.DisLike)));


        }
    }
}
