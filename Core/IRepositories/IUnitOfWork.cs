using QuickDeals.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Core.IRepositories
{
    public interface IUnitOfWork
    {
        Task<bool> SaveAsync();

        IUserRepository UserRepository { get; }
        IDealRepository DealRepository { get; }
        IRatingRepository RatingRepository { get; }
        IAdminRepository AdminRepository { get; }
        ICommentRepository CommentRepository { get; }

        DataContext DBcontext { get; }
    }
}
