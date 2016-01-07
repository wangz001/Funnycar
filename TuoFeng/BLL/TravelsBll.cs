using System;
using System.Collections.Generic;
using System.Data;
using TuoFeng.DAL;
using TuoFeng.Model;

namespace TuoFeng.BLL
{
    /// <summary>
    /// Travels
    /// </summary>
    public class TravelsBll
    {
        private readonly TravelsDal dal = new TravelsDal();
        public TravelsBll()
        { }
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Id)
        {
            return dal.Exists(Id);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Travels model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Travels model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int Id)
        {

            return dal.Delete(Id);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string Idlist)
        {
            return dal.DeleteList(Idlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Travels GetModel(int Id)
        {

            return dal.GetModel(Id);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public Travels GetModelByCache(int Id)
        {

            string CacheKey = "TravelsModel-" + Id;
            object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(Id);
                    if (objModel != null)
                    {
                        int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt("ModelCache");
                        Maticsoft.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (Travels)objModel;
        }

        private void SetModelCache(int travelId)
        {
            string CacheKey = "TravelsModel-" + travelId;
            try
            {
                var model = dal.GetModel(travelId);
                if (model != null)
                {
                    int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt("ModelCache");
                    Maticsoft.Common.DataCache.SetCache(CacheKey, model, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                }
            }
            catch { }
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Travels> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Travels> DataTableToList(DataTable dt)
        {
            List<Travels> modelList = new List<Travels>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Travels model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = dal.DataRowToModel(dt.Rows[n]);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod

        public bool SetCoverImage(int travelId, string imgUrl)
        {
            var flag = dal.SetCoverImage(travelId, imgUrl);
            if (flag)
            {
                SetModelCache(travelId);
            }
            return flag;
        }

        public List<TravelVm> GetMyTravels(int page, int count, int userid)
        {
            var list=new List<TravelVm>();
            var ds = dal.GetMyTravels(page,count,userid);
            if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows.Count <= 0) return list;
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                var partId = Int32.Parse(dataRow["id"].ToString());
                var travelId = 0;
                if (dataRow["TravelId"] != DBNull.Value && !string.IsNullOrEmpty(dataRow["TravelId"].ToString()))
                {
                    travelId = Int32.Parse(dataRow["TravelId"].ToString());
                }
                var partType = Int32.Parse(dataRow["PartType"].ToString());
                var description = dataRow["Description"].ToString();
                var partUrl = dataRow["PartUrl"]!=DBNull.Value?dataRow["PartUrl"].ToString():"";
                var longitude = dataRow["Longitude"] != DBNull.Value
                    ? Decimal.Parse(dataRow["Longitude"].ToString())
                    : 0;
                var latitude = dataRow["Latitude"] != DBNull.Value
                    ? Decimal.Parse(dataRow["Latitude"].ToString())
                    : 0;
                var height = dataRow["Height"] != DBNull.Value
                    ? Decimal.Parse(dataRow["Height"].ToString())
                    : 0;
                var area = dataRow["Area"] != DBNull.Value
                    ? dataRow["Area"].ToString()
                    : "";
                var createtime = DateTime.Parse(dataRow["CreateTime"].ToString());
                var travel=new Travels();
                if (travelId>0)
                {
                    travel = GetModelByCache(travelId);
                }

                var vm = new TravelVm()
                {
                    TravelName = travel != null ? travel.TravelName : "",
                    CoverImage = travel != null ? travel.CoverImage : "",
                    Id = partId,
                    TravelId = travelId,
                    PartType = partType,
                    PartUrl = partUrl,
                    Description = description,
                    Longitude = longitude,
                    Latitude = latitude,
                    Height = height,
                    Area = area,
                    CreateTime = createtime,
                };
                list.Add(vm);
            }
            return list;
        }


        public class TravelVm:TravelParts
        {
            public string TravelName { get; set; }

            /// <summary>
            /// 游记封面图
            /// </summary>
            public string CoverImage { get; set; }


        }
    }
}

