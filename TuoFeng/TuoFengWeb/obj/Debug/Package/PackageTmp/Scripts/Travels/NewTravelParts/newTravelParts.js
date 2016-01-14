var longitude, latitude;//全局变量，经度和纬度
var map, geolocation;
//加载地图，调用浏览器定位服务
map = new AMap.Map('container', {
    resizeEnable: true
});
//地理位置编码
var geocoder;
AMap.service('AMap.Geocoder', function () { //回调函数
    //实例化Geocoder
    geocoder = new AMap.Geocoder({
        city: "010" //城市，默认：“全国”
    });
});

var getLocation= function() {
    map.plugin('AMap.Geolocation', function() {
        geolocation = new AMap.Geolocation({
            enableHighAccuracy: true, //是否使用高精度定位，默认:true
            timeout: 10000, //超过10秒后停止定位，默认：无穷大
            buttonOffset: new AMap.Pixel(10, 20), //定位按钮与设置的停靠位置的偏移量，默认：Pixel(10, 20)
            zoomToAccuracy: true, //定位成功后调整地图视野范围使定位位置及精度范围视野内可见，默认：false
            buttonPosition: 'RB'
        });
        map.addControl(geolocation);
        geolocation.getCurrentPosition();
        AMap.event.addListener(geolocation, 'complete', onComplete); //返回定位信息
        AMap.event.addListener(geolocation, 'error', onError); //返回定位出错信息
    });
}

//解析定位结果
function onComplete(data) {
    var str = ['定位成功'];
    str.push('经度：' + data.position.getLng());
    str.push('纬度：' + data.position.getLat());
    str.push('精度：' + data.accuracy + ' 米');
    str.push('是否经过偏移：' + (data.isConverted ? '是' : '否'));
    //document.getElementById('tip').innerHTML = str.join('<br>');
    longitude = data.position.getLng();
    latitude = data.position.getLat();
    geoToAddress(data.position.getLng(), data.position.getLat());
}

//解析定位错误信息
function onError(data) {
    document.getElementById('tip').innerHTML = '定位失败';
}

//转换地址信息
function geoToAddress(lng, lan) {
    if (geocoder == null) return;
    var lnglatXy = [lng, lan];//地图上所标点的坐标
    geocoder.getAddress(lnglatXy, function (status, result) {
        if (status === 'complete' && result.info === 'OK') {
            //获得了有效的地址信息:
            $(".tab_address").html(result.regeocode.formattedAddress);
        } else {
            //获取地址失败
        }
    });
}
