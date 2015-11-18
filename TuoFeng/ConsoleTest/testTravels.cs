using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TuoFeng.BLL;
using TuoFeng.Model;

namespace ConsoleTest
{
    public class TestTravels
    {
        public static void addTravelParts()
        {
            var dic = new Dictionary<string,string>
            {
                {"userId", "2"},
                {"travelId", "1"},
                {"parttype", "1"},
                {"description", "银杏林"},
                {"area", "北京延庆"},
                {"partUrl", "http://img10.fblife.com/attachments/20151116/14476335102611.jpg"},
            };
            var flag = HttpRequestUtil.HttpPost("travels/AddTravelPart", dic);
            Console.WriteLine(flag);
        }

    }
}
