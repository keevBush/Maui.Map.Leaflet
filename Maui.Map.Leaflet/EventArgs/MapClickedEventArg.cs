using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.Map.Leaflet.EventArgs
{
    public class MapClickedEventArg : BaseLeafletEventArg
    {
        public const string EventName = "map-cliqued";
        [JsonProperty]
        public double PositionLatitude { get; set; }
        [JsonProperty]
        public double PositionLongitude { get; set; }
    }
}
