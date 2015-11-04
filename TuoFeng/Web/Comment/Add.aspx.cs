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
    public partial class Add : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                       
        }

        		protected void btnSave_Click(object sender, EventArgs e)
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
			int TravelPartId=int.Parse(this.txtTravelPartId.Text);
			int UserId=int.Parse(this.txtUserId.Text);
			string Content=this.txtContent.Text;
			int ToUserId=int.Parse(this.txtToUserId.Text);
			DateTime CreateTime=DateTime.Parse(this.txtCreateTime.Text);

			Maticsoft.Model.Comment model=new Maticsoft.Model.Comment();
			model.TravelPartId=TravelPartId;
			model.UserId=UserId;
			model.Content=Content;
			model.ToUserId=ToUserId;
			model.CreateTime=CreateTime;

			Maticsoft.BLL.Comment bll=new Maticsoft.BLL.Comment();
			bll.Add(model);
			Maticsoft.Common.MessageBox.ShowAndRedirect(this,"保存成功！","add.aspx");

		}


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
