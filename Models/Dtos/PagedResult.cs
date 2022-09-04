using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int ItemsFrom { get; set; }
        public int ItemsTo { get; set; }
        public int TotalItemsCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }

        public PagedResult(List<T> items, int totalCount, int pageSize, int pageNumber)
        {
            Items = items;
            PageNumber = pageNumber;
            TotalItemsCount = totalCount;
            PageSize = pageSize;
            ItemsFrom = pageSize * (pageNumber - 1) + 1;
            ItemsTo = ItemsFrom + pageSize - 1;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

    }
}
