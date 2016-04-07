using System.Linq;
using System.Collections.Generic;
using AjaxifiedPaging.Infrastructure.AjaxPaging.Entities;

namespace AjaxifiedPaging.Infrastructure.AjaxPaging
{
    public static class PagingHelpers
    {
        public static PageContent AsPagedContent<T>(this List<T> entries, PageContent current)
            where T : class
        {
            var paging = current.Paging == null ? 
                new Paging
                    {
                        CurrentPage = 1, 
                        ItemsPerPage = 10, 
                        TotalItems = entries.Count
                    } 
                : new Paging
                      {
                        CurrentPage = current.Paging.CurrentPage,
                        ItemsPerPage = 10,
                        TotalItems = entries.Count
                      };
            var pageContent = new PageContent
            {
                Container = current.Container,
                GetUrl = current.GetUrl,
                Paging = paging,
                Entries = entries.Skip((paging.CurrentPage - 1) * paging.ItemsPerPage)
                                 .Take(paging.ItemsPerPage)
                                 .Select(c => (object)c)
                                 .ToArray()
            };
            return pageContent;
        }
    }
}
