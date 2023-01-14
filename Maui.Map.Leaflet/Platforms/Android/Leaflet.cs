
using Java.Lang;
using Maui.Map.Leaflet.EventArgs;
using Newtonsoft.Json;

namespace Maui.Map.Leaflet
{
    // All the code in this file is only included on Android.
    public partial class Leaflet
    {
        #region Variables
        protected string _htmlCode = @"<!DOCTYPE html><html lang='en'><head><meta charset='UTF-8'><meta http-equiv='X-UA-Compatible' content='IE=edge'><meta name='viewport' content='width=device-width, initial-scale=1.0'><title>Leaflet.Windows</title><link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.3/leaflet.css'><script src='https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.3/leaflet.js'></script><style>#map {height: 20px;width: 20px;}body { margin: 0; padding: 0}</style></head><body><div id='map'></div><script>  let map = null;function InitializeMap(centerLat = -11.666667, centerLng = 27.483334, zoom = 13, layer = 'https://tile.openstreetmap.org/{z}/{x}/{y}.png', attribution = 'Maui.Map.Leaflet Contributor') { if (map === null) { map = L.map('map', { gestureHandling: true, zoomSnap: 0,zoomDelta: 0.25 }).setView([centerLat, centerLng], zoom); } L.tileLayer(layer, { attribution: attribution }).addTo(map); setTimeout(() => { document.getElementById('map').style.width = `${window.innerWidth}px`; document.getElementById('map').style.height = `${window.innerHeight}px`; map.invalidateSize(); InitializeEvent(); }, 600); } function InitializeEvent() { map.on('click', function (e) { InvokeCSharpAction( JSON.stringify({EventName:'map-cliqued', Latitude: e.latlng.lat, Longitude: e.latlng.lng })); }); }function AddPin(latitude, longitude, key){L.marker([latitude,longitude],{Key : key}).addTo(map);}InitializeMap();</script></body></html>";
        #endregion

        private const string JavascriptFunction = "function InvokeCSharpAction(data){jsBridge.invokeAction(data);}";

        partial void ChangedHandler(object sender)
        {
            if (sender is not WebView { Handler: { PlatformView: Android.Webkit.WebView nativeWebView } }) return;

            nativeWebView.SetWebViewClient(new JavascriptWebViewClient($"javascript: {JavascriptFunction}"));
            nativeWebView.AddJavascriptInterface(new JSBridge(this), "jsBridge");
            nativeWebView.RequestDisallowInterceptTouchEvent(true);
        }

        partial void ChangingHandler(object sender, HandlerChangingEventArgs e)
        {
            if (e.OldHandler != null)
            {
                if (sender is not WebView { Handler: { PlatformView: Android.Webkit.WebView nativeWebView } }) return;
                nativeWebView.RemoveJavascriptInterface("jsBridge");
            }
        }

        partial void ExecuteJavascript(object sender, string jsScript)
        {
            if (sender is not WebView { Handler: { PlatformView: Android.Webkit.WebView nativeWebView } }) return;

            nativeWebView.Post(() =>
            {
                nativeWebView.LoadUrl($"javascript:{jsScript};");
            });
        }
        public void InvokeAction(string data)
        {
            var args = JsonConvert.DeserializeObject<dynamic>(data);
            var eventName = (string)args.EventName;
            switch (eventName)
            {
                case MapClickedEventArg.EventName:
                    var argument = new MapClickedEventArg
                    {
                        PositionLatitude = (double)args.Latitude,
                        PositionLongitude = (double)args.Longitude,
                    };
                    MapCliqued?.Invoke(this, argument);
                    MapTappedCommand?.Execute(argument);
                    break;
                default:
                    break;
            }
        }

    }
}