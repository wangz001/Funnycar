using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EasyNet.Solr;
using EasyNet.Solr.Commons;
using EasyNet.Solr.Impl;
using TuoFeng.Model;

namespace TuoFengWeb
{
    public class SolrNetUtil
    {
        private const string SolrUrl = "http://localhost:8080/solr/";
        private const string CoreName = "tuofeng";

        static OptimizeOptions optimizeOptions = new OptimizeOptions();
        static ISolrResponseParser<NamedList, ResponseHeader> binaryResponseHeaderParser = new BinaryResponseHeaderParser();
        static IUpdateParametersConvert<NamedList> updateParametersConvert = new BinaryUpdateParametersConvert();
        static ISolrUpdateConnection<NamedList, NamedList> solrUpdateConnection = new SolrUpdateConnection<NamedList, NamedList>() { ServerUrl = SolrUrl };
        static ISolrUpdateOperations<NamedList> updateOperations = new SolrUpdateOperations<NamedList, NamedList>(solrUpdateConnection, updateParametersConvert) { ResponseWriter = "javabin" };

        static ISolrQueryConnection<NamedList> connection = new SolrQueryConnection<NamedList>() { ServerUrl = SolrUrl };
        static ISolrQueryOperations<NamedList> operations = new SolrQueryOperations<NamedList>(connection) { ResponseWriter = "javabin" };

        static IObjectDeserializer<Example> exampleDeserializer = new ExampleDeserializer();
        static ISolrResponseParser<NamedList, QueryResults<Example>> binaryQueryResultsParser = new BinaryQueryResultsParser<Example>(exampleDeserializer);
     
        static SolrNetUtil()
        {

        }

        public static void Search()
        {
            
        }

        public static int NewTravelIndex(Travels travle)
        {
            var docs = new List<SolrInputDocument>();
            var doc = new SolrInputDocument();
            doc.Add("", new SolrInputField("id", "travels_"+travle.Id));  //索引中Id不能重复
            doc.Add("", new SolrInputField("userid", travle.UserId));
            doc.Add("", new SolrInputField("TravelName", travle.TravelName));
            doc.Add("", new SolrInputField("CreateTime", travle.CreateTime));
            doc.Add("", new SolrInputField("UpdateTime", travle.UpdateTime));
            doc.Add("", new SolrInputField("CoverImage", travle.CoverImage));
            docs.Add(doc);
            var result = updateOperations.Update(CoreName, "/update", new UpdateOptions() { OptimizeOptions = optimizeOptions, Docs = docs });
            var header = binaryResponseHeaderParser.Parse(result);
            return header.Status; //返回状态码。0表示成功
        }

        /// <summary>
        /// 创建或更新索引  travelpart
        /// </summary>
        /// <param name="parts"></param>
        /// <returns></returns>
        public static int NewTravelPartIndex(TravelParts parts)
        {
            var docs = new List<SolrInputDocument>();
            var doc = new SolrInputDocument();
            doc.Add("", new SolrInputField("id", "travelparts_" + parts.Id));
            doc.Add("", new SolrInputField("TravelId", parts.TravelId));
            doc.Add("", new SolrInputField("UserId", parts.UserId));
            doc.Add("", new SolrInputField("PartType", parts.PartType));
            doc.Add("", new SolrInputField("Description", parts.Description));
            doc.Add("", new SolrInputField("PartUrl", parts.PartUrl));
            doc.Add("", new SolrInputField("Longitude", parts.Longitude));
            doc.Add("", new SolrInputField("Latitude", parts.Latitude));
            doc.Add("", new SolrInputField("Height", parts.Height));
            doc.Add("", new SolrInputField("Area", parts.Area));
            doc.Add("", new SolrInputField("CreateTime", parts.CreateTime));
            docs.Add(doc);
            var result = updateOperations.Update(CoreName, "/update", new UpdateOptions() { OptimizeOptions = optimizeOptions, Docs = docs });
            var header = binaryResponseHeaderParser.Parse(result);
            return header.Status; //返回状态码。0表示成功
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        public static string Index()
        {
            var docs = new List<SolrInputDocument>();
            var doc = new SolrInputDocument();
            doc.Add("id", new SolrInputField("id", "SOLR1000"));
            doc.Add("name", new SolrInputField("name", "Solr, the Enterprise Search Server"));
            doc.Add("features", new SolrInputField("features", new String[] { "Advanced Full-Text Search Capabilities using Lucene", "Optimized for High Volume Web Traffic", "Standards Based Open Interfaces - XML and HTTP", "Comprehensive HTML Administration Interfaces", "Scalability - Efficient Replication to other Solr Search Servers", "Flexible and Adaptable with XML configuration and Schema", "Good unicode support: h&#xE9;llo (hello with an accent over the e)" }));
            doc.Add("price", new SolrInputField("price", 0.0f));
            doc.Add("popularity", new SolrInputField("popularity", 10));
            doc.Add("inStock", new SolrInputField("inStock", true));
            doc.Add("incubationdate_dt", new SolrInputField("incubationdate_dt", new DateTime(2006, 1, 17, 0, 0, 0, DateTimeKind.Utc)));

            docs.Add(doc);

            var result = updateOperations.Update("collection1", "/update", new UpdateOptions() { OptimizeOptions = optimizeOptions, Docs = docs });
            var header = binaryResponseHeaderParser.Parse(result);
            return string.Format("Update Status:{0} QTime:{1}", header.Status, header.QTime);
        }

        /// <summary>
        /// 查询
        /// </summary>
        static void Query()
        {
            var result = operations.Query("collection1", "/select", SolrQuery.All, null);
            var header = binaryResponseHeaderParser.Parse(result);

            var examples = binaryQueryResultsParser.Parse(result);
                    
            System.Console.WriteLine(string.Format("Query Status:{0} QTime:{1} Total:{2}", header.Status, header.QTime, examples.NumFound));
            System.Console.ReadLine();
        }

        /// <summary>
        /// 实体类
        /// </summary>
        class Example
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string[] Features { get; set; }

            public float Price { get; set; }

            public int Popularity { get; set; }

            public bool InStock { get; set; }

            public DateTime IncubationDate { get; set; }
        }

        /// <summary>
        /// 反序列化器
        /// </summary>
        class ExampleDeserializer : IObjectDeserializer<Example>
        {
            public IEnumerable<Example> Deserialize(SolrDocumentList result)
            {
                foreach (SolrDocument doc in result)
                {
                    yield return new Example()
                    {
                        Id = doc["id"].ToString(),
                        Name = doc["name"].ToString(),
                        Features = (string[])((ArrayList)doc["features"]).ToArray(typeof(string)),
                        Price = (float)doc["price"],
                        Popularity = (int)doc["popularity"],
                        InStock = (bool)doc["inStock"],
                        IncubationDate = (DateTime)doc["incubationdate_dt"]
                    };
                }
            }
        }
    }
}