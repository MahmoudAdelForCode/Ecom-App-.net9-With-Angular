using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Sharing
{
    public class ProductParams
    {
        //string? sort ,int? CategoryId,int pageSize,int pageNumber
        public string? Sort { get; set; }
        public int? CategoryId { get; set; }
        public string? Search { get; set; }
        public int MaxPageSize { get; set; } = 6; // Default maximum page size
        private int _pageSize = 3; // Default page size
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value; // Ensure page size does not exceed max
        }
        public int pageNumber = 1; // Default page number
    }
}
