using System.Net.Sockets;
using System.Text;

namespace ConsoleClient {
    public class ClientManager : IDisposable {
        public TcpClient Socket { get; private set; }
        public NetworkStream Stream { get; private set; }
        public bool IsRunning { get; private set; } = false;

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public ClientManager() {
            try {
                Socket = new(Authentication.Config.Host, Authentication.Config.Port);
                if(!Socket.Connected) {
                    Dispose();
                    return;
                }

                Stream = Socket.GetStream();

                Task.Run(GetMessageAsync);
            } catch(Exception) {
                Dispose();
            }
        }

        public async Task GetMessageAsync() {
            while(IsRunning) {
                byte[] rcvBuf = new byte[1024];
                int nbytes = await Stream.ReadAsync(rcvBuf, 0, rcvBuf.Length);
                int sizeOfBuf = int.Parse(Encoding.Default.GetString(rcvBuf, 0, nbytes));

                rcvBuf = new byte[sizeOfBuf];
                nbytes = await Stream.ReadAsync(rcvBuf, 0, rcvBuf.Length);
                string msg = Encoding.Default.GetString(rcvBuf, 0, nbytes);

                await Console.Out.WriteLineAsync(msg);
            }
        }

        public void Dispose() {
            Socket?.Dispose();
        }
    }
}
