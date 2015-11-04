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
using Maticsoft.Common;
using LTP.Accounts.Bus;
namespace Maticsoft.Web.TravelParts
{
    public partial class Modify : Page
    {       

        		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				if (Request.Params["id"] != null && Request.Params["id"].Trim() != "")
				{
					int Id=(Convert.ToInt32(Request.Params["id"]));
					ShowInfo(Id);
				}
			}
		}
			
	private void ShowInfo(int Id)
	{
		Maticsoft.BLL.TravelParts bll=new Maticsoft.BLL.TravelParts();
		Maticsoft.Model.TravelParts model=bll.GetModel(Id);
		this.lblId.Text=model.Id.ToString();
		this.txtTravelId.Text=model.TravelId.ToString();
		this.txtUserId.Text=model.UserId.ToString();
		this.txtPartType.Text=model.PartType.ToString();
		this.txtDescription.Text=model.Description;
		this.txtPartUrl.Text=model.PartUrl;
		this.txtLongitude.Text=model.Longitude.ToString();
		this.txtLatitude.Text=model.Latitude.ToString();
		this.txtHeight.Text=model.Height.ToString();
		this.txtArea.Text=model.Area;
		this.txtCreateTime.Text=model.CreateTime.ToString();
		this.chkIsDelete.Checked=model.IsDelete;

	}

		public void btnSave_Click(object sender, EventArgs e)
		{
			
			string strErr="";
			if(!PageValidate.IsNumber(txtTravelId.Text))
			{
				strErr+="TravelId格式错误！\\n";	
			}
			if(!PageValidate.IsNumber(txtUserId.Text))
			{
				strErr+="UserId格式错误！\\n";	
			}
			if(!PageValidate.IsNumber(txtPartType.Text))
			{
				strErr+="PartType格式错误！\\n";	
			}
			if(this.txtDescription.Text.Trim().Length==0)
			{
				strErr+="Description不能为空！\\n";	
			}
			if(this.txtPartUrl.Text.Trim().Length==0)
			{
				strErr+="PartUrl不能为空！\\n";	
			}
			if(!PageValidate.IsDecimal(txtLongitude.Text))
			{
				strErr+="Longitude格式错误！\\n";	
			}
			if(!PageValidate.IsDecimal(txtLatitude.Text))
			{
				strErr+="Latitude格式错误！\\n";	
			}
			if(!PageValidate.IsDecimal(txtHeight.Text))
			{
				strErr+="Height格式错误！\\n";	
			}
			if(this.txtArea.Text.Trim().Length==0)
			{
				strErr+="Area不能为空！\\n";	
			}
			if(!PageValidate.IsDateTime(txtCreateTime.Text))
			{
				strErr+="CreateTime格式错误！\\n";	
			}

			if(strErr!="")
			{
				MessageBox.Show(this,strErr);
				return;
			}
			int Id=int.Parse(this.lblId.Text);
			int TravelId=int.Parse(this.txtTravelId.Text);
			int UserId=int.Parse(this.txtUserId.Text);
			int PartType=int.Parse(this.txtPartType.Text);
			string Description=this.txtDescription.Text;
			string PartUrl=this.txtPartUrl.Text;
			decimal Longitude=decimal.Parse(this.txtLongitude.Text);
			decimal Latitude=decimal.Parse(this.txtLatitude.Text);
			decimal Height=decimal.Parse(this.txtHeight.Text);
			string Area=this.txtArea.Text;
			DateTime CreateTime=DateTime.Parse(this.txtCreateTime.Text);
			bool IsDelete=this.chkIsDelete.Checked;


			Maticsoft.Model.TravelParts model=new Maticsoft.Model.TravelParts();
			model.Id=Id;
			model.TravelId=TravelId;
			model.UserId=UserId;
			model.PartType=PartType;
			model.Description=Description;
			model.PartUrl=PartUrl;
			model.Longitude=Longitude;
			model.Latitude=Latitude;
			model.Height=Height;
			model.Area=Area;
			model.CreateTime=CreateTime;
			model.IsDelete=IsDelete;

			Maticsoft.BLL.TravelParts bll=new Maticsoft.BLL.TravelParts();
			bll.Update(model);
			Maticsoft.Common.MessageBox.ShowAndRedirect(this,"保存成功！","list.aspx");

		}


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
