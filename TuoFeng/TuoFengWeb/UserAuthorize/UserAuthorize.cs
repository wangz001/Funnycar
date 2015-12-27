//using System;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Bita.CarData.AdminWeb.Helpers;
//using Bita.CarData.DomainEntity;
//using Bita.CarData.IEntityRepository;

//namespace Bita.CarData.AdminWeb.UserAuthorize
//{
//    /// <summary>表示一个特性，该特性用于限制用户访问权限</summary>
//    public class UserAuthorizeAttribute : AuthorizeAttribute
//    {

//        public override void OnAuthorization(AuthorizationContext filterContext)
//        {
//            var user = AccountSession.Account;
//            if (user == null)
//            {
//                filterContext.Result = new RedirectResult("/Home/Login");
//            }
//            else
//            {
//                base.OnAuthorization(filterContext);
//                if (filterContext.HttpContext.Response.StatusCode == 403)
//                {
//                    filterContext.Result = new RedirectResult("/Home/NotAuthorized");
//                }    
//            }
            
//        }

//        protected override bool AuthorizeCore(HttpContextBase httpContext)
//        {
//            //ViewContext
//            var controller = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
//            var action =  httpContext.Request.RequestContext.RouteData.Values["action"].ToString();
//            var isAllowed = IsAllowed(AccountSession.Account, controller, action);
//            if (!isAllowed)
//            {
//                var wrapper = new HttpRequestWrapper(HttpContext.Current.Request);
//                if (wrapper.IsAjaxRequest())
//                {
//                    HttpContext.Current.Response.Write("NotAuthorized");
//                    HttpContext.Current.Response.StatusCode = 403;
//                    HttpContext.Current.Response.End();
//                }
//                else
//                {
//                    HttpContext.Current.Response.StatusCode = 403;
//                }
//            }
//            return isAllowed || base.AuthorizeCore(httpContext);
//        }


//        /// <summary>验证是否有权限</summary>
//        /// <param name="httpContext">请求上下文</param>
//        /// <returns>有权限返回true 否则返回false</returns>
//        protected bool IsAuthorize(HttpContextBase httpContext)
//        {
//            if (httpContext == null)
//            {
//                throw new ArgumentNullException("HttpContext不能为空");
//            }

//            if (httpContext.Session != null)
//            {
//                var user = AccountSession.Account;
//                var controller = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
//                var action = httpContext.Request.RequestContext.RouteData.Values["action"].ToString();
//                return IsAllowed(user, controller, action);
//            }
//            return false;
//        }

//        /// <summary>验证是否有权限</summary>
//        protected bool IsAuthorize(HttpContextBase httpContext, string controller, string action)
//        {
//            if (httpContext.Session != null)
//            {
//                var user = AccountSession.Account;
//                return IsAllowed(user, controller, action);
//            }

//            return false;
//        }

//        /// <summary>执行用户权限授权</summary>
//        /// <param name="httpContext">请求上下文</param>
//        /// <returns>有权限返回true 否则返回false</returns>
//        internal bool PerformAuthorizeCore(HttpContextBase httpContext)
//        {
//            return IsAuthorize(httpContext);
//        }

//        /// <summary>执行用户权限授权</summary>
//        internal bool PerformAuthorizeCore(HttpContextBase httpContext, string controller, string action)
//        {
//            return IsAuthorize(httpContext,controller,action);
//        }



//        /// <summary>验证是否有权限</summary>
//        /// <param name="user">用户</param>
//        /// <param name="controller">controller名称</param>
//        /// <param name="action">action名称</param>
//        /// <returns>有权限返回true 否则返回false</returns>
//        public static bool IsAllowed(User user, string controller, string action)
//        {
//            if (user == null) //这里应该不可能有null的情况了
//            {
//                return false;
//            }
//            var authorityRepository = ComponentFactory.EntityRepository.GetInstance<IAuthorityRepository>();
//            var roleJoinAuthorites = authorityRepository.GetAuthorityByRoleId(user.RoleId).ToList();
//            return roleJoinAuthorites.Where(roleAuthority => !string.IsNullOrEmpty(controller))
//                .Any(r => r.Controller.Trim().Equals(controller.Trim(),StringComparison.InvariantCultureIgnoreCase)
//                          && r.Action.Trim().Equals(action.Trim(), StringComparison.InvariantCultureIgnoreCase));
//        }
//    }
//}
