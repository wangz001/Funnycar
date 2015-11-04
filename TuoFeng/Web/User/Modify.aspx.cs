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
		Maticsoft.BLL.User bll=new Maticsoft.BLL.User();
		Maticsoft.Model.User model=bll.GetModel(Id);
		this.lblId.Text=model.Id.ToString();
		this.txtUserName.Text=model.UserName;
		this.txtShowName.Text=model.ShowName;
		this.txtPassWord.Text=model.PassWord;
		this.chkSex.Checked=model.Sex;
		this.txtPhoneNum.Text=model.PhoneNum;
		this.txtEmail.Text=model.Email;
		this.txtAge.Text=model.Age.ToString();
		this.txtImgUrl.Text=model.ImgUrl;
		this.chkIsEnable.Checked=model.IsEnable;
		this.txtCreateTime.Text=model.CreateTime.ToString();
		this.txtUpdateTime.Text=model.UpdateTime.ToString();

	}

		public void btnSave_Click(object sender, EventArgs e)
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
			int Id=int.Parse(this.lblId.Text);
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
			model.Id=Id;
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
			bll.Update(model);
			Maticsoft.Common.MessageBox.ShowAndRedirect(this,"保存成功！","list.aspx");

		}


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
