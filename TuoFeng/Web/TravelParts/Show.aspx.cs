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
namespace Maticsoft.Web.TravelParts
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
					int Id=(Convert.ToInt32(strid));
					ShowInfo(Id);
				}
			}
		}
		
	private void ShowInfo(int Id)
	{
		Maticsoft.BLL.TravelParts bll=new Maticsoft.BLL.TravelParts();
		Maticsoft.Model.TravelParts model=bll.GetModel(Id);
		this.lblId.Text=model.Id.ToString();
		this.lblTravelId.Text=model.TravelId.ToString();
		this.lblUserId.Text=model.UserId.ToString();
		this.lblPartType.Text=model.PartType.ToString();
		this.lblDescription.Text=model.Description;
		this.lblPartUrl.Text=model.PartUrl;
		this.lblLongitude.Text=model.Longitude.ToString();
		this.lblLatitude.Text=model.Latitude.ToString();
		this.lblHeight.Text=model.Height.ToString();
		this.lblArea.Text=model.Area;
		this.lblCreateTime.Text=model.CreateTime.ToString();
		this.lblIsDelete.Text=model.IsDelete?"是":"否";

	}


    }
}
