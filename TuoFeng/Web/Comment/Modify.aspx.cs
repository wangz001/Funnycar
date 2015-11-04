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
namespace Maticsoft.Web.Comment
{
    public partial class Modify : Page
    {       

        		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				if (Request.Params["id"] != null && Request.Params["id"].Trim() != "")
				{
					long Id=(Convert.ToInt64(Request.Params["id"]));
					ShowInfo(Id);
				}
			}
		}
			
	private void ShowInfo(long Id)
	{
		Maticsoft.BLL.Comment bll=new Maticsoft.BLL.Comment();
		Maticsoft.Model.Comment model=bll.GetModel(Id);
		this.lblId.Text=model.Id.ToString();
		this.txtTravelPartId.Text=model.TravelPartId.ToString();
		this.txtUserId.Text=model.UserId.ToString();
		this.txtContent.Text=model.Content;
		this.txtToUserId.Text=model.ToUserId.ToString();
		this.txtCreateTime.Text=model.CreateTime.ToString();

	}

		public void btnSave_Click(object sender, EventArgs e)
		{
			
			string strErr="";
			if(!PageValidate.IsNumber(txtTravelPartId.Text))
			{
				strErr+="TravelPartId格式错误！\\n";	
			}
			if(!PageValidate.IsNumber(txtUserId.Text))
			{
				strErr+="UserId格式错误！\\n";	
			}
			if(this.txtContent.Text.Trim().Length==0)
			{
				strErr+="Content不能为空！\\n";	
			}
			if(!PageValidate.IsNumber(txtToUserId.Text))
			{
				strErr+="回复某人格式错误！\\n";	
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
			long Id=long.Parse(this.lblId.Text);
			int TravelPartId=int.Parse(this.txtTravelPartId.Text);
			int UserId=int.Parse(this.txtUserId.Text);
			string Content=this.txtContent.Text;
			int ToUserId=int.Parse(this.txtToUserId.Text);
			DateTime CreateTime=DateTime.Parse(this.txtCreateTime.Text);


			Maticsoft.Model.Comment model=new Maticsoft.Model.Comment();
			model.Id=Id;
			model.TravelPartId=TravelPartId;
			model.UserId=UserId;
			model.Content=Content;
			model.ToUserId=ToUserId;
			model.CreateTime=CreateTime;

			Maticsoft.BLL.Comment bll=new Maticsoft.BLL.Comment();
			bll.Update(model);
			Maticsoft.Common.MessageBox.ShowAndRedirect(this,"保存成功！","list.aspx");

		}


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
