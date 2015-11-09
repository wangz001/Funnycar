using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maticsoft.BLL;

namespace ConsoleTest
{
    class Program
    {
        public static void Main(string[] args)
        {
            User bll=new User();
            var flag=bll.Exists(5);

        }
    }
}
