using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.BuildingBlocks.Application.Pagination
{
    public class PagedResult<T>
    {
        public IReadOnlyCollection<T> Items { get; }
        public int TotalCount { get; }
        public int Page { get; }
        public int PageSize { get; }

        public PagedResult(
            IReadOnlyCollection<T> items,
            int totalCount,
            int page,
            int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
        }
    }
}
