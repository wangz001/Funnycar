using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasyNet.Solr;
using EasyNet.Solr.Commons;
using EasyNet.Solr.Impl;

namespace TuoFengWeb.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        public string Index(string keyWord)
        {
            var result = SolrNetUtil.Query("银杏",1);

            return result;
        }

    }
}
