using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Core.IRepositories
{
    public interface IRatingRepository
    {
        public void AddLike(int dealId);
        public Task<int> GetLikeCount(int dealId);
        public Task<int> GetDisLikeCount(int dealId);
    }
}
