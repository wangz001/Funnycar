using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Providers.Entities;
using User = TuoFeng.Model.User;

namespace TFApi.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            TuoFeng.BLL.UserBll bll = new TuoFeng.BLL.UserBll();
            var flag = bll.Exists(5);

            var model = new User();
            model.UserName = "bwlang";

            model.ShowName="驼峰";
            model.PassWord="123";
            model.Sex=true;
            model.PhoneNum="18911186941";
            model.Email="bwlang@sian.cn";
            model.Age=20;
            model.ImgUrl="";
            model.IsEnable=true;
            model.CreateTime=DateTime.Now;
            model.UpdateTime=DateTime.Now;
            var result = bll.Add(model);
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}