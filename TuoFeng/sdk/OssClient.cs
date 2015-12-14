﻿/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 * 版权所有 （C）阿里云计算有限公司
 */

using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using Aliyun.OSS.Domain;
using Aliyun.OSS.Commands;
using Aliyun.OSS.Util;
using Aliyun.OSS.Common;
using Aliyun.OSS.Properties;
using Aliyun.OSS.Common.Communication;
using Aliyun.OSS.Common.Authentication;
using ExecutionContext = Aliyun.OSS.Common.Communication.ExecutionContext;
using ICredentials = Aliyun.OSS.Common.Authentication.ICredentials;

namespace Aliyun.OSS
{
    /// <summary>
    /// 访问阿里云对象存储服务（Object Storage Service， OSS）的入口类。
    /// </summary>
    public class OssClient : IOss
    {
        #region Fields & Properties

        private volatile Uri _endpoint;
        private readonly ICredentialsProvider _credsProvider;
        private readonly IServiceClient _serviceClient;

        #endregion

        #region Constructors

        /// <summary>
        /// 由用户指定的OSS访问地址、阿里云颁发的Access Key Id/Access Key Secret构造一个新的<see cref="OssClient" />实例。
        /// </summary>
        /// <param name="endpoint">OSS的访问地址。</param>
        /// <param name="accessKeyId">OSS的访问ID。</param>
        /// <param name="accessKeySecret">OSS的访问密钥。</param>
        public OssClient(string endpoint, string accessKeyId, string accessKeySecret)
            : this(endpoint, accessKeyId, accessKeySecret, null) { }

        /// <summary>
        /// 由用户指定的OSS访问地址、STS提供的临时Token信息(Access Key Id/Access Key Secret/Security Token)
        /// 构造一个新的<see cref="OssClient" />实例。
        /// </summary>
        /// <param name="endpoint">OSS的访问地址。</param>
        /// <param name="accessKeyId">STS提供的临时访问ID。</param>
        /// <param name="accessKeySecret">STS提供的访问密钥。</param>
        /// <param name="securityToken">STS提供的安全令牌。</param>
        public OssClient(string endpoint, string accessKeyId, string accessKeySecret, string securityToken)
            : this(new Uri(endpoint.ToLower().StartsWith("http") ? endpoint : "http://" + endpoint),
                   accessKeyId, accessKeySecret, securityToken) { }

        /// <summary>
        /// 由用户指定的OSS访问地址、阿里云颁发的Access Key Id/Access Key Secret构造一个新的<see cref="OssClient" />实例。
        /// </summary>
        /// <param name="endpoint">OSS的访问地址。</param>
        /// <param name="accessKeyId">OSS的访问ID。</param>
        /// <param name="accessKeySecret">OSS的访问密钥。</param>
        public OssClient(Uri endpoint, string accessKeyId, string accessKeySecret)
            : this(endpoint, accessKeyId, accessKeySecret, null, new ClientConfiguration()) { }

        /// <summary>
        /// 由用户指定的OSS访问地址、STS提供的临时Token信息(Access Key Id/Access Key Secret/Security Token)
        /// 构造一个新的<see cref="OssClient" />实例。
        /// </summary>
        /// <param name="endpoint">OSS的访问地址。</param>
        /// <param name="accessKeyId">STS提供的临时访问ID。</param>
        /// <param name="accessKeySecret">STS提供的临时访问密钥。</param>
        /// <param name="securityToken">STS提供的安全令牌。</param>
        public OssClient(Uri endpoint, string accessKeyId, string accessKeySecret, string securityToken)
            : this(endpoint, accessKeyId, accessKeySecret, securityToken, new ClientConfiguration()) { }

        /// <summary>
        /// 由用户指定的OSS访问地址、阿里云颁发的Access Key Id/Access Key Secret、客户端配置
        /// 构造一个新的<see cref="OssClient" />实例。
        /// </summary>
        /// <param name="endpoint">OSS的访问地址。</param>
        /// <param name="accessKeyId">OSS的访问ID。</param>
        /// <param name="accessKeySecret">OSS的访问密钥。</param>
        /// <param name="configuration">客户端配置。</param>
        public OssClient(Uri endpoint, string accessKeyId, string accessKeySecret, ClientConfiguration configuration)
            : this(endpoint, new DefaultCredentialsProvider(new DefaultCredentials(accessKeyId, accessKeySecret, null)), configuration) { }

        /// <summary>
        /// 由用户指定的OSS访问地址、STS提供的临时Token信息(Access Key Id/Access Key Secret/Security Token)、
        /// 客户端配置构造一个新的<see cref="OssClient" />实例。
        /// </summary>
        /// <param name="endpoint">OSS的访问地址。</param>
        /// <param name="accessKeyId">STS提供的临时访问ID。</param>
        /// <param name="accessKeySecret">STS提供的临时访问密钥。</param>
        /// <param name="securityToken">STS提供的安全令牌。</param>
        /// <param name="configuration">客户端配置。</param>
        public OssClient(Uri endpoint, string accessKeyId, string accessKeySecret, string securityToken, ClientConfiguration configuration)
            : this(endpoint, new DefaultCredentialsProvider(new DefaultCredentials(accessKeyId, accessKeySecret, securityToken)), configuration) { }

        /// <summary>
        /// 由用户指定的OSS访问地址、Credentials提供者构造一个新的<see cref="OssClient" />实例。
        /// </summary>
        /// <param name="endpoint">OSS的访问地址。</param>
        /// <param name="credsProvider">Credentials提供者。</param>
        public OssClient(Uri endpoint, ICredentialsProvider credsProvider)
            : this(endpoint, credsProvider, new ClientConfiguration()) { }

        /// <summary>
        /// 由用户指定的OSS访问地址、Credentials提供者、客户端配置构造一个新的<see cref="OssClient" />实例。
        /// </summary>
        /// <param name="endpoint">OSS的访问地址。</param>
        /// <param name="credsProvider">Credentials提供者。</param>
        /// <param name="configuration">客户端配置。</param>
        public OssClient(Uri endpoint, ICredentialsProvider credsProvider, ClientConfiguration configuration)
        {
            if (endpoint == null)
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "endpoint");

            if (!endpoint.ToString().StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !endpoint.ToString().StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException(OssResources.EndpointNotSupportedProtocal, "endpoint");

            if (credsProvider == null)
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "credsProvider");

            _endpoint = endpoint;
            _credsProvider = credsProvider;
            _serviceClient = ServiceClientFactory.CreateServiceClient(configuration ?? new ClientConfiguration());
        }

        #endregion

        #region Switch Credentials & Endpoint

        /// <inheritdoc/>
        public void SwitchCredentials(ICredentials creds)
        {
            if (creds == null)
                throw new ArgumentNullException("creds");
            _credsProvider.SetCredentials(creds);
        }

        /// <inheritdoc/>
        public void SetEndpoint(Uri endpoint)
        {
            _endpoint = endpoint;
        }

        #endregion

        #region Bucket Operations

        /// <inheritdoc/>
        public Bucket CreateBucket(string bucketName)
        {
            var cmd = CreateBucketCommand.Create(_serviceClient, _endpoint,
                                                 CreateContext(HttpMethod.Put, bucketName, null),
                                                 bucketName);
            using(cmd.Execute())
            {
                // Do nothing
            }

            return new Bucket(bucketName);
        }

        /// <inheritdoc/>
        public void DeleteBucket(string bucketName)
        {
            var cmd = DeleteBucketCommand.Create(_serviceClient, _endpoint,
                                                 CreateContext(HttpMethod.Delete, bucketName, null),
                                                 bucketName);
            using(cmd.Execute())
            {
                // Do nothing
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Bucket> ListBuckets()
        {
            var cmd = ListBucketsCommand.Create(_serviceClient, _endpoint,
                                                CreateContext(HttpMethod.Get, null, null), null);
            var result = cmd.Execute();
            return result.Buckets;
        }

        /// <inheritdoc/>
        public ListBucketsResult ListBuckets(ListBucketsRequest listBucketsRequest)
        {
            var cmd = ListBucketsCommand.Create(_serviceClient, _endpoint,
                                                CreateContext(HttpMethod.Get, null, null), listBucketsRequest);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public void SetBucketAcl(string bucketName, CannedAccessControlList acl)
        {
            var setBucketAclRequest = new SetBucketAclRequest(bucketName, acl);
            SetBucketAcl(setBucketAclRequest);
        }

        /// <inheritdoc/>
        public void SetBucketAcl(SetBucketAclRequest setBucketAclRequest)
        {
            ThrowIfNullRequest(setBucketAclRequest);

            var cmd = SetBucketAclCommand.Create(_serviceClient, _endpoint,
                                                CreateContext(HttpMethod.Put, setBucketAclRequest.BucketName, null),
                                                setBucketAclRequest.BucketName, setBucketAclRequest);
            using (cmd.Execute())
            {
                // Do nothing
            }
        }

        /// <inheritdoc/>
        public AccessControlList GetBucketAcl(string bucketName)
        {
            var cmd = GetBucketAclCommand.Create(_serviceClient, _endpoint,
                                                 CreateContext(HttpMethod.Get, bucketName, null),
                                                 bucketName);
            return cmd.Execute();
        }


        /// <inheritdoc/>
        public void SetBucketCors(SetBucketCorsRequest setBucketCorsRequest)
        {
            ThrowIfNullRequest(setBucketCorsRequest);
            var cmd = SetBucketCorsCommand.Create(_serviceClient, _endpoint,
                                                 CreateContext(HttpMethod.Put, setBucketCorsRequest.BucketName, null),
                                                 setBucketCorsRequest.BucketName,
                                                 setBucketCorsRequest);
            using (cmd.Execute())
            {
                // Do nothing
            }
        }

        /// <inheritdoc/>
        public IList<CORSRule> GetBucketCors(string bucketName)
        {
            var cmd = GetBucketCorsCommand.Create(_serviceClient, _endpoint,
                                                 CreateContext(HttpMethod.Get, bucketName, null),
                                                 bucketName);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public void DeleteBucketCors(string bucketName)
        {
            var cmd = DeleteBucketCorsCommand.Create(_serviceClient, _endpoint,
                                                    CreateContext(HttpMethod.Delete, bucketName, null),
                                                    bucketName);
            using (cmd.Execute())
            {
                // Do nothing
            }
        }
        
        /// <inheritdoc/>
        public void SetBucketLogging(SetBucketLoggingRequest setBucketLoggingRequest)
        {
            ThrowIfNullRequest(setBucketLoggingRequest);
            var cmd = SetBucketLoggingCommand.Create(_serviceClient, _endpoint,
                                                    CreateContext(HttpMethod.Put, setBucketLoggingRequest.BucketName, null),
                                                    setBucketLoggingRequest.BucketName,
                                                    setBucketLoggingRequest);
            using (cmd.Execute())
            {
                // Do nothing
            }
        }

        /// <inheritdoc/>
        public BucketLoggingResult GetBucketLogging(string bucketName)
        {
            var cmd = GetBucketLoggingCommand.Create(_serviceClient, _endpoint,
                                                    CreateContext(HttpMethod.Get, bucketName, null),
                                                    bucketName);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public void DeleteBucketLogging(string bucketName)
        {
            var cmd = DeleteBucketLoggingCommand.Create(_serviceClient, _endpoint,
                                                        CreateContext(HttpMethod.Delete, bucketName, null),
                                                        bucketName);
            using (cmd.Execute())
            {
                // Do nothing
            }
        }

        /// <inheritdoc/>
        public void SetBucketWebsite(SetBucketWebsiteRequest setBucketWebSiteRequest)
        {
            ThrowIfNullRequest(setBucketWebSiteRequest);
            var cmd = SetBucketWebsiteCommand.Create(_serviceClient, _endpoint,
                                                     CreateContext(HttpMethod.Put, setBucketWebSiteRequest.BucketName, null),
                                                     setBucketWebSiteRequest.BucketName,
                                                     setBucketWebSiteRequest);
            using (cmd.Execute())
            {
                // Do nothing
            }
        }

        /// <inheritdoc/>
        public BucketWebsiteResult GetBucketWebsite(string bucketName)
        {
            var cmd = GetBucketWebsiteCommand.Create(_serviceClient, _endpoint,
                                                    CreateContext(HttpMethod.Get, bucketName, null),
                                                    bucketName);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public void DeleteBucketWebsite(string bucketName)
        {
            var cmd = DeleteBucketWebsiteCommand.Create(_serviceClient,  _endpoint,
                                                       CreateContext(HttpMethod.Delete, bucketName, null),
                                                       bucketName);
            using (cmd.Execute())
            {
                // Do nothing
            }
        }


        /// <inheritdoc/>
        public void SetBucketReferer(SetBucketRefererRequest setBucketRefererRequest)
        {
            ThrowIfNullRequest(setBucketRefererRequest);
            var cmd = SetBucketRefererCommand.Create(_serviceClient, _endpoint,
                                                    CreateContext(HttpMethod.Put, setBucketRefererRequest.BucketName, null),
                                                    setBucketRefererRequest.BucketName,
                                                    setBucketRefererRequest);
            using (cmd.Execute())
            {
                // Do nothing
            }
        }

        /// <inheritdoc/>
        public RefererConfiguration GetBucketReferer(string bucketName)
        {
            var cmd = GetBucketRefererCommand.Create(_serviceClient, _endpoint,
                                                     CreateContext(HttpMethod.Get, bucketName, null),
                                                     bucketName);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public void SetBucketLifecycle(SetBucketLifecycleRequest setBucketLifecycleRequest)
        {
            ThrowIfNullRequest(setBucketLifecycleRequest);

            var cmd = SetBucketLifecycleCommand.Create(_serviceClient, _endpoint,
                                                      CreateContext(HttpMethod.Put, setBucketLifecycleRequest.BucketName, null),
                                                      setBucketLifecycleRequest.BucketName,
                                                      setBucketLifecycleRequest);
            using (cmd.Execute())
            {
                // Do nothing
            }
        }

        /// <inheritdoc/>
        public IList<LifecycleRule> GetBucketLifecycle(string bucketName)
        {
            var cmd = GetBucketLifecycleCommand.Create(_serviceClient, _endpoint,
                                                      CreateContext(HttpMethod.Get, bucketName, null),
                                                      bucketName);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public bool DoesBucketExist(string bucketName)
        {
            if (string.IsNullOrEmpty(bucketName))
                throw new ArgumentException(Resources.ExceptionIfArgumentStringIsNullOrEmpty, "bucketName");
            if (!OssUtils.IsBucketNameValid(bucketName))
                throw new ArgumentException(OssResources.BucketNameInvalid, "bucketName");

            try
            {
                GetBucketAcl(bucketName);
            }
            catch (OssException e)
            {
                if (e.ErrorCode.Equals(OssErrorCode.NoSuchBucket))
                    return false;
            }

            return true;
        }

        #endregion

        #region Object Operations

        /// <inheritdoc/>
        public ObjectListing ListObjects(string bucketName)
        {
            return ListObjects(bucketName, null);
        }

        /// <inheritdoc/>
        public IAsyncResult BeginListObjects(string bucketName, AsyncCallback callback, object state)
        {
            return BeginListObjects(bucketName, null, callback, state);
        }

        /// <inheritdoc/>
        public ObjectListing ListObjects(string bucketName, string prefix)
        {
            var listObjectsRequest = new ListObjectsRequest(bucketName)
            {
                Prefix = prefix
            };
            return ListObjects(listObjectsRequest);
        }

        /// <inheritdoc/>
        public IAsyncResult BeginListObjects(string bucketName, string prefix, AsyncCallback callback, object state)
        {
            var listObjectsRequest = new ListObjectsRequest(bucketName)
            {
                Prefix = prefix
            };
            return BeginListObjects(listObjectsRequest, callback, state);
        }

        /// <inheritdoc/>
        public ObjectListing ListObjects(ListObjectsRequest listObjectsRequest)
        {
            ThrowIfNullRequest(listObjectsRequest);
            var cmd = ListObjectsCommand.Create(_serviceClient, _endpoint,
                                               CreateContext(HttpMethod.Get, listObjectsRequest.BucketName, null),
                                               listObjectsRequest);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public IAsyncResult BeginListObjects(ListObjectsRequest listObjectsRequest, AsyncCallback callback, object state)
        {
            if (listObjectsRequest == null)
                throw new ArgumentNullException("listObjectsRequest");

            var cmd = ListObjectsCommand.Create(_serviceClient, _endpoint,
                                               CreateContext(HttpMethod.Get, listObjectsRequest.BucketName, null),
                                               listObjectsRequest);
            return OssUtils.BeginOperationHelper(cmd, callback, state);
        }

        /// <inheritdoc/>
        public ObjectListing EndListObjects(IAsyncResult asyncResult)
        {
            return OssUtils.EndOperationHelper<ObjectListing>(_serviceClient, asyncResult);
        }


        /// <inheritdoc/>
        public PutObjectResult PutObject(string bucketName, string key, Stream content)
        {
            return PutObject(bucketName, key, content, null);
        }

        /// <inheritdoc/>
        public IAsyncResult BeginPutObject(string bucketName, string key, Stream content, AsyncCallback callback, object state)
        {
            return BeginPutObject(bucketName, key, content, null, callback, state);
        }

        /// <inheritdoc/>
        public PutObjectResult PutObject(string bucketName, string key, Stream content, ObjectMetadata metadata)
        {
            metadata = metadata ?? new ObjectMetadata();
            SetContentTypeIfNull(key, null, ref metadata);

            var cmd = PutObjectCommand.Create(_serviceClient, _endpoint,
                                             CreateContext(HttpMethod.Put, bucketName, key),
                                             bucketName, key, content, metadata);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public IAsyncResult BeginPutObject(string bucketName, string key, Stream content, ObjectMetadata metadata,
            AsyncCallback callback, object state)
        {
            metadata = metadata ?? new ObjectMetadata();
            SetContentTypeIfNull(key, null, ref metadata);

            var cmd = PutObjectCommand.Create(_serviceClient, _endpoint,
                                             CreateContext(HttpMethod.Put, bucketName, key),
                                             bucketName, key, content, metadata);
            return OssUtils.BeginOperationHelper(cmd, callback, state);
        }

        /// <inheritdoc/>
        public PutObjectResult PutObject(string bucketName, string key, string fileToUpload)
        {
            return PutObject(bucketName, key, fileToUpload, null);
        }

        /// <inheritdoc/>
        public IAsyncResult BeginPutObject(string bucketName, string key, string fileToUpload, AsyncCallback callback, object state)
        {
            return BeginPutObject(bucketName, key, fileToUpload, null, callback, state);
        }

        /// <inheritdoc/>
        public PutObjectResult PutObject(string bucketName, string key, string fileToUpload, ObjectMetadata metadata)
        {
            if (!File.Exists(fileToUpload) || Directory.Exists(fileToUpload))
                throw new ArgumentException(String.Format("Invalid file path {0}.", fileToUpload));

            metadata = metadata ?? new ObjectMetadata();
            SetContentTypeIfNull(key, fileToUpload, ref metadata);

            PutObjectResult result;
            using (Stream content = File.OpenRead(fileToUpload))
            {
                result = PutObject(bucketName, key, content, metadata);
            }
            return result;
        }

        /// <inheritdoc/>
        public IAsyncResult BeginPutObject(string bucketName, string key, string fileToUpload, ObjectMetadata metadata,
            AsyncCallback callback, object state)
        {
            if (!File.Exists(fileToUpload) || Directory.Exists(fileToUpload))
                throw new ArgumentException(String.Format("Invalid file path {0}.", fileToUpload));

            metadata = metadata ?? new ObjectMetadata();
            SetContentTypeIfNull(key, fileToUpload, ref metadata);

            IAsyncResult result;
            using (Stream content = File.OpenRead(fileToUpload))
            {
                result = BeginPutObject(bucketName, key, content, metadata, callback, state);
            }
            return result;
        }

        /// <inheritdoc/>
        public PutObjectResult EndPutObject(IAsyncResult asyncResult)
        {
            return OssUtils.EndOperationHelper<PutObjectResult>(_serviceClient, asyncResult);
        }


        /// <inheritdoc/>
        public PutObjectResult PutObject(Uri signedUrl, string fileToUpload)
        {
            if (!File.Exists(fileToUpload) || Directory.Exists(fileToUpload))
                throw new ArgumentException(String.Format("Invalid file path {0}.", fileToUpload));

            var metadata = new ObjectMetadata();
            SetContentTypeIfNull(null, fileToUpload, ref metadata);

            PutObjectResult result;
            using (Stream content = File.OpenRead(fileToUpload))
            {
                result = PutObject(signedUrl, content, metadata);
            }
            return result;
        }

        /// <inheritdoc/>
        public PutObjectResult PutObject(Uri signedUrl, Stream content, ObjectMetadata metadata = null)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(signedUrl);
            webRequest.Timeout = Timeout.Infinite;  // A temporary solution. 
            webRequest.Method = HttpMethod.Put.ToString().ToUpperInvariant();
            webRequest.ContentLength = content.Length;

            if (metadata != null && metadata.ContentType != null) {
                webRequest.ContentType = metadata.ContentType;
            }
           
            using (var requestStream = webRequest.GetRequestStream())
            {
                IoUtils.WriteTo(content, requestStream);
            }

            var response = webRequest.GetResponse() as HttpWebResponse;
            PutObjectResult result = null;
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                result = new PutObjectResult { ETag = response.Headers[HttpHeaders.ETag] };
            }
            return result;
        }

        /// <inheritdoc/>
        public PutObjectResult PutBigObject(string bucketName, string key, string fileToUpload, ObjectMetadata metadata, long? partSize = null)
        {
            if (!File.Exists(fileToUpload) || Directory.Exists(fileToUpload))
                throw new ArgumentException(String.Format("Invalid file path {0}.", fileToUpload));

            // 计算content-type
            metadata = metadata ?? new ObjectMetadata();
            SetContentTypeIfNull(key, fileToUpload, ref metadata);

            using (var fs = File.Open(fileToUpload, FileMode.Open))
            {
                return PutBigObject(bucketName, key, fs, metadata, partSize);
            }
        }

        /// <inheritdoc/>
        public PutObjectResult PutBigObject(string bucketName, string key, Stream content, ObjectMetadata metadata, long? partSize = null)
        {
            // 计算content-type
            metadata = metadata ?? new ObjectMetadata();
            SetContentTypeIfNull(key, null, ref metadata);

            // 调整实际的part size
            long actualPartSize = AdjustPartSize(partSize);

            // 如果上传文件小于分片大小，直接上传即可
            if (content.Length <= actualPartSize)
            {
                return PutObject(bucketName, key, content, metadata);
            }

            // 初始化分片上传
            var initRequest = new InitiateMultipartUploadRequest(bucketName, key, metadata);
            var initResult = InitiateMultipartUpload(initRequest);
            var uploadId = initResult.UploadId;

            // 上传分片
            List<PartETag> partETags = DoUploadMultiPart(bucketName, key, content, actualPartSize, uploadId);

            // 完成上传
            var completeRequest = new CompleteMultipartUploadRequest(bucketName, key, uploadId);
            foreach (var partETag in partETags)
            {
                completeRequest.PartETags.Add(partETag);
            }
            var result = CompleteMultipartUpload(completeRequest);
            return new PutObjectResult() { ETag = result.ETag };
        }

        /// <inheritdoc/>
        public OssObject GetObject(Uri signedUrl)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(signedUrl);
            webRequest.Timeout = Timeout.Infinite;  // A temporary solution. 
            webRequest.Method = HttpMethod.Get.ToString().ToUpperInvariant();

            var response = webRequest.GetResponse() as HttpWebResponse;
            OssObject result = null;
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                result = new OssObject();
                var ms = new MemoryStream();
                IoUtils.WriteTo(response.GetResponseStream(), ms);
                ms.Seek(0, SeekOrigin.Begin);
                result.Content = ms;
            }
            return result;
        }

        /// <inheritdoc/>
        public OssObject GetObject(string bucketName, string key)
        {
            return GetObject(new GetObjectRequest(bucketName, key));
        }

        /// <inheritdoc/>
        public IAsyncResult BeginGetObject(string bucketName, string key, AsyncCallback callback, Object state)
        {
            GetObjectRequest getObjectRequest = new GetObjectRequest(bucketName, key);
            return BeginGetObject(getObjectRequest, callback, state);
        }

        /// <inheritdoc/>
        public OssObject GetObject(GetObjectRequest getObjectRequest)
        {
            ThrowIfNullRequest(getObjectRequest);

            var cmd = GetObjectCommand.Create(_serviceClient, _endpoint,
                                             CreateContext(HttpMethod.Get, getObjectRequest.BucketName, getObjectRequest.Key),
                                             getObjectRequest);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public IAsyncResult BeginGetObject(GetObjectRequest getObjectRequest, AsyncCallback callback, Object state)
        {
            ThrowIfNullRequest(getObjectRequest);

            var cmd = GetObjectCommand.Create(_serviceClient, _endpoint,
                                             CreateContext(HttpMethod.Get, getObjectRequest.BucketName, getObjectRequest.Key),
                                             getObjectRequest);
            return OssUtils.BeginOperationHelper(cmd, callback, state);
        }

        /// <inheritdoc/>
        public OssObject EndGetObject(IAsyncResult asyncResult)
        {
            return OssUtils.EndOperationHelper<OssObject>(_serviceClient, asyncResult);
        }


        /// <inheritdoc/>
        public ObjectMetadata GetObject(GetObjectRequest getObjectRequest, Stream output)
        {
            var ossObject = GetObject(getObjectRequest);
            using(ossObject.Content)
            {
                IoUtils.WriteTo(ossObject.Content, output);
            }
            return ossObject.Metadata;
        }

        /// <inheritdoc/>
        public ObjectMetadata GetObjectMetadata(string bucketName, string key)
        {
            var cmd = GetObjectMetadataCommand.Create(_serviceClient, _endpoint,
                                                     CreateContext(HttpMethod.Head, bucketName, key),
                                                     bucketName, key);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public void DeleteObject(string bucketName, string key)
        {
            var cmd = DeleteObjectCommand.Create(_serviceClient, _endpoint,
                                                CreateContext(HttpMethod.Delete, bucketName, key),
                                                bucketName, key);
            cmd.Execute();
 
        }

        /// <inheritdoc/>
        public DeleteObjectsResult DeleteObjects(DeleteObjectsRequest deleteObjectsRequest)
        {
            ThrowIfNullRequest(deleteObjectsRequest);

            var cmd = DeleteObjectsCommand.Create(_serviceClient, _endpoint,
                                                 CreateContext(HttpMethod.Post, deleteObjectsRequest.BucketName, null),
                                                 deleteObjectsRequest);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public CopyObjectResult CopyObject(CopyObjectRequest copyObjectRequst)
        {
            ThrowIfNullRequest(copyObjectRequst);
            var cmd = CopyObjectCommand.Create(_serviceClient, _endpoint,
                                               CreateContext(HttpMethod.Put, copyObjectRequst.DestinationBucketName, copyObjectRequst.DestinationKey),
                                               copyObjectRequst);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public IAsyncResult BeginCopyObject(CopyObjectRequest copyObjectRequst, AsyncCallback callback, Object state)
        {
            ThrowIfNullRequest(copyObjectRequst);

            var cmd = CopyObjectCommand.Create(_serviceClient, _endpoint,
                                              CreateContext(HttpMethod.Put, copyObjectRequst.DestinationBucketName, copyObjectRequst.DestinationKey),
                                              copyObjectRequst);
            return OssUtils.BeginOperationHelper(cmd, callback, state);
        }

        /// <inheritdoc/>
        public CopyObjectResult EndCopyResult(IAsyncResult asyncResult)
        {
            return OssUtils.EndOperationHelper<CopyObjectResult>(_serviceClient, asyncResult);
        }

        /// <inheritdoc/>
        public CopyObjectResult CopyBigObject(CopyObjectRequest copyObjectRequest, long? partSize = null)
        {
            ThrowIfNullRequest(copyObjectRequest);

            // 调整实际的part size
            long actualPartSize = AdjustPartSize(partSize);

            // 获取拷贝文件大小
            var objectMeta = GetObjectMetadata(copyObjectRequest.SourceBucketName, copyObjectRequest.SourceKey);
            var fileSize = objectMeta.ContentLength;

            if (fileSize <= actualPartSize)
            {
                return CopyObject(copyObjectRequest);
            }

            // 初始化分片拷贝
            var initRequest = new InitiateMultipartUploadRequest(copyObjectRequest.DestinationBucketName,
                                                                 copyObjectRequest.DestinationKey);
            var initResult = InitiateMultipartUpload(initRequest);
            var uploadId = initResult.UploadId;

            // 分片拷贝
            List<PartETag> partETags = DoCopyMultiPart(copyObjectRequest, fileSize, actualPartSize, uploadId);

            // 完成拷贝
            var completeRequest = new CompleteMultipartUploadRequest(copyObjectRequest.DestinationBucketName,
                                                                     copyObjectRequest.DestinationKey, uploadId);
            foreach (var partETag in partETags)
            {
                completeRequest.PartETags.Add(partETag);
            }
            var result = CompleteMultipartUpload(completeRequest);

            // 获取目标文件上次修改时间
            objectMeta = GetObjectMetadata(copyObjectRequest.DestinationBucketName, copyObjectRequest.DestinationKey);
            return new CopyObjectResult() { ETag = result.ETag, LastModified = objectMeta.LastModified };
        }

        /// <inheritdoc/>
        public void ModifyObjectMeta(string bucketName, string key, ObjectMetadata newMeta)
        {
            var copyObjectRequest = new CopyObjectRequest(bucketName, key, bucketName, key)
            {
                NewObjectMetadata = newMeta
            };

            CopyBigObject(copyObjectRequest);
        }

        /// <inheritdoc/>
        public bool DoesObjectExist(string bucketName, string key)
        {
            try
            {
                var cmd = HeadObjectCommand.Create(_serviceClient, _endpoint,
                                                  CreateContext(HttpMethod.Head, bucketName, key),
                                                  bucketName, key);

                using (cmd.Execute())
                {
                    // Do nothing
                }
            }
            catch (OssException e)
            {
                if (e.ErrorCode == OssErrorCode.NoSuchBucket ||
                    e.ErrorCode == OssErrorCode.NoSuchKey)
                {
                    return false;
                }

                // Rethrow
                throw;
            }
            catch (WebException ex)
            {
                HttpWebResponse errorResponse = ex.Response as HttpWebResponse;
                if (errorResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }

                // Rethrow
                throw;
            }
            return true;
        }

        #endregion
        
        #region Generate URL
        
        /// <inheritdoc/>        
        public Uri GeneratePresignedUri(string bucketName, string key)
        {
            var request = new GeneratePresignedUriRequest(bucketName, key, SignHttpMethod.Get);
            return GeneratePresignedUri(request);
        }

        /// <inheritdoc/> 
        public Uri GeneratePresignedUri(string bucketName, string key, DateTime expiration)
        {
            var request = new GeneratePresignedUriRequest(bucketName, key, SignHttpMethod.Get)
            {
                Expiration = expiration
            };
            return GeneratePresignedUri(request);
        }
        
        /// <inheritdoc/>        
        public Uri GeneratePresignedUri(string bucketName, string key, SignHttpMethod method)
        {
            var request = new GeneratePresignedUriRequest(bucketName, key, method);
            return GeneratePresignedUri(request);            
        }

        /// <inheritdoc/>  
        public Uri GeneratePresignedUri(string bucketName, string key, DateTime expiration, 
            SignHttpMethod method)
        {
            var request = new GeneratePresignedUriRequest(bucketName, key, method)
            {
                Expiration = expiration
            };
            return GeneratePresignedUri(request);
        }
        
        /// <inheritdoc/>        
        public Uri GeneratePresignedUri(GeneratePresignedUriRequest generatePresignedUriRequest)
        {
            ThrowIfNullRequest(generatePresignedUriRequest);

            var creds = _credsProvider.GetCredentials();
            var accessKeyId = creds.AccessKeyId;
            var accessKeySecret = creds.AccessKeySecret;
            var securityToken = creds.SecurityToken;
            var useToken = creds.UseToken;
            var bucketName = generatePresignedUriRequest.BucketName;
            var key = generatePresignedUriRequest.Key;
            
            const long ticksOf1970 = 621355968000000000;
            var expires = ((generatePresignedUriRequest.Expiration.ToUniversalTime().Ticks - ticksOf1970) / 10000000L)
                .ToString(CultureInfo.InvariantCulture);
            var resourcePath = OssUtils.MakeResourcePath(_endpoint, bucketName, key);
                
            var request = new ServiceRequest();
            var conf = OssUtils.GetClientConfiguration(_serviceClient);
            request.Endpoint = OssUtils.MakeBucketEndpoint(_endpoint, bucketName, conf);
            request.ResourcePath = resourcePath;

            switch (generatePresignedUriRequest.Method)
            {
                case SignHttpMethod.Get:
                    request.Method = HttpMethod.Get;
                    break;
                case SignHttpMethod.Put:
                    request.Method = HttpMethod.Put;
                    break;
                default:
                    throw new ArgumentException("Unsupported http method.");
            }
            
            request.Headers.Add(HttpHeaders.Date, expires);
            if (!string.IsNullOrEmpty(generatePresignedUriRequest.ContentType))
                request.Headers.Add(HttpHeaders.ContentType, generatePresignedUriRequest.ContentType);
            if (!string.IsNullOrEmpty(generatePresignedUriRequest.ContentMd5))
                request.Headers.Add(HttpHeaders.ContentMd5, generatePresignedUriRequest.ContentMd5);

            foreach (var pair in generatePresignedUriRequest.UserMetadata)
                request.Headers.Add(OssHeaders.OssUserMetaPrefix + pair.Key, pair.Value);
            
            if (generatePresignedUriRequest.ResponseHeaders != null)
                generatePresignedUriRequest.ResponseHeaders.Populate(request.Parameters);

            foreach (var param in generatePresignedUriRequest.QueryParams)
                request.Parameters.Add(param.Key, param.Value);

            if (useToken)
                request.Parameters.Add(RequestParameters.SECURITY_TOKEN, securityToken);
            
            var canonicalResource = "/" + (bucketName ?? "") + ((key != null ? "/" + key : ""));
            var httpMethod = generatePresignedUriRequest.Method.ToString().ToUpperInvariant();
            
            var canonicalString =
                SignUtils.BuildCanonicalString(httpMethod, canonicalResource, request/*, expires*/);
            var signature = ServiceSignature.Create().ComputeSignature(accessKeySecret, canonicalString);

            IDictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams.Add(RequestParameters.EXPIRES, expires);
            queryParams.Add(RequestParameters.OSS_ACCESS_KEY_ID, accessKeyId);
            queryParams.Add(RequestParameters.SIGNATURE, signature);
            foreach (var param in request.Parameters)
                queryParams.Add(param.Key, param.Value);

            var queryString = HttpUtils.ConbineQueryString(queryParams);
            var uriString = request.Endpoint.ToString();
            if (!uriString.EndsWith("/"))
                uriString += "/";
            uriString += resourcePath + "?" + queryString;
            
            return new Uri(uriString);
        }
        
        #endregion

        #region Generate Post Policy

        /// <inheritdoc/>
        public string GeneratePostPolicy(DateTime expiration, PolicyConditions conds)
        {
            if (conds == null)
            {
                throw new ArgumentNullException("conds");
            }

            var formatedExpiration = DateUtils.FormatIso8601Date(expiration);
            var jsonizedExpiration = string.Format("\"expiration\":\"{0}\"", formatedExpiration);
            var jsonizedConds = conds.Jsonize();
            return String.Format("{{{0},{1}}}", jsonizedExpiration, jsonizedConds);
        }

        #endregion

        #region Multipart Operations
        /// <inheritdoc/>
        public MultipartUploadListing ListMultipartUploads(ListMultipartUploadsRequest listMultipartUploadsRequest)
        {
            ThrowIfNullRequest(listMultipartUploadsRequest);
            var cmd = ListMultipartUploadsCommand.Create(_serviceClient, _endpoint,
                                                        CreateContext(HttpMethod.Get, listMultipartUploadsRequest.BucketName, null),
                                                        listMultipartUploadsRequest);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public InitiateMultipartUploadResult InitiateMultipartUpload(InitiateMultipartUploadRequest initiateMultipartUploadRequest)
        {
            ThrowIfNullRequest(initiateMultipartUploadRequest);
            var cmd = InitiateMultipartUploadCommand.Create(_serviceClient, _endpoint,
                                                           CreateContext(HttpMethod.Post, initiateMultipartUploadRequest.BucketName, initiateMultipartUploadRequest.Key),
                                                           initiateMultipartUploadRequest);
            return cmd.Execute();
        }
        
        /// <inheritdoc/>
        public void AbortMultipartUpload(AbortMultipartUploadRequest abortMultipartUploadRequest)
        {
            ThrowIfNullRequest(abortMultipartUploadRequest);
            var cmd = AbortMultipartUploadCommand.Create(_serviceClient, _endpoint,
                                                        CreateContext(HttpMethod.Delete, abortMultipartUploadRequest.BucketName, abortMultipartUploadRequest.Key),
                                                        abortMultipartUploadRequest);
            using(cmd.Execute())
            {
                // Do nothing
            }
        }

        /// <inheritdoc/>        
        public UploadPartResult UploadPart(UploadPartRequest uploadPartRequest)
        {
            ThrowIfNullRequest(uploadPartRequest);
            var cmd = UploadPartCommand.Create(_serviceClient, _endpoint,
                                              CreateContext(HttpMethod.Put, uploadPartRequest.BucketName, uploadPartRequest.Key),
                                              uploadPartRequest);
            return cmd.Execute();
        }

        /// <inheritdoc/>        
        public IAsyncResult BeginUploadPart(UploadPartRequest uploadPartRequest, AsyncCallback callback, object state)
        {
            ThrowIfNullRequest(uploadPartRequest);

            var cmd = UploadPartCommand.Create(_serviceClient, _endpoint,
                                              CreateContext(HttpMethod.Put, uploadPartRequest.BucketName, uploadPartRequest.Key),
                                              uploadPartRequest);
            return OssUtils.BeginOperationHelper(cmd, callback, state);
        }

        /// <inheritdoc/>        
        public UploadPartResult EndUploadPart(IAsyncResult asyncResult)
        {
            return OssUtils.EndOperationHelper<UploadPartResult>(_serviceClient, asyncResult);
        }


        /// <inheritdoc/>
        public UploadPartCopyResult UploadPartCopy(UploadPartCopyRequest uploadPartCopyRequest)
        {
            ThrowIfNullRequest(uploadPartCopyRequest);
            var cmd = UploadPartCopyCommand.Create(_serviceClient, _endpoint,
                                                  CreateContext(HttpMethod.Put, uploadPartCopyRequest.TargetBucket, uploadPartCopyRequest.TargetKey),
                                                  uploadPartCopyRequest);
            return cmd.Execute();
        }

        /// <inheritdoc/>
        public IAsyncResult BeginUploadPartCopy(UploadPartCopyRequest uploadPartCopyRequest,
            AsyncCallback callback, object state)
        {
            ThrowIfNullRequest(uploadPartCopyRequest);

            var cmd = UploadPartCopyCommand.Create(_serviceClient, _endpoint,
                                                  CreateContext(HttpMethod.Put, uploadPartCopyRequest.TargetBucket, uploadPartCopyRequest.TargetKey),
                                                  uploadPartCopyRequest);
            return OssUtils.BeginOperationHelper(cmd, callback, state);
        }

        /// <inheritdoc/>
        public UploadPartCopyResult EndUploadPartCopy(IAsyncResult asyncResult)
        {
            return OssUtils.EndOperationHelper<UploadPartCopyResult>(_serviceClient, asyncResult);
        }


        /// <inheritdoc/>                
        public PartListing ListParts(ListPartsRequest listPartsRequest)
        {
            ThrowIfNullRequest(listPartsRequest);
            var cmd = ListPartsCommand.Create(_serviceClient, _endpoint,
                                             CreateContext(HttpMethod.Get, listPartsRequest.BucketName, listPartsRequest.Key),
                                             listPartsRequest);   
            return cmd.Execute();            
        }
        
        /// <inheritdoc/>                
        public CompleteMultipartUploadResult CompleteMultipartUpload(CompleteMultipartUploadRequest completeMultipartUploadRequest)
        {
            ThrowIfNullRequest(completeMultipartUploadRequest);
            var cmd = CompleteMultipartUploadCommand.Create(_serviceClient, _endpoint,
                                                           CreateContext(HttpMethod.Post, completeMultipartUploadRequest.BucketName, completeMultipartUploadRequest.Key),
                                                           completeMultipartUploadRequest);
            return cmd.Execute();
        }
        
        #endregion

        #region Private Methods
        private ExecutionContext CreateContext(HttpMethod method, string bucket, string key)
        {
            var builder = new ExecutionContextBuilder
            {
                Bucket = bucket,
                Key = key,
                Method = method,
                Credentials = _credsProvider.GetCredentials()
            };
            builder.ResponseHandlers.Add(new ErrorResponseHandler());
            return builder.Build();
        }

        private void ThrowIfNullRequest<TRequestType> (TRequestType request)
        {
            if (request == null)
                throw new ArgumentNullException("request");
        }

        private static void SetContentTypeIfNull(string key, string fileName, ref ObjectMetadata metadata)
        {
            if (metadata.ContentType == null)
            {
                metadata.ContentType = HttpUtils.GetContentType(key, fileName);
            }
        }

        private static long AdjustPartSize(long? partSize)
        {
            var actualPartSize = partSize ?? OssUtils.DefaultPartSize;
            actualPartSize = actualPartSize < OssUtils.PartSizeLowerLimit ? OssUtils.PartSizeLowerLimit : actualPartSize;
            return actualPartSize;
        }

        private List<PartETag> DoUploadMultiPart(string bucketName, string key, Stream content, long partSize, string uploadId)
        {
            var fileSize = content.Length;
            var partCount = fileSize / partSize;
            if (fileSize % partSize != 0)
            {
                partCount++;
            }

            if (partCount >= OssUtils.PartNumberUpperLimit)
            {
                partCount = OssUtils.PartNumberUpperLimit;
                partSize = fileSize / partCount + 1;
            }

            // 分片上传 
            var partETags = new List<PartETag>();
            using (var fs = content)
            {
                for (var i = 0; i < partCount; i++)
                {
                    var skipBytes = partSize * i;
                    fs.Seek(skipBytes, 0);
                    var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
                    var request = new UploadPartRequest(bucketName, key, uploadId)
                    {
                        InputStream = fs,
                        PartSize = size,
                        PartNumber = i + 1
                    };

                    var partResult = UploadPart(request);
                    partETags.Add(partResult.PartETag);
                }
            }

            return partETags;
        }

        private List<PartETag> DoCopyMultiPart(CopyObjectRequest copyObjectRequest, long fileSize, long partSize, string uploadId)
        {
            // 计算分片个数
            var partCount = fileSize / partSize;
            if (fileSize % partSize != 0)
            {
                partCount++;
            }

            if (partCount >= OssUtils.PartNumberUpperLimit)
            {
                partCount = OssUtils.PartNumberUpperLimit;
                partSize = fileSize / partCount + 1;
            }

            // 分片拷贝
            var partETags = new List<PartETag>();
            for (var i = 0; i < partCount; i++)
            {
                var skipBytes = partSize * i;
                var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
                var request = new UploadPartCopyRequest(copyObjectRequest.DestinationBucketName,
                                                        copyObjectRequest.DestinationKey,
                                                        copyObjectRequest.SourceBucketName,
                                                        copyObjectRequest.SourceKey, uploadId)
                {
                    PartSize = size,
                    PartNumber = i + 1,
                    BeginIndex = skipBytes
                };
                var copyResult = UploadPartCopy(request);
                partETags.Add(copyResult.PartETag);
            }

            return partETags;
        }
        #endregion
    }
}