namespace AjaxifiedPaging.Infrastructure.AjaxPaging.Entities
{
    public class PageContent
    {
        public string Container { get; set; }
        public string GetUrl { get; set; }
        public Paging Paging { get; set; }
        public Search Search { get; set; }
        public object[] Entries { get; set; }
    }
}