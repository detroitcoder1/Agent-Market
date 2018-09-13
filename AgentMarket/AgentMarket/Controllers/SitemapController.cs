using AgentMarket.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace AgentMarket.Controllers
{
    public class SitemapController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        //
        // GET: /Sitemap/
        public ActionResult Index()
        {
            List<ISitemapItem> items = new List<ISitemapItem>();
            string host = Request.Url.GetLeftPart(UriPartial.Authority);
            var urls = context.StaticMenuItems.Select(x => x.Id).AsEnumerable().Select(x => new SitemapItem(host + "/staticmenu/details/" + x));
            items.AddRange(urls);
            urls = context.DynamicMenuItems.Select(x => x.Id).AsEnumerable().Select(x => new SitemapItem(host + "/items/index/" + x));
            items.AddRange(urls);
            urls = context.Items.Select(x => x.Id).AsEnumerable().Select(x => new SitemapItem(host + "/items/details/" + x));
            items.AddRange(urls);
            return new XmlSitemapResult(items);
        }
	}

    public enum ChangeFrequency
    {
        Always,
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Yearly,
        Never
    }

    public interface ISitemapItem
    {
        string Url { get; }
        DateTime? LastModified { get; }
        ChangeFrequency? ChangeFrequency { get; }
        float? Priority { get; }
    }

    public class SitemapItem : ISitemapItem
    {
        public SitemapItem(string url)
        {
            Url = url;
        }

        public string Url { get; set; }

        public DateTime? LastModified { get; set; }

        public ChangeFrequency? ChangeFrequency { get; set; }

        public float? Priority { get; set; }
    }

    public class XmlSitemapResult : ActionResult
    {
        private IEnumerable<ISitemapItem> _items;

        public XmlSitemapResult(IEnumerable<ISitemapItem> items)
        {
            _items = items;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            string encoding = context.HttpContext.Response.ContentEncoding.WebName;
            XDocument sitemap = new XDocument(new XDeclaration("1.0", encoding, "yes"), new XElement("urlset", from item in _items select CreateItemElement(item)));

            context.HttpContext.Response.ContentType = "application/rss+xml";
            context.HttpContext.Response.Flush();
            context.HttpContext.Response.Write(sitemap.Declaration + sitemap.ToString());
        }

        private XElement CreateItemElement(ISitemapItem item)
        {
            XElement itemElement = new XElement("url", new XElement("loc", item.Url.ToLower()));

            if (item.LastModified.HasValue)
                itemElement.Add(new XElement("lastmod", item.LastModified.Value.ToString("yyyy-MM-dd")));

            if (item.ChangeFrequency.HasValue)
                itemElement.Add(new XElement("changefreq", item.ChangeFrequency.Value.ToString().ToLower()));

            if (item.Priority.HasValue)
                itemElement.Add(new XElement("priority", item.Priority.Value.ToString(CultureInfo.InvariantCulture)));

            return itemElement;
        }
    }
}