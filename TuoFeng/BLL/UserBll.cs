using System;
using System.Collections.Generic;
using System.Data;
using TuoFeng.Model;

namespace TuoFeng.BLL
{
    /// <summary>
    /// User
    /// </summary>
    public class UserBll
    {
        private readonly DAL.UserDal dal = new DAL.UserDal();
        public UserBll()
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
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string UserName)
        {
            return dal.Exists(UserName);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(User model)
        {
            var id= dal.Add(model);
            if (id>0)
            {
                SetModelCache(id);
            }
            return id;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(User model)
        {
            var flag= dal.Update(model);
            if (true)
            {
                SetModelCache(model.Id);
            }
            return flag;
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
        private User GetModel(int Id)
        {

            return dal.GetModel(Id);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public User GetModelByCache(int Id)
        {

            string CacheKey = "UserModel-" + Id;
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
            return (User)objModel;
        }

        private void SetModelCache(int Id)
        {
            string CacheKey = "UserModel-" + Id;
            try
            {
                var objModel = dal.GetModel(Id);
                if (objModel != null)
                {
                    int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt("ModelCache");
                    Maticsoft.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache),
                        TimeSpan.Zero);
                }
            }
            catch
            {
            }
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
        public List<User> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<User> DataTableToList(DataTable dt)
        {
            List<User> modelList = new List<User>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                User model;
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

        public bool SetHeadImage(int userid, string imgUrl)
        {
            var flag = dal.SetHeadImage(userid, imgUrl);
            if (flag)
            {
                SetModelCache(userid);
            }
            return flag;
        }
    }
}

