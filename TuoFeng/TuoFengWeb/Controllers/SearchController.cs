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

        public string Index()
        {
            var result=SolrNetUtil.Index();

            return result;
        }

    }
}
