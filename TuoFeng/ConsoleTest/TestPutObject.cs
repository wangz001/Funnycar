using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Aliyun.OSS;
using Aliyun.OSS.Common;

namespace ConsoleTest
{
    public class TestPutObject
    {
        const string accessKeyId = "w358YdbTYPQvTD8j";
        const string accessKeySecret = "7TntYK3LV94OTTVNBid9oFLw8MgNhf";
        const string endpoint = "http://oss-cn-beijing.aliyuncs.com";

        static OssClient client = new OssClient(endpoint, accessKeyId, accessKeySecret);

        const string bucketName = "funnycar";
        const string key = "1215/aaa.jpg";
        const string fileToUpload = @"C:\Users\Administrator\Desktop\image\20071017111345564_2.jpg";

        static AutoResetEvent _event = new AutoResetEvent(false);

        public static void PutObject()
        {
            try
            {
                // 1. put object to specified output stream
                using (var fs = File.Open(fileToUpload, FileMode.Open))
                {
                    var metadata = new ObjectMetadata();
                    //metadata.UserMetadata.Add("mykey1", "myval1");
                    //metadata.UserMetadata.Add("mykey2", "myval2");
                    metadata.CacheControl = "No-Cache";
                    metadata.ContentType = "image/jpeg";
                    metadata.ContentLength = fs.Length;
                    client.PutObject(bucketName, key, fs);
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

        public static void AsyncPutObject()
        {
            try
            {
                // 1. put object to specified output stream
                using (var fs = File.Open(fileToUpload, FileMode.Open))
                {
                    var metadata = new ObjectMetadata();
                    metadata.UserMetadata.Add("mykey1", "myval1");
                    metadata.UserMetadata.Add("mykey2", "myval2");
                    metadata.CacheControl = "No-Cache";
                    metadata.ContentType = "text/html";
                    client.BeginPutObject(bucketName, key, fs, metadata, PutObjectCallback, new string('a', 8));

                    _event.WaitOne();
                }
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

        private static void PutObjectCallback(IAsyncResult ar)
        {
            try
            {
                var result = client.EndPutObject(ar);
                Console.WriteLine(result.ETag);
                Console.WriteLine(ar.AsyncState as string);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _event.Set();
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
    }
}
