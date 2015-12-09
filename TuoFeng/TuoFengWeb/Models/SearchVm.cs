using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuoFengWeb.Models
{
    public class SearchVm
    {
        //user信息
        public int UserId { get; set; }

        //travel信息
        public int TravleId { get; set; }

        public string TravelName { get; set; }


        //travelparts信息
        public int TravelPartId { get; set; }

        public string Description { get; set; }

        private DateTime createTime;
        public string CreateTime {
            get { return createTime.ToShortDateString(); }
            set { createTime = DateTime.Parse(value); }
        }
    }
}