using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickDeals.Helper
{
    //*The only purpose of this class is to pass this(PaginationParams) object in repository method as a parameter, this avoid passing 3,4 parameter individually, we are passing PaginationParams as one object in method paramter, it looks cleaner and it is good practice also**/
    public class PaginationParams
    {
        public const int MaxPageSize = 50;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;

            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
