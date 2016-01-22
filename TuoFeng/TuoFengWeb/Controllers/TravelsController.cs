using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Maticsoft.Common;
using SolrNet.Commands.Parameters;
using TuoFeng.BLL;
using TuoFeng.Model;

namespace TuoFengWeb.Controllers
{
    public class TravelsController : Controller
    {
        //
        // GET: /Travels/
        private readonly TravelsBll _travelsBll = new TravelsBll();
        private readonly TravelPartsBll _travelPartsBll = new TravelPartsBll();
        private readonly UserBll _userBll = new UserBll();
        private readonly ThumbBll _thumbBll = new ThumbBll();
        private readonly CommentBll _commentBll = new CommentBll();

        private readonly string _baseUrl = ConfigHelper.GetConfigString("AppImgBaseUrl");
        public ActionResult Index()
        {
            const string title = "首页";


            ViewBag.Title = title;
            return View();
        }

        public string Create(string userId, string travelName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(travelName))
            {
                return HttpRequestResult.StateNotNull;
            }
            int useridInt;
            if (Int32.TryParse(userId, out useridInt) && useridInt > 0)
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
        public string SetCoverImage(int travelId, string imgUrl)
        {
            if (travelId <= 0 || string.IsNullOrEmpty(imgUrl)) return HttpRequestResult.StateNotNull;
            var falg = _travelsBll.SetCoverImage(travelId, imgUrl);
            if (falg)
            {
                return HttpRequestResult.StateOk;
            }
            else
            {
                return HttpRequestResult.StateError;
            }
        }


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
        /// 获取用户的游记列表，
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetTravelNamesByUserId(int userid)
        {
            var itemList = new List<string>();
            var list = _travelsBll.GetModelList(" UserId=" + userid + " and IsDelete=0").OrderByDescending(m => m.CreateTime);
            if (list.Any())
            {
                itemList.AddRange(list.Select(travel => string.Format("{{\"travelid\":\"{0}\",\"travelname\":\"{1}\"}}", travel.Id, travel.TravelName)));
                return string.Format("[{0}]", string.Join(",", itemList));
            }
            return string.Empty;
        }

        public ActionResult NewTravelParts(int userid)
        {
            ViewBag.User = _userBll.GetModelByCache(userid);
            return View();
        }

        [HttpPost]
        public string AddTravelPart(FormCollection collection)
        {
            var travelIdStr = collection.Get("travelId");
            var userIdStr = collection.Get("userId");
            var partType = collection.Get("partType");
            var description = collection.Get("description");
            var partUrl = collection.Get("partUrl");
            var longitude = collection.Get("longitude");
            var latitude = collection.Get("latitude");
            var area = collection.Get("address");
            if (string.IsNullOrEmpty(userIdStr)) return HttpRequestResult.StateNotNull;
            if (string.IsNullOrEmpty(description) && string.IsNullOrEmpty(partUrl))
                return HttpRequestResult.StateNotNull;
            var model = new TravelParts();
            {
                model.UserId = Int32.Parse(userIdStr);
                if (!string.IsNullOrEmpty(travelIdStr))
                {
                    var travelId = Int32.Parse(travelIdStr);
                    if (travelId > 0)
                    {
                        model.TravelId = travelId;
                    }
                }
                model.PartType = Int32.Parse(partType);
                model.Description = description;
                model.PartUrl = partUrl;
                if (!string.IsNullOrEmpty(longitude) && !string.IsNullOrEmpty(latitude))
                {
                    model.Longitude = decimal.Parse(longitude);
                    model.Latitude = decimal.Parse(latitude);
                }
                if (!string.IsNullOrEmpty(area)) model.Area = area;
                model.CreateTime = DateTime.Now;
                model.IsDelete = false;
            };
            var flag = _travelPartsBll.Add(model);
            if (flag > 0)
            {
                return HttpRequestResult.StateOk;
            }
            else
            {
                return HttpRequestResult.StateError;
            }
        }

        /// <summary>
        /// 分页获取最新的消息。以后可以加入标签，根据用户的兴趣获取，或者根据地理位置获取
        /// </summary>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <param name="userid">有userid的时候，是从添加页跳过去的。否则是直接打开首页</param>
        /// <returns></returns>
        public string GetTravelPartLists(int page, int count, int? userid)
        {
            var resultArr = new List<string>();
            const string jsonItem = "{" +
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
                                    "}";
            var startIndex = (page - 1) * count + 1;
            var endIndex = page * count;
            var travelParts = _travelPartsBll.GetListByPage(" IsDelete=0", "CreateTime desc", startIndex, endIndex);
            if (travelParts != null && travelParts.Tables[0].Rows.Count > 0)
            {
                var lists = _travelPartsBll.DataTableToList(travelParts.Tables[0]);
                foreach (TravelParts travelPart in lists)
                {
                    int travelId = travelPart.TravelId ?? 0;
                    var travelName = (travelId > 0) ? _travelsBll.GetModelByCache(travelId).TravelName : "";
                    var userId = travelPart.UserId;
                    var userModel = _userBll.GetModelByCache(userId);
                    var userName = userModel.UserName;
                    var headImage = "" + userModel.ImgUrl;
                    var sex = userModel.Sex ? "男" : "女";
                    var str = jsonItem.Replace("@userName", userName);
                    str = str.Replace("@userId", userId.ToString());
                    str = str.Replace("@headImage", headImage);
                    str = str.Replace("@sex", sex);
                    str = str.Replace("@travelId", travelId.ToString());
                    str = str.Replace("@travelName", travelName);
                    str = str.Replace("@travelPartId", travelPart.Id.ToString());
                    str = str.Replace("@description", travelPart.Description);
                    var imagesStr = string.Empty;
                    if (!string.IsNullOrEmpty(travelPart.PartUrl))
                    {
                        var arr = travelPart.PartUrl.Split(',');
                        var tempimg = arr[0];
                        tempimg = tempimg.Replace("http://appimg.impinker.cn", "");
                        imagesStr = _baseUrl + tempimg;
                    }
                    str = str.Replace("@images", imagesStr);//图片或视频
                    str = str.Replace("@location", travelPart.Area);
                    str = str.Replace("@createTime", travelPart.CreateTime.ToString("yy-MM-dd HH:mm"));
                    str = str.Replace("@thembCount", _thumbBll.GetThembCountByPartId(travelPart.Id).ToString());//
                    str = str.Replace("@commentCount", _commentBll.GetCommentCountByPartId(travelPart.Id).ToString());//
                    resultArr.Add(str);
                }
                if (resultArr.Count > 0)
                {
                    return string.Format("[{0}]", string.Join(",", resultArr));
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 首页分页获取数据，用id来区分，不用page。userid用来统计用户行为
        /// </summary>
        /// <param name="count"></param>
        /// <param name="minId"></param>
        /// <param name="maxId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetIndexLists(int count, int minId, int maxId, int? userid)
        {
            if (count > 20) count = 20;
            if (count < 1) count = 10;

            var resultArr = new List<string>();
            const string jsonItem = "{" +
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
                                    "}";
            DataSet travelParts;
            if (minId > 0 && maxId == 0)
            {
                //上拉分页获取
                travelParts = _travelPartsBll.GetListByIdRange(count, minId, true);
            }
            else if (minId == 0 && maxId > 0)
            {
                //下拉刷新
                travelParts = _travelPartsBll.GetListByIdRange(count, maxId, false);
            }
            else
            {
                return string.Empty;
            }
            if (travelParts != null && travelParts.Tables[0].Rows.Count > 0)
            {
                var lists = _travelPartsBll.DataTableToList(travelParts.Tables[0]);
                foreach (TravelParts travelPart in lists)
                {
                    int travelId = travelPart.TravelId ?? 0;
                    var travelName = (travelId > 0) ? _travelsBll.GetModelByCache(travelId).TravelName : "";
                    var userId = travelPart.UserId;
                    var userModel = _userBll.GetModelByCache(userId);
                    var userName = userModel.UserName;
                    var headImage = "" + userModel.ImgUrl;
                    var sex = userModel.Sex ? "男" : "女";
                    var str = jsonItem.Replace("@userName", userName);
                    str = str.Replace("@userId", userId.ToString());
                    str = str.Replace("@headImage", headImage);
                    str = str.Replace("@sex", sex);
                    str = str.Replace("@travelId", travelId.ToString());
                    str = str.Replace("@travelName", travelName);
                    str = str.Replace("@travelPartId", travelPart.Id.ToString());
                    str = str.Replace("@description", travelPart.Description);
                    var imagesStr = string.Empty;
                    if (!string.IsNullOrEmpty(travelPart.PartUrl))
                    {
                        var arr = travelPart.PartUrl.Split(',');
                        var tempimg = arr[0];
                        tempimg = tempimg.Replace("http://appimg.impinker.cn", "");
                        imagesStr = _baseUrl + tempimg;
                    }
                    str = str.Replace("@images", imagesStr);//图片或视频
                    str = str.Replace("@location", travelPart.Area);
                    str = str.Replace("@createTime", travelPart.CreateTime.ToString("yy-MM-dd HH:mm"));
                    str = str.Replace("@thembCount", _thumbBll.GetThembCountByPartId(travelPart.Id).ToString());//
                    str = str.Replace("@commentCount", _commentBll.GetCommentCountByPartId(travelPart.Id).ToString());//
                    resultArr.Add(str);
                }
                if (resultArr.Count > 0)
                {
                    return string.Format("[{0}]", string.Join(",", resultArr));
                }
            }
            return string.Empty;
        }

        public ActionResult MyTravels(int userid)
        {
            ViewBag.User = _userBll.GetModelByCache(userid);
            return View();
        }

        /// <summary>
        /// 获取个人的游记， 不属于游记的part数据也要，按时间倒序排序
        /// </summary>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetMyTravels(int page, int count, int userid)
        {
            var lists = _travelsBll.GetMyTravels(page, count, userid);
            if (lists == null || lists.Count == 0) return string.Empty;
            var resultArr = new List<string>();
            const string jsonItem = "{" +
                                    "\"travelId\": \"@travelId\"" +
                                    ",\"travelName\": \"@travelName\"" +
                                    ",\"travelPartId\": \"@travelPartId\"" +
                                    ",\"description\": \"@description\"" +
                                    ",\"images\": \"@images\"" +
                                    ",\"location\": \"@location\"" +
                                    ",\"createTime\": \"@createTime\"" +
                                    ",\"thembCount\": \"@thembCount\"" +
                                    ",\"commentCount\": \"@commentCount\"" +
                                    "}";
            foreach (TravelsBll.TravelVm vm in lists)
            {
                var str = jsonItem.Replace("@travelId", vm.TravelId == null ? "0" : vm.TravelId.ToString());
                str = str.Replace("@travelName", vm.TravelName);
                str = str.Replace("@travelPartId", vm.Id.ToString());
                str = str.Replace("@description", vm.Description);
                str = str.Replace("@location", vm.Area);
                str = str.Replace("@createTime", vm.CreateTime.ToShortDateString());
                str = str.Replace("@thembCount", "0");
                str = str.Replace("@commentCount", "0");
                var imagesStr = string.Empty;
                if (!string.IsNullOrEmpty(vm.PartUrl))
                {
                    var arr = vm.PartUrl.Split(',');
                    var tempimg = arr[0];
                    tempimg = tempimg.Replace("http://appimg.impinker.cn", "");
                    imagesStr = _baseUrl + tempimg;
                }
                str = str.Replace("@images", imagesStr);//图片或视频

                resultArr.Add(str);
            }
            if (resultArr.Count > 0)
            {
                return string.Format("[{0}]", string.Join(",", resultArr));
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取个人的游记章节，按时间倒序排序
        /// </summary>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetMyTravelParts(int page, int count, int userid)
        {
            var resultArr = new List<string>();
            const string jsonItem = "{" +
                                    "\"travelId\": \"@travelId\"" +
                                    ",\"travelName\": \"@travelName\"" +
                                    ",\"travelPartId\": \"@travelPartId\"" +
                                    ",\"description\": \"@description\"" +
                                    ",\"images\": \"@images\"" +
                                    ",\"location\": \"@location\"" +
                                    ",\"createTime\": \"@createTime\"" +
                                    ",\"thembCount\": \"@thembCount\"" +
                                    ",\"commentCount\": \"@commentCount\"" +
                                    "}";
            var startIndex = (page - 1) * count;
            var endIndex = startIndex + count - 1;
            var travelParts = _travelPartsBll.GetListByUserId(userid, startIndex, endIndex);
            if (travelParts != null && travelParts.Tables[0].Rows.Count > 0)
            {
                var lists = _travelPartsBll.DataTableToList(travelParts.Tables[0]);
                foreach (TravelParts travelPart in lists)
                {
                    int travelId = travelPart.TravelId ?? 0;
                    var travelName = (travelId > 0) ? _travelsBll.GetModelByCache(travelId).TravelName : "";
                    var userId = travelPart.UserId;
                    var userModel = _userBll.GetModelByCache(userId);
                    var userName = userModel.UserName;
                    var headImage = "" + userModel.ImgUrl;
                    var sex = userModel.Sex ? "男" : "女";
                    var str = jsonItem.Replace("@userName", userName);
                    //str = str.Replace("@userId", userId.ToString());
                    //str = str.Replace("@headImage", headImage);
                    //str = str.Replace("@sex", sex);
                    str = str.Replace("@travelId", travelId.ToString());
                    str = str.Replace("@travelName", travelName);
                    str = str.Replace("@travelPartId", travelPart.Id.ToString());
                    str = str.Replace("@description", travelPart.Description);
                    var imagesStr = string.Empty;
                    if (!string.IsNullOrEmpty(travelPart.PartUrl))
                    {
                        var arr = travelPart.PartUrl.Split(',');
                        var tempimg = arr[0];
                        tempimg = tempimg.Replace("http://appimg.impinker.cn", "");
                        imagesStr = _baseUrl + tempimg;
                    }
                    str = str.Replace("@images", imagesStr);//图片或视频
                    str = str.Replace("@location", travelPart.Area);
                    str = str.Replace("@createTime", travelPart.CreateTime.ToString("yy-MM-dd hh:mm"));
                    str = str.Replace("@thembCount", _thumbBll.GetThembCountByPartId(travelPart.Id).ToString());//
                    str = str.Replace("@commentCount", _commentBll.GetCommentCountByPartId(travelPart.Id).ToString());//
                    resultArr.Add(str);
                }
                if (resultArr.Count > 0)
                {
                    return string.Format("[{0}]", string.Join(",", resultArr));
                }
            }
            return string.Empty;
        }

        public ActionResult TravelPreview(int travelid)
        {
            var defaultImg = "http://gqianniu.alicdn.com/bao/uploaded/i4//tfscom/i3/TB10LfcHFXXXXXKXpXXXXXXXXXX_!!0-item_pic.jpg_250x250q60.jpg";
            var travel = _travelsBll.GetModelByCache(travelid);
            if (!string.IsNullOrEmpty(travel.CoverImage) && defaultImg != travel.CoverImage)
            {
                travel.CoverImage = _baseUrl + travel.CoverImage;
            }
            else
            {
                travel.CoverImage = defaultImg;
            }
            ViewBag.travel = travel;
            return View();
        }


        public string GetTravelPartsByTravelId(int page, int count, int travelid)
        {
            var resultArr = new List<string>();
            const string jsonItem = "{" +
                                    "\"travelPartId\": \"@travelPartId\"" +
                                    ",\"description\": \"@description\"" +
                                    ",\"images\": \"@images\"" +
                                    ",\"location\": \"@location\"" +
                                    ",\"createTime\": \"@createTime\"" +
                                    ",\"thembCount\": \"@thembCount\"" +
                                    ",\"commentCount\": \"@commentCount\"" +
                                    "}";
            if (travelid > 0)
            {
                var lists = _travelsBll.GetPartListsByTravelId(travelid, page, count);
                if (lists != null && lists.Count > 0)
                {
                    foreach (TravelParts travelPart in lists)
                    {
                        var str = jsonItem.Replace("@travelPartId", travelPart.Id.ToString());
                        str = str.Replace("@description", travelPart.Description);
                        var imagesStr = string.Empty;
                        if (!string.IsNullOrEmpty(travelPart.PartUrl))
                        {
                            var arr = travelPart.PartUrl.Split(',');
                            var tempimg = arr[0];
                            tempimg = tempimg.Replace("http://appimg.impinker.cn", "");
                            imagesStr = _baseUrl + tempimg;
                        }
                        str = str.Replace("@images", imagesStr);//图片或视频
                        str = str.Replace("@location", travelPart.Area);
                        str = str.Replace("@createTime", travelPart.CreateTime.ToString("yy-MM-dd hh:mm"));
                        str = str.Replace("@thembCount", _thumbBll.GetThembCountByPartId(travelPart.Id).ToString());//
                        str = str.Replace("@commentCount", _commentBll.GetCommentCountByPartId(travelPart.Id).ToString());//
                        resultArr.Add(str);
                    }
                    if (resultArr.Count > 0)
                    {
                        return string.Format("[{0}]", string.Join(",", resultArr));
                    }
                }
            }
            return string.Empty;
        }
    }
}
