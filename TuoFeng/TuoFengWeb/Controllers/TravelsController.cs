using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TuoFeng.BLL;
using TuoFeng.Model;

namespace TuoFengWeb.Controllers
{
    public class TravelsController : Controller
    {
        //
        // GET: /Travels/
        private readonly TravelsBll _travelsBll=new TravelsBll();
        private readonly TravelPartsBll _travelPartsBll=new TravelPartsBll();
        private readonly UserBll _userBll=new UserBll();
        private readonly ThumbBll _thumbBll=new ThumbBll();
        private readonly CommentBll _commentBll=new CommentBll();

        public ActionResult Index()
        {
            return null;
        }

        public string Create(string userId,string travelName)
        {
            if (string.IsNullOrEmpty(userId)||string.IsNullOrEmpty(travelName))
            {
                return HttpRequestResult.StateNotNull;
            }
            int useridInt;
            if (Int32.TryParse(userId,out useridInt)&&useridInt>0)
            {
                var model = new Travels
                {
                    TravelName = travelName,
                    UserId = useridInt,
                    IsDelete = false,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                var flag = _travelsBll.Add(model);
                return flag > 0 ? HttpRequestResult.StateOk : HttpRequestResult.StateError;
            }
            return HttpRequestResult.StateError;
        }

        /// <summary>
        /// 设置封面图
        /// </summary>
        /// <param name="travelId"></param>
        /// <param name="imgUrl"></param>
        /// <returns></returns>
        public string SetCoverImage(int travelId,string imgUrl)
        {

            return null;
        }

        //
        // POST: /Travels/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return null;
            }
        }

        //
        // GET: /Travels/Delete/5

        public ActionResult Delete(int id)
        {
            return null;
        }

        /// <summary>
        /// 分页获取最新的消息。以后可以加入标签，根据用户的兴趣获取，或者根据地理位置获取
        /// </summary>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public string GetTravelPartLists(int page, int count)
        {
            var resultArr = new List<string>();
            const string jsonItem = "{{" +
                                    "\"userName\": \"@userName\"" +
                                    ",\"userId\": \"@userId\"" +
                                    ",\"headImage\": \"@headImage\"" +
                                    ",\"sex\": \"@sex\"" +
                                    ",\"travelId\": \"@travelId\"" +
                                    ",\"travelName\": \"@travelName\"" +
                                    ",\"travelPartId\": \"@travelPartId\"" +
                                    ",\"description\": \"@description\"" +
                                    ",\"images\": \"@images\"" +
                                    ",\"location\": \"@location\"" +
                                    ",\"createTime\": \"@createTime\"" +
                                    ",\"thembCount\": \"@thembCount\"" +
                                    ",\"commentCount\": \"@commentCount\"" +
                                    "}}";
            var startIndex = (page-1)*count;
            var endIndex = startIndex + count;
            var travelParts = _travelPartsBll.GetListByPage("", "CreateTime", startIndex, endIndex);
            if (travelParts!=null&&travelParts.Tables[0].Rows.Count>0)
            {
                var lists = _travelPartsBll.DataTableToList(travelParts.Tables[0]);
                foreach (TravelParts travelPart in lists)
                {
                    int travelId = travelPart.TravelId ??0;
                    var travelName = (travelId > 0)? _travelsBll.GetModelByCache(travelId).TravelName: "";
                    var userId = travelPart.UserId;
                    var userModel = _userBll.GetModelByCache(userId);
                    var userName =userModel.UserName;
                    var headImage = ""+userModel.ImgUrl;
                    var sex = userModel.Sex?"男":"女";
                    var str = jsonItem.Replace("@userName", userName);
                    str=str.Replace("@userId", userId.ToString());
                    str = str.Replace("@headImage", headImage);
                    str = str.Replace("@sex", sex);
                    str = str.Replace("@travelId", travelId.ToString());
                    str = str.Replace("@travelName", travelName);
                    str = str.Replace("@travelPartId", travelPart.Id.ToString());
                    str = str.Replace("@description", travelPart.Description);
                    str = str.Replace("@images", travelPart.PartUrl);//图片或视频
                    str = str.Replace("@location", travelPart.Area);
                    str = str.Replace("@createTime", travelPart.CreateTime.ToString("yy-MM-dd hh:mm"));
                    str = str.Replace("@thembCount", "");//_thumbBll.GetModelByPartId(travelPart.Id)
                    str = str.Replace("@commentCount", "");//_commentBll.GetModelByPartId(travelPart.Id)
                    resultArr.Add(str);
                }
                if (resultArr.Count > 0)
                {
                    return string.Format("[{0}]", string.Join(",", resultArr));
                }
            }
            return string.Empty;
        }
        
    }
}
