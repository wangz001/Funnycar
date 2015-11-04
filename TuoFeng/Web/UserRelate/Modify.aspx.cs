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
namespace Maticsoft.Web.UserRelate
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
		Maticsoft.BLL.UserRelate bll=new Maticsoft.BLL.UserRelate();
		Maticsoft.Model.UserRelate model=bll.GetModel(Id);
		this.lblId.Text=model.Id.ToString();
		this.txtUserId.Text=model.UserId.ToString();
		this.txtRelateUserId.Text=model.RelateUserId.ToString();
		this.chkIsDelete.Checked=model.IsDelete;
		this.txtCreateTime.Text=model.CreateTime.ToString();

	}

		public void btnSave_Click(object sender, EventArgs e)
		{
			
			string strErr="";
			if(!PageValidate.IsNumber(txtUserId.Text))
			{
				strErr+="UserId格式错误！\\n";	
			}
			if(!PageValidate.IsNumber(txtRelateUserId.Text))
			{
				strErr+="RelateUserId格式错误！\\n";	
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
			int UserId=int.Parse(this.txtUserId.Text);
			int RelateUserId=int.Parse(this.txtRelateUserId.Text);
			bool IsDelete=this.chkIsDelete.Checked;
			DateTime CreateTime=DateTime.Parse(this.txtCreateTime.Text);


			Maticsoft.Model.UserRelate model=new Maticsoft.Model.UserRelate();
			model.Id=Id;
			model.UserId=UserId;
			model.RelateUserId=RelateUserId;
			model.IsDelete=IsDelete;
			model.CreateTime=CreateTime;

			Maticsoft.BLL.UserRelate bll=new Maticsoft.BLL.UserRelate();
			bll.Update(model);
			Maticsoft.Common.MessageBox.ShowAndRedirect(this,"保存成功！","list.aspx");

		}


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
