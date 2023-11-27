using System;
using System.Net.Sockets;
using System.Text;

namespace ConsoleServer.Model {
    public class ChatRoom : IDisposable {
        public string RoomName { get; set; }
        public string Password { get; private set; }
        public User Admin { get; set; }

        public List<User> UserList { get; set; } = new();

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public ChatRoom(string roomName) {
            RoomName = roomName;
            Password = "";
        }
        public ChatRoom(string roomName, string pw) {
            RoomName = roomName;
            Password = pw;
        }

        public async Task Whisper(string username, string msg) {
            // todo. find user

            // todo. send message to usr
        }

        public async Task BroadCast(string msg) {
            // handle null message

            byte[] buffer = Encoding.UTF8.GetBytes(msg);

            foreach(var usr in UserList) {
                await SendMessageToUserAsync(usr.Socket, buffer);
            }
        }

        public async Task SendMessageToUserAsync(TcpClient socket, byte[] buf) {
            NetworkStream stream = socket.GetStream();

            byte[] sizeBuf = Encoding.Default.GetBytes(buf.Length.ToString());

            // step1. send size-of buffer
            await stream.WriteAsync(sizeBuf, 0, sizeBuf.Length);

            // step2. send message
            await stream.WriteAsync(buf, 0, buf.Length);
        }

        public void Dispose() {
            UserList.Clear();
        }
    }
}
