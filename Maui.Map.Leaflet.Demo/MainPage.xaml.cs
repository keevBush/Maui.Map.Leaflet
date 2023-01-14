namespace Maui.Map.Leaflet.Demo
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, System.EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private void Leaflet_MapCliqued(object sender, EventArgs.MapClickedEventArg e)
        {
            map.AddPin(new Models.Pin
            {
                Key =Guid.NewGuid().ToString(),
                Latitude = e.PositionLatitude,
                Longitude = e.PositionLongitude,
            });
        }
    }
}