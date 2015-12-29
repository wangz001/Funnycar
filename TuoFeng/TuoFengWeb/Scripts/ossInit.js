// 获取 stsToken
var xhr = new XMLHttpRequest();
var ossUpload = new Object();
xhr.onreadystatechange = function () {
    if (xhr.readyState == 4) {
        init(JSON.parse(xhr.responseText));
    }
};
//True 表示脚本会在 send() 方法之后继续执行，而不等待来自服务器的响应。
xhr.open('GET', '/Home/GetOssSecurityToken', true);
xhr.send(null);


var init = function (stsToken) {
    var mossUpload = new OssUpload({
        bucket: 'funnycar',
        endpoint: 'http://oss-cn-beijing.aliyuncs.com',
        // 如果文件大于 chunkSize 则分块上传, chunkSize 不能小于 100KB 即 102400
        chunkSize: 1048576,
        // 分块上传的并发数
        concurrency: 2,
        stsToken:
        {
            "Credentials": {
                "AccessKeySecret": stsToken.AccessKeySecret,
                "AccessKeyId": stsToken.AccessKeyId,
                "Expiration": stsToken.Expiration,
                "SecurityToken": stsToken.SecurityToken
            }
        }
    });
    ossUpload= mossUpload;
};