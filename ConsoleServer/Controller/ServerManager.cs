using System.Net;
using System.Net.Sockets;
using ConsoleServer.Log;
using ConsoleServer.Model;
using ConsoleServer.Repository;

namespace ConsoleServer.Controller {
    public class ServerManager : IDisposable {
        public TcpListener Listener { get; private set; }
        public Terminal Shell { get; private set; }
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

        public void Start() {
            Context.ServerMGR = this;

            Shell = new(this);
            Logger.Log("Set server context to Terminal.");

            Task.Run(StartServerAsync);

            Shell.StartTerminal();
            Logger.Log("Terminal Created");

            Repos.RoomList.Add(new ChatRoom("Global", null));
            //Repos.RoomList.Add(new ChatRoom("채팅방1", "abc", null));
            //Repos.RoomList.Add(new ChatRoom("no password room test",  null));

            Logger.Log("Global room created.");
        }

        /// <summary>
        /// Start Server as Asynchronous
        /// </summary>
        public async Task StartServerAsync() {
            if(IsRunning)
                return;

            Listener = new(IPAddress.Parse(Authentication.Config.Host), Port);
            Listener.Start();

            Logger.Log("Server started.");

            IsRunning = true;

            await AcceptClientAsync();
        }

        /// <summary>
        /// Accept Client send conn. Request as Asynchronous
        /// </summary>
        public async Task AcceptClientAsync() {
            while(IsRunning) {
                Logger.Log("Listening");
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
