using Maui.Map.Leaflet.Models;
using Maui.Map.Leaflet.Exceptions;
using Microsoft.Maui.ApplicationModel;

namespace Maui.Map.Leaflet.Tests
{
    public class UpdatePin
    {
        [Fact]
        public void PinMustExistBeforeEditIt()
        {
            var pin = new Pin()
            {
                Key = "pin"
            };
            var leaflet = new Leaflet();
            leaflet.AddPin(pin);
            string pinInitString = pin.ToString();
            pin.Longitude = 10;

            leaflet.UpdatePin(pin);
            var pinUpdated = leaflet.Pins.First(p => p.Key == "pin");

            Assert.NotEqual(pinInitString, pinUpdated.ToString());
        }

        [Fact]
        public void PinMustHaveKey()
        {
            var pin = new Pin();    

            var leaflet = new Leaflet();

            //Act Listen exception
            var exception = Record.Exception(() => leaflet.UpdatePin(pin));

            // Assert Exception
            Assert.IsAssignableFrom<PinMustHaveKeyException>(exception);
        }

        [Fact]
        public void RisePinMustExistException()
        {

            var pin = new Pin() { Key = "pin" };

            var leaflet = new Leaflet();

            //Act Listen exception
            var exception = Record.Exception(() => leaflet.UpdatePin(pin));

            // Assert Exception
            Assert.IsAssignableFrom<PinMustExistException>(exception);
        }
    }
}