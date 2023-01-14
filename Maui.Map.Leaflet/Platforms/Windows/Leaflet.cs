using Maui.Map.Leaflet.EventArgs;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;

namespace Maui.Map.Leaflet
{
    // All the code in this file is only included on Windows.
    public partial class Leaflet
    {
        protected string _htmlCode = "<!DOCTYPE html><html lang='en'><head><meta charset='UTF-8'><meta http-equiv='X-UA-Compatible' content='IE=edge'><meta name='viewport' content='width=device-width, initial-scale=1.0'><title>Leaflet.Windows</title><link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.3/leaflet.css'><script src='https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.3/leaflet.js'></script><style>#map {height: 20px;width: 20px;}body { margin: 0; padding: 0 }</style></head><body><div id='map'></div><script>  let map = null; function InvokeMapClickedAction(data) { fetch(`http://localhost:9696/api/map-cliqued?latitude=${data.PositionLatitude}&longitude=${data.PositionLongitude}`).then(response => { console.log('done') }).catch(err => { console.error(err) }); } function InitializeMap(centerLat = -11.666667, centerLng = 27.483334, zoom = 13, layer = 'https://tile.openstreetmap.org/{z}/{x}/{y}.png', attribution = 'Maui.Map.Leaflet Contributor') { if (map === null) { map = L.map('map').setView([centerLat, centerLng], zoom); } L.tileLayer(layer, { attribution: attribution }).addTo(map); setTimeout(() => { document.getElementById('map').style.width = `${window.innerWidth}px`; document.getElementById('map').style.height = `${window.innerHeight}px`; map.invalidateSize(); InitializeEvent(); }, 600); } function InitializeEvent() { map.on('click', function (e) { InvokeMapClickedAction({ PositionLatitude: e.latlng.lat, PositionLongitude: e.latlng.lng }); }); }function AddPin(latitude, longitude, key){L.marker([latitude,longitude],{Key : key}).addTo(map);}</script></body></html>";
        protected ËmbeddedServer _server = null;
        public virtual async void LeafletController_LeafletServerRequest(object sender, (string eventName, string args) e)
        {
            switch (e.eventName)
            {
                case MapClickedEventArg.EventName:
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        MapTappedCommand?.Execute(JsonConvert.DeserializeObject<MapClickedEventArg>(e.args));
                        MapCliqued?.Invoke(this, JsonConvert.DeserializeObject<MapClickedEventArg>(e.args));

                    });
                    break;
                default:
                    break;
            }
        }


    }
}