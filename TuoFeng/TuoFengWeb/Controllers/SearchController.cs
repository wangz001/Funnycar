﻿using System;
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

        public string Index(string keyWord,int page)
        {
            var result = SolrNetUtil.Query(keyWord,page);
            return result;
        }

    }
}
