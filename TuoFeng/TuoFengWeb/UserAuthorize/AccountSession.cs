using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TuoFeng.BLL;
using TuoFeng.Model;

namespace TuoFengWeb.UserAuthorize
{
    public class AccountSession
    {
        static readonly UserBll UserBll=new UserBll();
        /// <summary>
        /// 登录账号信息
        /// </summary>
        public static User Account
        {
            set
            {
                var carsDataUserIdCookie = value.Id.ToString(CultureInfo.InvariantCulture);
                FormsAuthentication.SetAuthCookie(carsDataUserIdCookie, false);
            }
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated == false)
                {
                    var wrapper = new HttpRequestWrapper(HttpContext.Current.Request);
                    if (wrapper.IsAjaxRequest())
                    {
                        HttpContext.Current.Response.StatusCode = 401;
                        HttpContext.Current.Response.Write("logout");
                        HttpContext.Current.Response.End();
                    }
                    return null;
                }

                var carsDataUserIdCookie = HttpContext.Current.User.Identity.Name;
                var userId = int.Parse(carsDataUserIdCookie);
                var user = UserBll.GetModelByCache(userId);
                return user;
            }
        }
    }
}
