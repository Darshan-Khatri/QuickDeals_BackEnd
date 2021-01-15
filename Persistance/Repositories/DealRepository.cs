using AutoMapper;
using QuickDeals.Core.IRepositories;
using QuickDeals.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Persistance.Repositories
{
    public class DealRepository : IDealRepository
    {
        private readonly IMapper mapper;
        private readonly DataContext context;

        public DealRepository(IMapper mapper, DataContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public Task<DealDto> CreateDeal(DealDto dealDto)
        {
            
            throw new NotImplementedException();
        }
    }
}
