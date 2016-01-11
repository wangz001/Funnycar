using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasyNet.Solr;
using EasyNet.Solr.Commons;
using EasyNet.Solr.Impl;
using Newtonsoft.Json;

namespace TuoFengWeb.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        public ActionResult Index()
        {
            return View();
        }

        public string DoSearch(int userid,string keyWord, int page)
        {
            var resultStr = string.Empty;
            const int count = 10;
            if (page<0)
            {
                page = 1;
            }
            if (string.IsNullOrEmpty(keyWord)) return string.Empty;
            var list = SolrNetUtil.Query(keyWord, page,count);
            if (list!=null&&list.Count>0)
            {
                resultStr = JsonConvert.SerializeObject(list);
            }
            return resultStr;
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}
