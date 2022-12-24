using Maui.Map.Leaflet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.Map.Leaflet.Tests
{
    public class AddPin
    {
        [Fact]
        public void CheckIfPinCanAdded()
        {
            var pin = new Pin
            {
                Key = "pin-1",
                Latitude = 12,
                Longitude = 15
            };

            var map = new Leaflet();
            var lastCount = map.Pins.Count;
            map.AddPin(pin);
            var newCount = map.Pins.Count;
            Assert.NotEqual(newCount, lastCount);
            Assert.True(lastCount < newCount);
        }

        public void FailIfPinHaveNotKey()
        {
            var pin = new Pin();

            var map = new Leaflet();
            var lastCount = map.Pins.Count;
            map.AddPin(pin);
            var newCount = map.Pins.Count;
        }
    }
}
