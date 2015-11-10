using System;
using System.Web.Http;
using System.Web.Mvc;
using TuoFeng.BLL;
using TuoFeng.Model;

namespace TFApi.Controllers
{
    public class UserController : ApiController
    {
        private readonly UserBll _userBll=new UserBll();

        // GET api/user/5
        public string Get()
        {
            return "value";
        }
        // GET api/user/5
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public string Put(string userName,string passWord)
        {

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord))
            {
                return "用户名或密码不能为空";
            }
            if (_userBll.Exists(userName))
            {
                return "用户名已被注册,请尝试别的名字";
            }
            var user = new User
            {
                UserName = userName,
                PassWord = passWord,
                IsEnable = true,
                CreateTime =DateTime.Now,
                UpdateTime = DateTime.Now
            };
            var userid=_userBll.Add(user);
            return (userid > 0).ToString();
        }

        // PUT api/user/5
        public void Update(int id, [FromBody]string value)
        {


        }

        // DELETE api/user/5
        public void Delete(int id)
        {
        }
    }
}
