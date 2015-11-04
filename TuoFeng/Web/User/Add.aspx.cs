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
namespace Maticsoft.Web.User
{
    public partial class Add : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                       
        }

        		protected void btnSave_Click(object sender, EventArgs e)
		{
			
			string strErr="";
			if(this.txtUserName.Text.Trim().Length==0)
			{
				strErr+="UserName不能为空！\\n";	
			}
			if(this.txtShowName.Text.Trim().Length==0)
			{
				strErr+="ShowName不能为空！\\n";	
			}
			if(this.txtPassWord.Text.Trim().Length==0)
			{
				strErr+="PassWord不能为空！\\n";	
			}
			if(this.txtPhoneNum.Text.Trim().Length==0)
			{
				strErr+="PhoneNum不能为空！\\n";	
			}
			if(this.txtEmail.Text.Trim().Length==0)
			{
				strErr+="Email不能为空！\\n";	
			}
			if(!PageValidate.IsNumber(txtAge.Text))
			{
				strErr+="Age格式错误！\\n";	
			}
			if(this.txtImgUrl.Text.Trim().Length==0)
			{
				strErr+="ImgUrl不能为空！\\n";	
			}
			if(!PageValidate.IsDateTime(txtCreateTime.Text))
			{
				strErr+="CreateTime格式错误！\\n";	
			}
			if(!PageValidate.IsDateTime(txtUpdateTime.Text))
			{
				strErr+="UpdateTime格式错误！\\n";	
			}

			if(strErr!="")
			{
				MessageBox.Show(this,strErr);
				return;
			}
			string UserName=this.txtUserName.Text;
			string ShowName=this.txtShowName.Text;
			string PassWord=this.txtPassWord.Text;
			bool Sex=this.chkSex.Checked;
			string PhoneNum=this.txtPhoneNum.Text;
			string Email=this.txtEmail.Text;
			int Age=int.Parse(this.txtAge.Text);
			string ImgUrl=this.txtImgUrl.Text;
			bool IsEnable=this.chkIsEnable.Checked;
			DateTime CreateTime=DateTime.Parse(this.txtCreateTime.Text);
			DateTime UpdateTime=DateTime.Parse(this.txtUpdateTime.Text);

			Maticsoft.Model.User model=new Maticsoft.Model.User();
			model.UserName=UserName;
			model.ShowName=ShowName;
			model.PassWord=PassWord;
			model.Sex=Sex;
			model.PhoneNum=PhoneNum;
			model.Email=Email;
			model.Age=Age;
			model.ImgUrl=ImgUrl;
			model.IsEnable=IsEnable;
			model.CreateTime=CreateTime;
			model.UpdateTime=UpdateTime;

			Maticsoft.BLL.User bll=new Maticsoft.BLL.User();
			bll.Add(model);
			Maticsoft.Common.MessageBox.ShowAndRedirect(this,"保存成功！","add.aspx");

		}


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
