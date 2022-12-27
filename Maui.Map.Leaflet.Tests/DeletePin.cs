using Maui.Map.Leaflet.Exceptions;
using Maui.Map.Leaflet.Models;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.Map.Leaflet.Tests
{
    public class DeletePin
    {
        [Fact]
        public void PinMustHaveKey()
        {
            var pin = new Pin();

            var map = new Leaflet();

            //Act Listen exception
            var exception = Record.Exception(() => map.DeletePin(pin));

            // Assert Exception
            Assert.IsAssignableFrom<PinMustHaveKeyException>(exception);
        }

        [Fact]
        public void PinMustDecreaseLengthOfListPinsExist()
        {
            var pin = new Pin()
            {
                Key = "pin"
            };
            var map = new Leaflet();
            map.AddPin(pin);
            var count = map.Pins.Count;
            
            map.DeletePin(pin);

            Assert.True(count > map.Pins.Count);
        }

        [Fact]
        public void PinMustExistBeforeDeleteIt()
        {
            var pin = new Pin()
            {
                Key = "pin"
            };

            var map = new Leaflet();

            var exception = Record.Exception(() => map.DeletePin(pin));

            // Assert Exception
            Assert.IsAssignableFrom<PinMustExistException>(exception);
        }
    }
}
