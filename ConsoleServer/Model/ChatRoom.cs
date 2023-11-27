namespace ConsoleServer.Model {
    public class ChatRoom : IDisposable {
        public List<User> UserList = new();


        public void Dispose() {

        }
    }
}
