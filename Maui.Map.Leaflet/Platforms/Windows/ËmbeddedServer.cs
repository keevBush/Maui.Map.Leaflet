using EmbedIO;
using EmbedIO.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.Map.Leaflet
{
    public class ËmbeddedServer
    {
        #region Variables
        private WebServer _server;
        #endregion
        public void InitializeServer(string port)
        {
            var url = $"http://localhost:{port}/";

            _server = new WebServer(o => o
                    .WithUrlPrefix(url)
                    .WithMode(HttpListenerMode.EmbedIO))
                .WithWebApi("/api", m => m.WithController<LeafletController>());
            _server.StateChanged += Server_StateChanged;

            _server.Start();
        }

        private async void Server_StateChanged(object sender, WebServerStateChangedEventArgs e)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Leaflet-Logs");
            var directory = new DirectoryInfo(path);
            if(!directory.Exists)
                directory.Create();
            try
            {
                await File.AppendAllLinesAsync(Path.Combine(path, "leaflet.log"), new string[] { $"Date :{DateTime.Now} | Old state : {e.OldState} | New state:{e.NewState}" });

            }
            catch (Exception)
            {
            }
        }

        public void StopServer()
        {
            _server.Listener.Stop();
        }
    }
}
