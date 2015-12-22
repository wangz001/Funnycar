using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Sts.Model.V20150401;

namespace TuoFengWeb.Common
{
    //阿里云oss服务
    public class OssAccessUtil
    {
        //user alice's key and secret
        const string accessKeyId = "5VygCfR832iUb570";
        const string accessKeySecret = "xDiOX2MMIGuzyCUdyQhufT8Kon1ZKA";
        private static AssumeRoleResponse.Credentials_ credentials = null;
        private static DateTime _timeStamp;

        public static AssumeRoleResponse.Credentials_ GetSecurityToken()
        {
            if (credentials!=null&&(DateTime.Now-_timeStamp).Seconds<30)
            {
                //防止请求过于频繁，设置5分钟请求一次。
                return credentials;
            }
            // 构建一个 Aliyun Client, 用于发起请求
            // 构建Aliyun Client时需要设置AccessKeyId和AccessKeySevcret
            // STS是Global Service, API入口位于杭州, 这里Region填写"cn-hangzhou"
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            var client = new DefaultAcsClient(profile);

            // 构造AssumeRole请求
            var request = new AssumeRoleRequest();
            // 指定角色Arn
            request.RoleArn = "acs:ram::1724377057077130:role/myfirstrole";
            request.RoleSessionName = "alice";
            // 可以设置Token有效期，可选参数，默认3600秒；
            request.DurationSeconds = 3600;
            // 可以设置Token的附加Policy，可以在获取Token时，通过额外设置一个Policy进一步减小Token的权限；
            // request.Policy="<policy-content>"
            try
            {
                AssumeRoleResponse response = client.GetAcsResponse(request);
                //Console.WriteLine("AccessKeyId: " + response.Credentials.AccessKeyId);
                //Console.WriteLine("AccessKeySecret: " + response.Credentials.AccessKeySecret);
                //Console.WriteLine("SecurityToken: " + response.Credentials.SecurityToken);
                ////Token过期时间；服务器返回UTC时间，这里转换成北京时间显示；
                //Console.WriteLine("Expiration: " + DateTime.Parse(response.Credentials.Expiration).ToLocalTime());
                credentials = response.Credentials;
                _timeStamp = DateTime.Now;
                return credentials;
            }
            catch (Exception ex)
            {
                return null;
                //记录错误日志
            }
        }
    }
}