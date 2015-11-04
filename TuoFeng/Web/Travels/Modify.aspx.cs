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
namespace Maticsoft.Web.Travels
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
		Maticsoft.BLL.Travels bll=new Maticsoft.BLL.Travels();
		Maticsoft.Model.Travels model=bll.GetModel(Id);
		this.lblId.Text=model.Id.ToString();
		this.txtUserId.Text=model.UserId.ToString();
		this.txtTravelName.Text=model.TravelName;
		this.txtCreateTime.Text=model.CreateTime.ToString();
		this.txtUpdateTime.Text=model.UpdateTime.ToString();
		this.txtCoverImage.Text=model.CoverImage;
		this.chkIsDelete.Checked=model.IsDelete;

	}

		public void btnSave_Click(object sender, EventArgs e)
		{
			
			string strErr="";
			if(!PageValidate.IsNumber(txtUserId.Text))
			{
				strErr+="UserId格式错误！\\n";	
			}
			if(this.txtTravelName.Text.Trim().Length==0)
			{
				strErr+="TravelName不能为空！\\n";	
			}
			if(!PageValidate.IsDateTime(txtCreateTime.Text))
			{
				strErr+="CreateTime格式错误！\\n";	
			}
			if(!PageValidate.IsDateTime(txtUpdateTime.Text))
			{
				strErr+="UpdateTime格式错误！\\n";	
			}
			if(this.txtCoverImage.Text.Trim().Length==0)
			{
				strErr+="封面图不能为空！\\n";	
			}

			if(strErr!="")
			{
				MessageBox.Show(this,strErr);
				return;
			}
			int Id=int.Parse(this.lblId.Text);
			int UserId=int.Parse(this.txtUserId.Text);
			string TravelName=this.txtTravelName.Text;
			DateTime CreateTime=DateTime.Parse(this.txtCreateTime.Text);
			DateTime UpdateTime=DateTime.Parse(this.txtUpdateTime.Text);
			string CoverImage=this.txtCoverImage.Text;
			bool IsDelete=this.chkIsDelete.Checked;


			Maticsoft.Model.Travels model=new Maticsoft.Model.Travels();
			model.Id=Id;
			model.UserId=UserId;
			model.TravelName=TravelName;
			model.CreateTime=CreateTime;
			model.UpdateTime=UpdateTime;
			model.CoverImage=CoverImage;
			model.IsDelete=IsDelete;

			Maticsoft.BLL.Travels bll=new Maticsoft.BLL.Travels();
			bll.Update(model);
			Maticsoft.Common.MessageBox.ShowAndRedirect(this,"保存成功！","list.aspx");

		}


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
