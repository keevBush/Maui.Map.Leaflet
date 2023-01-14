using Android.Webkit;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.Map.Leaflet
{
    public class JSBridge : Java.Lang.Object
    {
        readonly Leaflet _leaflet;

        internal JSBridge(Leaflet leaflet)
        {
            _leaflet = leaflet;
        }

        [JavascriptInterface]
        [Export("invokeAction")]
        public void InvokeAction(string data) => _leaflet.InvokeAction(data);
    }
}
