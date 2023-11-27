using System.Net;
using System.Net.Sockets;
using ConsoleServer.Model;
using ConsoleServer.Repository;

namespace ConsoleServer.Controller {
    public class ServerManager : IDisposable {
        public TcpListener Listener { get; private set; }
        public int Port { get; }
        public bool IsRunning { get; private set; } = false;
        public Terminal Shell { get; } = new();

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

        public void Start() {
            Task.Run(StartServerAsync);
            Shell.StartTerminal();
        }

        /// <summary>
        /// Start Server as Asynchronous
        /// </summary>
        public async Task StartServerAsync() {
            if(!IsRunning)
                return;

            Listener = new(IPAddress.Parse(Authentication.Config.Host), Port);
            Listener.Start();

            if(!Listener.Pending())
                return;

            IsRunning = true;

            await AcceptClientAsync();
        }

        public async Task AcceptClientAsync() {
            while(IsRunning) {
                TcpClient socket = await Listener.AcceptTcpClientAsync();
                Repos.UserList.Add(new User(socket));
            }
        }



        public void StopServer() {
            Dispose();
        }

        public void Dispose() {
            Listener?.Stop();
            IsRunning = false;
        }
    }
}
