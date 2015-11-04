using System;
namespace Maticsoft.Model
{
	/// <summary>
	/// Travels:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Travels
	{
		public Travels()
		{}
		#region Model
		private int _id;
		private int _userid;
		private string _travelname;
		private DateTime _createtime;
		private DateTime? _updatetime;
		private string _coverimage;
		private bool _isdelete;
		/// <summary>
		/// 
		/// </summary>
		public int Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TravelName
		{
			set{ _travelname=value;}
			get{return _travelname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime CreateTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? UpdateTime
		{
			set{ _updatetime=value;}
			get{return _updatetime;}
		}
		/// <summary>
		/// 封面图
		/// </summary>
		public string CoverImage
		{
			set{ _coverimage=value;}
			get{return _coverimage;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool IsDelete
		{
			set{ _isdelete=value;}
			get{return _isdelete;}
		}
		#endregion Model

	}
}

