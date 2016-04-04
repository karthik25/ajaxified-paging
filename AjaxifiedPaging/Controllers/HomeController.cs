using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace AjaxifiedPaging.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetPagedData(string pageContentRequest)
        {
            var request = JsonConvert.DeserializeObject<PageContent>(pageContentRequest);
            var entries = GetEntries();
            var pageContent = entries.AsPagedContent(request);
            return Json(pageContent, JsonRequestBehavior.AllowGet);
        }

        private List<Camp> GetEntries()
        {
            var camps = Enumerable.Range(1, 25).Select(r => new Camp
                {
                    CampId = r,
                    CampName = "Camp " + r,
                    StartDate = DateTime.Now.AddMonths(-(r + 2)),
                    EndDate = DateTime.Now.AddMonths(r)
                });
            return camps.ToList();
        }
    }

    public class PageContent
    {
        public string Container { get; set; }
        public string GetUrl { get; set; }
        public Paging Paging { get; set; }
        public Search Search { get; set; }
        public object[] Entries { get; set; }
    }

    public class Paging
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }
    }

    public class Search
    {
        public string Name { get; set; }
    }

    public class Camp
    {
        public int CampId { get; set; }
        public string CampName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public static class PagingHelpers
    {
        public static PageContent AsPagedContent<T>(this List<T> entries, PageContent current)
            where T : class
        {
            var paging = current.Paging == null ? new Paging {CurrentPage = 1, ItemsPerPage = 10, TotalItems = entries.Count} : new Paging
                {
                    CurrentPage = current.Paging.CurrentPage, ItemsPerPage = 10, TotalItems = entries.Count
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
