using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuickDeals.Core.IRepositories;
using QuickDeals.Core.Models;
using QuickDeals.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Persistance.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public CommentRepository(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CommentDto> AddComment(AddCommentDto addComment)
        {
            var UserId = await context.Users.Where(x => x.UserName == addComment.Username).Select(s => s.Id).SingleOrDefaultAsync();

            var Comment = new Comment
            {
                AppUserId = UserId,
                DealId = addComment.DealId,
                comment = addComment.Comment
            };

            await context.Comments.AddAsync(Comment);

            return mapper.Map<CommentDto>(Comment);
        }
    }
}
