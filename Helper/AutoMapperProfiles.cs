﻿using AutoMapper;
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
                    src.DealRating.Count(c => c.DisLike)))
                .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => 
                    src.AppUser.UserName));


            CreateMap<Photo, PhotoDto>();
            
        
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.CommentText, opt => opt.MapFrom(src => src.comment))
                .ForMember(dest => dest.DealId, opt => opt.MapFrom(src => src.DealId))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.AppUser.UserName))
                .ForMember(dest => dest.CommentDate, opt => opt.MapFrom(src => src.date));
        }
    }
}
