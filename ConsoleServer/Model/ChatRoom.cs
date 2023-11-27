namespace ConsoleServer.Model {
    public class ChatRoom : IDisposable {
        public string RoomName { get; set; }
        public string Password { get; private set; }

        public User Admin { get; set; }

        public List<User> UserList { get; set; } = new();

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public ChatRoom(string roomName, User user) {
            RoomName = roomName;
            Password = "";
        }
        public ChatRoom(string roomName, string pw, User user) {
            RoomName = roomName;
            Password = pw;
        }

        public async Task Whisper(User sender, string username, string msg) {
            // todo. find user
            User? user = UserList.FirstOrDefault((r) => r.UserName == username);

            if (user != null)
                await user.SendMessageAsync($"[WHISPER] {sender.UserName} : {msg}");
            else
                await sender.SendMessageAsync($"A user named '{username} does not exist!'");
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
