using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TuoFeng.BLL;
using TuoFeng.Model;

namespace ConsoleTest
{
    public class testUser
    {
        public static void regist()
        {
            var dic = new Dictionary<string,string>
            {
                {"UserName", "驼峰"},
                {"PassWord", "ttttttt"}
            };
            var flag = HttpRequestUtil.HttpPost("api/User/put", dic);
            Console.WriteLine(flag);
        }

    }
}
