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

            Socket = new(Authentication.Config.Host, Authentication.Config.Port);
            //Console.WriteLine("Connected!");

            Stream = Socket.GetStream();

            IsRunning = true;

            Thread sender = new(Sender);
            Thread receiver = new(Receiver);

            sender.Start();
            receiver.Start();
        }

        public void Receiver() {
            try {
                while(IsRunning) {
                    byte[] rcvBuf = new byte[1024];
                    //int nbytes = Stream.Read(rcvBuf, 0, rcvBuf.Length);
                    //int sizeOfBuf = int.Parse(Encoding.Default.GetString(rcvBuf, 0, nbytes));

                    //rcvBuf = new byte[sizeOfBuf];
                    int nbytes = Stream.Read(rcvBuf, 0, rcvBuf.Length);
                    string msg = Encoding.Default.GetString(rcvBuf, 0, nbytes);

                    Console.WriteLine(msg);
                }
            } catch(Exception ex) {

            } finally {
                Dispose();
            }

        }

        public void Sender() {
            try {
                while(IsRunning) {
                    string? msg = Console.ReadLine();

                    switch(msg.ToLower()) {
                        case "#clear":
                            Console.Clear();
                            continue;
                    }

                    byte[] sndBuf = Encoding.Default.GetBytes(msg);
                    //byte[] sizeOfBuf = Encoding.Default.GetBytes(sndBuf.Length.ToString());

                    //Stream.Write(sizeOfBuf, 0, sizeOfBuf.Length);
                    Stream.Write(sndBuf, 0, sndBuf.Length);
                }
            } catch(Exception ex) {

            } finally {
                Dispose();
            }

        }
        public void Dispose() {
            Socket?.Dispose();
            Stream?.Dispose();

            Console.WriteLine("Disconnected from server.");
        }
    }
}
