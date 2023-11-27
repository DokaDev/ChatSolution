using System.Net.Sockets;

namespace ConsoleClient {
    public class ClientManager : IDisposable {
        public TcpClient Socket { get; private set; }

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public ClientManager() {
            try {
                Socket = new(Authentication.Config.Host, Authentication.Config.Port);
            } catch(Exception) {
                Dispose();
            }
        }

        public void Dispose() {
            Socket?.Dispose();
        }
    }
}
