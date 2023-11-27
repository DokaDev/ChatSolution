using System.Net;
using System.Net.Sockets;

namespace ConsoleServer.Controller {
    public class ServerManager : IDisposable {
        public TcpListener Listener { get; private set; }
        public int Port { get; }
        public bool IsRunning { get; private set; } = false;

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        /// <param name="port"></param>
        public ServerManager() {
            Port = Authentication.Config.Port;
        }
        public ServerManager(int port) {
            Port = port;
        }

        /// <summary>
        /// Start Server as Asynchronous
        /// </summary>
        public async Task StartServerAsync() {
            if(!IsRunning)
                return;

            Listener = new(IPAddress.Parse(Authentication.Config.Host), Port);
            Listener.Start();

            Task.Run(() => GetCommandAsync());
            Task.Run(() => AcceptClientAsync());

            IsRunning = true;
        }

        private async Task AcceptClientAsync() {
            while(IsRunning) {
                TcpClient socket = await Listener.AcceptTcpClientAsync();
            }
        }

        private async Task GetCommandAsync() {
            while(IsRunning) {
                string? input = await Console.In.ReadLineAsync();
                // todo. Handle User-Command Logic
            }
        }

        public void Dispose() {
            Listener?.Stop();
            IsRunning = false;
        }
    }
}
