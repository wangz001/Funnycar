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
namespace Maticsoft.Web.User
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
		Maticsoft.BLL.User bll=new Maticsoft.BLL.User();
		Maticsoft.Model.User model=bll.GetModel(Id);
		this.lblId.Text=model.Id.ToString();
		this.lblUserName.Text=model.UserName;
		this.lblShowName.Text=model.ShowName;
		this.lblPassWord.Text=model.PassWord;
		this.lblSex.Text=model.Sex?"是":"否";
		this.lblPhoneNum.Text=model.PhoneNum;
		this.lblEmail.Text=model.Email;
		this.lblAge.Text=model.Age.ToString();
		this.lblImgUrl.Text=model.ImgUrl;
		this.lblIsEnable.Text=model.IsEnable?"是":"否";
		this.lblCreateTime.Text=model.CreateTime.ToString();
		this.lblUpdateTime.Text=model.UpdateTime.ToString();

	}


    }
}
