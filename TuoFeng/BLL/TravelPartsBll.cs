﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TuoFeng.DAL;
using TuoFeng.Model;

namespace TuoFeng.BLL
{
	/// <summary>
	/// TravelParts
	/// </summary>
	public partial class TravelPartsBll
	{
		private readonly TravelPartsDal dal=new TravelPartsDal();
		public TravelPartsBll()
		{}
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
		public int  Add(TravelParts model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(TravelParts model)
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
		public bool DeleteList(string Idlist )
		{
			return dal.DeleteList(Idlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public TravelParts GetModel(int Id)
		{
			
			return dal.GetModel(Id);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public TravelParts GetModelByCache(int Id)
		{
			
			string CacheKey = "TravelPartsModel-" + Id;
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
				catch{}
			}
			return (TravelParts)objModel;
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
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<TravelParts> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<TravelParts> DataTableToList(DataTable dt)
		{
			List<TravelParts> modelList = new List<TravelParts>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				TravelParts model;
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
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
		}

        /// <summary>
        /// 获取用户的说说
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
	    public DataSet GetListByUserId(int userid, int startIndex, int endIndex)
	    {
	        var strWhere = " UserId=" + userid;
            return dal.GetListByPage(strWhere, "CreateTime desc ", startIndex, endIndex);
	    }

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod

	    public List<TravelParts> GetPartListsByTravelId(int travelid, int page, int count)
	    {
	        var startIndex = (page - 1)*count+1;
	        var endIndex = page*count;
	        var ds = dal.GetListByPage(" TravelId="+travelid, " CreateTime", startIndex, endIndex);
	        if (ds!=null&&ds.Tables[0]!=null&&ds.Tables[0].Rows.Count>0)
	        {
	            return DataTableToList(ds.Tables[0]);
	        }
	        return null;
	    }

	    /// <summary>
	    /// 首页，根据id来分页获取数据，防止重复
	    /// </summary>
	    /// <param name="minId"></param>
	    /// <param name="count"></param>
	    /// <param name="partId"></param>
	    /// <param name="isPullUp">true 表示上拉分页。false表示下拉刷新</param>
	    /// <returns></returns>
	    public DataSet GetListByIdRange(int count,int partId, bool isPullUp)
        {
            return  dal.GetListByIdRange(count,partId,isPullUp);
        }
	}
}

