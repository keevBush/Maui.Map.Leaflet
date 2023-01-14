using EmbedIO.Routing;
using EmbedIO;
using EmbedIO.WebApi;
using System.Xml.Linq;
using Maui.Map.Leaflet.EventArgs;
using Newtonsoft.Json;
using System.Globalization;

namespace Maui.Map.Leaflet
{
    public class LeafletController : WebApiController
    {
        public static event EventHandler<(string eventName, string args)> LeafletServerRequest;

        [Route(HttpVerbs.Get, "/map-cliqued")]
        public async Task PostJsonData()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-EN");
            var data = HttpContext.GetRequestQueryData();

            LeafletServerRequest?.Invoke(this, (eventName: MapClickedEventArg.EventName, args: JsonConvert.SerializeObject(new MapClickedEventArg
            {
                PositionLatitude = Convert.ToDouble (data["latitude"]),
                PositionLongitude = Convert.ToDouble(data["longitude"])
            })));

        }
    }
}