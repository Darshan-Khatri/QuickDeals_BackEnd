using QuickDeals.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Persistance.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly DataContext context;

        public RatingRepository(DataContext context)
        {
            this.context = context;
        }

        public void AddLike(int dealId)
        {
            //context.Ratings.
            
        }
    }
}
