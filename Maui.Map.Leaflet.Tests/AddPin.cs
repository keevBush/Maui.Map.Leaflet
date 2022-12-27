using Maui.Map.Leaflet.Exceptions;
using Maui.Map.Leaflet.Models;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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

        [Fact]
        public void RisePinNoKeyException()
        {
            var pin = new Pin();
            

            var map = new Leaflet();

            //Act Exception
            var exception = Record.Exception(() => map.AddPin(pin));
            Assert.IsAssignableFrom<PinMustHaveKeyException>(exception);
        }

        [Fact]
        public void CheckIfPinAlreadyExist()
        {
            var pin = new Pin
            {
                Key = "pin-1",
                Latitude = 12,
                Longitude = 15
            };

            var map = new Leaflet();
            map.AddPin(pin);
            //Act Exception
            var exception = Record.Exception(() => map.AddPin(pin));
            Assert.IsAssignableFrom<PinAlreadyExistException>(exception);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("key")]
        public void FinalizeThestTheory(string? key)
        {
            var map = new Leaflet();
            var lastCount = map.Pins.Count;
            var pin = new Pin { Key = key };
            
            if (string.IsNullOrEmpty(key))
            {
                //Act Exception
                var exception = Record.Exception(() => map.AddPin(pin));
                Assert.IsAssignableFrom<PinMustHaveKeyException>(exception);
            }
            else
            {
                map.AddPin(pin);

                var newCount = map.Pins.Count;
                Assert.NotEqual(newCount, lastCount);
                Assert.True(lastCount < newCount);
            }

        }
    }
}
