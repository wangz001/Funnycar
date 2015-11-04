using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
namespace Maticsoft.Web.Thumb
{
    public partial class Show : Page
    {        
        		public string strid=""; 
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				if (Request.Params["id"] != null && Request.Params["id"].Trim() != "")
				{
					strid = Request.Params["id"];
					long Id=(Convert.ToInt64(strid));
					ShowInfo(Id);
				}
			}
		}
		
	private void ShowInfo(long Id)
	{
		Maticsoft.BLL.Thumb bll=new Maticsoft.BLL.Thumb();
		Maticsoft.Model.Thumb model=bll.GetModel(Id);
		this.lblId.Text=model.Id.ToString();
		this.lblTravelPartId.Text=model.TravelPartId.ToString();
		this.lblUserId.Text=model.UserId.ToString();
		this.lblIsDelete.Text=model.IsDelete?"是":"否";
		this.lblCreateTime.Text=model.CreateTime.ToString();

	}


    }
}
