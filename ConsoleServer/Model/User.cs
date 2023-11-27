using System.Net.Sockets;

namespace ConsoleServer.Model {
    public class User : IDisposable {
        public TcpClient Socket { get; } = null;
        public bool IsRunning { get; private set; } = false;

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public User(TcpClient socket) {
            Socket = socket;

            if(Socket.Connected == false)
                return;
            IsRunning = true;

            Task.Run(RunAsync);
        }

        public async Task RunAsync() {

        }

        public void Dispose() {
            Socket?.Dispose();
        }
    }
}
