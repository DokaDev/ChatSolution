namespace ConsoleServer.Model {
    public class ChatRoom : IDisposable {
        public string RoomName { get; set; }
        public string Password { get; private set; }

        public User Admin { get; set; }

        public List<User> UserList { get; set; } = new();

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        /// <param name="roomName">ROOM NAME</param>
        /// <param name="user">ADMIN</param>
        public ChatRoom(string roomName, User user) {
            RoomName = roomName;
            Password = "";
        }
        /// <summary>
        /// CONSTRUCTOR(OVERLOADED)
        /// </summary>
        /// <param name="roomName">ROOM NAME</param>
        /// <param name="pw">PASSWORD</param>
        /// <param name="user">ADMIN</param>
        public ChatRoom(string roomName, string pw, User user) {
            RoomName = roomName;
            Password = pw;
        }

        public async Task Whisper(User sender, string username, string msg) {
            // todo. find user
            User? user = UserList.FirstOrDefault((r) => r.UserName == username);

            if(user != null)
                await user.SendMessageAsync($"[WHISPER] {sender.UserName} : {msg}");
            else
                await sender.SendMessageAsync($"A user named '{username} does not exist!'");
        }

        public async Task BroadCast(string msg) {
            foreach(var usr in UserList) {
                await usr.SendMessageAsync(msg);
            }
        }

        /// <summary>
        /// Only for Admin
        /// </summary>
        /// <param name="user">Function caller</param>
        public async Task DeleteRoom(User user) {
            if(Admin == user) {
                await BroadCast($"Server :: [{RoomName}] deleted by administrator, [{Admin.UserName}]");
                Dispose();
            } else {
                await user.SendMessageAsync("This function only can use administrator!");
            }
        }

        /// <summary>
        /// Only for Admin
        /// </summary>
        /// <param name="request">Function caller</param>
        /// <param name="target"></param>
        public async Task KickUser(User request, User target) {
            if(Admin == request) {

            } else {
                await request.SendMessageAsync("This function only can use administrator!");
            }
        }

        public void Dispose() {
            foreach(var user in UserList) {
                user.CurrentRoom = null;
            }
            UserList.Clear();
        }
    }
}
