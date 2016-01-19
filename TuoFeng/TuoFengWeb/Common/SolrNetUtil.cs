
using System.Collections.Generic;
using System.Data;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.Impl;
using TuoFengWeb.Models;

namespace TuoFengWeb.Common
{
    public class SolrNetUtil
    {

        public static List<SearchVm2> Query(string keyWord, int rowNum, int count)
        {
            //定义solr
            //ISolrOperations<SearchVm2> solr= ServiceLocator.Current.GetInstance<ISolrOperations<SearchVm2>>();

            //建立排序，条件.
            QueryOptions options = new QueryOptions();
            options.Rows = count;//数据条数
            options.Start = 1;//开始项


            //创建查询条件
            var qTB = new SolrQueryByField("text", keyWord);

            //创建条件集合
            List<ISolrQuery> query = new List<ISolrQuery>();
            //添加条件
            query.Add(qTB);

            //按照时间倒排序.
            options.AddOrder(new SortOrder("CreateTime", Order.DESC));

            //条件集合之间的关系
            var qTBO = new SolrMultipleCriteriaQuery(query, "OR");

            //执行查询,有5个重载
            //SolrQueryResults<SearchVm2> results = solr.Query(qTBO,options);


            return null;//results;
        }

        public class SearchVm2
        {
            //user信息
            public int UserId { get; set; }
        }
    }
}