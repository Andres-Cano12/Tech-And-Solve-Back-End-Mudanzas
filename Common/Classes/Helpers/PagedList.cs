
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Common.Classes.Extensions;
using App.Common.Classes.Base.Repositories;
using App.Common.Classes.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using App.Common.Classes.Helpers.DynamicFilters;
using System.Linq.Expressions;
using App.Common.Classes.DTO.Request;

namespace App.Common.Classes.Helpers
{
    public class PagedList<T>
    {
        public int TotalItems { get; set; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public List<T> List { get; }
        public int TotalPages =>
              (int)Math.Ceiling(TotalItems / (double)PageSize);

        public PagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            TotalItems = source.Count();
            PageNumber = pageNumber;
            PageSize = pageSize;
            List = source
                        .Skip(pageSize * (pageNumber - 1))
                        .Take(pageSize)
                        .ToList();
        }

        public PagedList(IQueryable<T> source, PagingParams pagingParams)
        {
            TotalItems = source.Count();
            PageNumber = pagingParams.PageNumber;
            PageSize = pagingParams.PageSize;
            List = source
                .ApplyOrderBy(pagingParams.SortProperty, pagingParams.SortType)
                .Skip(pagingParams.PageSize * (pagingParams.PageNumber - 1))
                .Take(pagingParams.PageSize)
                .ToList();
        }

        public PagedList(IQueryable<T> source, Expression<Func<T, bool>> deleg, PagingParams pagingParams)
        {
            if (deleg != null )
            {
                source = source.Where(   deleg);
            }

            if (pagingParams.SortProperty != null)
            {
                source = source.ApplyOrderBy(pagingParams.SortProperty, pagingParams.SortType);
            }

            TotalItems = source.Count();
            PageNumber = pagingParams.PageNumber;
            if (pagingParams.PageSize == 0)
            {
                pagingParams.PageSize = TotalItems;
                PageSize = pagingParams.PageSize;
                List = source
                    .Skip(pagingParams.PageSize * (pagingParams.PageNumber - 1))
                    .Take(pagingParams.PageSize)
                    .ToList();
            }
            else
            {
                PageSize = pagingParams.PageSize;
                List = source
                    .Skip(pagingParams.PageSize * (pagingParams.PageNumber - 1))
                    .Take(pagingParams.PageSize)
                    .ToList();
            }            
        }

        public PagedList(List<T> source, int pageNumber, int pageSize, int totalItems)
        {
            TotalItems = totalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;
            List = source.ToList();
        }
    }
}
