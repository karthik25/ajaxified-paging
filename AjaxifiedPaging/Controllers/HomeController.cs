using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AjaxifiedPaging.Infrastructure.AjaxPaging;
using AjaxifiedPaging.Infrastructure.AjaxPaging.Entities;
using AjaxifiedPaging.Models;
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

        private static List<Camp> GetEntries()
        {
            var camps = Enumerable.Range(1, 65).Select(r => new Camp
                {
                    CampId = r,
                    CampName = "Camp " + r,
                    StartDate = DateTime.Now.AddMonths(-(r + 2)),
                    EndDate = DateTime.Now.AddMonths(r)
                });
            return camps.ToList();
        }
    }          
}
