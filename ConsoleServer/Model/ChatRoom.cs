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
            foreach(var usr in UserList) {
                await usr.SendMessageAsync(msg);
            }
        }

        public void Dispose() {
            UserList.Clear();
        }
    }
}
