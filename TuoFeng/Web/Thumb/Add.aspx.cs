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
namespace Maticsoft.Web.Thumb
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
			bool IsDelete=this.chkIsDelete.Checked;
			DateTime CreateTime=DateTime.Parse(this.txtCreateTime.Text);

			Maticsoft.Model.Thumb model=new Maticsoft.Model.Thumb();
			model.TravelPartId=TravelPartId;
			model.UserId=UserId;
			model.IsDelete=IsDelete;
			model.CreateTime=CreateTime;

			Maticsoft.BLL.Thumb bll=new Maticsoft.BLL.Thumb();
			bll.Add(model);
			Maticsoft.Common.MessageBox.ShowAndRedirect(this,"保存成功！","add.aspx");

		}


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
