using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Sts.Model.V20150401;
using Aliyun.OSS;
using Aliyun.OSS.Common;

namespace ConsoleTest
{
    public class TestPutObject
    {
        const string accessKeyId = "5VygCfR832iUb570";
        const string accessKeySecret = "xDiOX2MMIGuzyCUdyQhufT8Kon1ZKA";
        const string endpoint = "http://oss-cn-beijing.aliyuncs.com";
        static OssClient client = new OssClient(endpoint, accessKeyId, accessKeySecret);

        const string bucketName = "funnycar";
        static string key = "1219/aaa.jpg";
        const string fileToUpload = @"C:\Users\Administrator\Desktop\image\20071017111345564_2.jpg";

        static AutoResetEvent _event = new AutoResetEvent(false);

        public static void PutObject()
        {
            key =DateTime.Now.ToShortDateString()+"/aaa.jpg";
            try
            {
                var credentials = GetSecurityToken();
                var ossClient = new OssClient(endpoint, credentials.AccessKeyId, credentials.AccessKeySecret, credentials.SecurityToken);

                // 1. put object to specified output stream
                using (var fs = File.Open(fileToUpload, FileMode.Open))
                {
                    var metadata = new ObjectMetadata();
                    //metadata.UserMetadata.Add("mykey1", "myval1");
                    //metadata.UserMetadata.Add("mykey2", "myval2");
                    metadata.CacheControl = "No-Cache";
                    metadata.ContentType = "image/jpeg";
                    metadata.ContentLength = fs.Length;
                    ossClient.PutObject(bucketName, key, fs);
                }

                 //2. put object to specified file
                //client.PutObject(bucketName, key, fileToUpload);

                // 3. put object from specified object with multi-level virtual directory
                //key = "folder/sub_folder/key0";
                //client.PutObject(bucketName, key, fileToUpload);

            }
            catch (OssException ex)
            {
                Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                    ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed with error info: {0}", ex.Message);
            }
        }

        public static void CreateBucket(string bucketName)
        {
            var created = false;
            try
            {
                client.CreateBucket(bucketName);
                created = true;
                Console.WriteLine("Created bucket name: " + bucketName);
                client.CreateBucket(bucketName);
            }
            catch (OssException ex)
            {
                if (ex.ErrorCode == OssErrorCode.BucketAlreadyExists)
                {
                    Console.WriteLine("Bucket '{0}' already exists, please modify and recreate it.", bucketName);
                }
                else
                {
                    Console.WriteLine("Failed with error info: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                        ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
                }
            }
            finally
            {
                if (created)
                {
                    client.DeleteBucket(bucketName);
                }
            }
        }

        /// <summary>
        /// 获取临时访问权限
        /// </summary>
        /// <returns></returns>
        private static AssumeRoleResponse.Credentials_ GetSecurityToken()
        {
            // 构建一个 Aliyun Client, 用于发起请求
            // 构建Aliyun Client时需要设置AccessKeyId和AccessKeySevcret
            // STS是Global Service, API入口位于杭州, 这里Region填写"cn-hangzhou"
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            DefaultAcsClient client = new DefaultAcsClient(profile);

            // 构造AssumeRole请求
            AssumeRoleRequest request = new AssumeRoleRequest();
            // 指定角色Arn
            request.RoleArn = "acs:ram::1724377057077130:role/myfirstrole";
            request.RoleSessionName = "alice";
            // 可以设置Token有效期，可选参数，默认3600秒；
            //request.DurationSeconds = 3600;
            // 可以设置Token的附加Policy，可以在获取Token时，通过额外设置一个Policy进一步减小Token的权限；
            // request.Policy="<policy-content>"

            try
            {
                AssumeRoleResponse response = client.GetAcsResponse(request);

                Console.WriteLine("AccessKeyId: " + response.Credentials.AccessKeyId);
                Console.WriteLine("AccessKeySecret: " + response.Credentials.AccessKeySecret);
                Console.WriteLine("SecurityToken: " + response.Credentials.SecurityToken);
                //Token过期时间；服务器返回UTC时间，这里转换成北京时间显示；
                Console.WriteLine("Expiration: " + DateTime.Parse(response.Credentials.Expiration).ToLocalTime());
                return response.Credentials;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return null;
        }
    
    }
}
