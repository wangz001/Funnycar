using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Maticsoft.DBUtility;
//Please add references
using TuoFeng.Model;

namespace TuoFeng.DAL
{
	/// <summary>
	/// 数据访问类:TravelParts
	/// </summary>
	public partial class TravelPartsDal
	{
		public TravelPartsDal()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Id", "TravelParts"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from TravelParts");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)
			};
			parameters[0].Value = Id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(TravelParts model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into TravelParts(");
			strSql.Append("TravelId,UserId,PartType,Description,PartUrl,Longitude,Latitude,Height,Area,CreateTime,IsDelete)");
			strSql.Append(" values (");
			strSql.Append("@TravelId,@UserId,@PartType,@Description,@PartUrl,@Longitude,@Latitude,@Height,@Area,@CreateTime,@IsDelete)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@TravelId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@PartType", SqlDbType.TinyInt,1),
					new SqlParameter("@Description", SqlDbType.VarChar,-1),
					new SqlParameter("@PartUrl", SqlDbType.VarChar,100),
					new SqlParameter("@Longitude", SqlDbType.Decimal,9),
					new SqlParameter("@Latitude", SqlDbType.Decimal,9),
					new SqlParameter("@Height", SqlDbType.Decimal,9),
					new SqlParameter("@Area", SqlDbType.NVarChar,100),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@IsDelete", SqlDbType.Bit,1)};
			parameters[0].Value = model.TravelId;
			parameters[1].Value = model.UserId;
			parameters[2].Value = model.PartType;
			parameters[3].Value = model.Description;
			parameters[4].Value = model.PartUrl;
			parameters[5].Value = model.Longitude;
			parameters[6].Value = model.Latitude;
			parameters[7].Value = model.Height;
			parameters[8].Value = model.Area;
			parameters[9].Value = model.CreateTime;
			parameters[10].Value = model.IsDelete;

			object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(TravelParts model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update TravelParts set ");
			strSql.Append("TravelId=@TravelId,");
			strSql.Append("UserId=@UserId,");
			strSql.Append("PartType=@PartType,");
			strSql.Append("Description=@Description,");
			strSql.Append("PartUrl=@PartUrl,");
			strSql.Append("Longitude=@Longitude,");
			strSql.Append("Latitude=@Latitude,");
			strSql.Append("Height=@Height,");
			strSql.Append("Area=@Area,");
			strSql.Append("CreateTime=@CreateTime,");
			strSql.Append("IsDelete=@IsDelete");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@TravelId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@PartType", SqlDbType.TinyInt,1),
					new SqlParameter("@Description", SqlDbType.VarChar,-1),
					new SqlParameter("@PartUrl", SqlDbType.VarChar,100),
					new SqlParameter("@Longitude", SqlDbType.Decimal,9),
					new SqlParameter("@Latitude", SqlDbType.Decimal,9),
					new SqlParameter("@Height", SqlDbType.Decimal,9),
					new SqlParameter("@Area", SqlDbType.NVarChar,100),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@IsDelete", SqlDbType.Bit,1),
					new SqlParameter("@Id", SqlDbType.Int,4)};
			parameters[0].Value = model.TravelId;
			parameters[1].Value = model.UserId;
			parameters[2].Value = model.PartType;
			parameters[3].Value = model.Description;
			parameters[4].Value = model.PartUrl;
			parameters[5].Value = model.Longitude;
			parameters[6].Value = model.Latitude;
			parameters[7].Value = model.Height;
			parameters[8].Value = model.Area;
			parameters[9].Value = model.CreateTime;
			parameters[10].Value = model.IsDelete;
			parameters[11].Value = model.Id;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from TravelParts ");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)
			};
			parameters[0].Value = Id;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string Idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from TravelParts ");
			strSql.Append(" where Id in ("+Idlist + ")  ");
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public TravelParts GetModel(int Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Id,TravelId,UserId,PartType,Description,PartUrl,Longitude,Latitude,Height,Area,CreateTime,IsDelete from TravelParts ");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)
			};
			parameters[0].Value = Id;

			TravelParts model=new TravelParts();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public TravelParts DataRowToModel(DataRow row)
		{
			TravelParts model=new TravelParts();
			if (row != null)
			{
				if(row["Id"]!=null && row["Id"].ToString()!="")
				{
					model.Id=int.Parse(row["Id"].ToString());
				}
				if(row["TravelId"]!=DBNull.Value && row["TravelId"].ToString()!="")
				{
					model.TravelId=int.Parse(row["TravelId"].ToString());
				}
				if(row["UserId"]!=null && row["UserId"].ToString()!="")
				{
					model.UserId=int.Parse(row["UserId"].ToString());
				}
				if(row["PartType"]!=null && row["PartType"].ToString()!="")
				{
					model.PartType=int.Parse(row["PartType"].ToString());
				}
				if(row["Description"]!=null)
				{
					model.Description=row["Description"].ToString();
				}
				if(row["PartUrl"]!=null)
				{
					model.PartUrl=row["PartUrl"].ToString();
				}
				if(row["Longitude"]!=null && row["Longitude"].ToString()!="")
				{
					model.Longitude=decimal.Parse(row["Longitude"].ToString());
				}
				if(row["Latitude"]!=null && row["Latitude"].ToString()!="")
				{
					model.Latitude=decimal.Parse(row["Latitude"].ToString());
				}
				if(row["Height"]!=null && row["Height"].ToString()!="")
				{
					model.Height=decimal.Parse(row["Height"].ToString());
				}
				if(row["Area"]!=null)
				{
					model.Area=row["Area"].ToString();
				}
				if(row["CreateTime"]!=null && row["CreateTime"].ToString()!="")
				{
					model.CreateTime=DateTime.Parse(row["CreateTime"].ToString());
				}
				if(row["IsDelete"]!=null && row["IsDelete"].ToString()!="")
				{
					if((row["IsDelete"].ToString()=="1")||(row["IsDelete"].ToString().ToLower()=="true"))
					{
						model.IsDelete=true;
					}
					else
					{
						model.IsDelete=false;
					}
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Id,TravelId,UserId,PartType,Description,PartUrl,Longitude,Latitude,Height,Area,CreateTime,IsDelete ");
			strSql.Append(" FROM TravelParts ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" Id,TravelId,UserId,PartType,Description,PartUrl,Longitude,Latitude,Height,Area,CreateTime,IsDelete ");
			strSql.Append(" FROM TravelParts ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM TravelParts ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = DbHelperSQL.GetSingle(strSql.ToString());
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.Id desc");
			}
			strSql.Append(")AS Row, T.*  from TravelParts T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "TravelParts";
			parameters[1].Value = "Id";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod

	    public DataSet GetModelListByTravelId(int travelid, int page, int count)
	    {
	        var startIndex = (page-1)*count;
	        var endIndex = page*count;
	        var sqlStr = @"
SELECT  *
FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY T.createtime ) AS Row ,
                    T.*
          FROM      TravelParts T
          WHERE     TravelId = @TravelId  AND IsDelete=0
        ) AS TT
WHERE   tt.Row BETWEEN @startIndex AND @endIndex
";
            SqlParameter[] para =
            {
                new SqlParameter("@TravelId", SqlDbType.Int) { Value = travelid },
                new SqlParameter("@startIndex",SqlDbType.Int){Value = startIndex}, 
                new SqlParameter("@endIndex",SqlDbType.Int){Value = endIndex}
            };

            return DbHelperSQL.Query(sqlStr,para);
	    }
	}
}

