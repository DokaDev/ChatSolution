using ConsoleServer.Model;

namespace ConsoleServer.Repository {
    public static class Repos {
        public static List<User> UserList { get; } = new();
        public static List<ChatRoom> RoomList { get; } = new();

    }
}
