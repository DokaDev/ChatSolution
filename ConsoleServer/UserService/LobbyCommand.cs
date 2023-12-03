using ConsoleServer.Model;
using ConsoleServer.Repository;

namespace ConsoleServer.UserService {
    public static class LobbyCommand {
        public static async Task ShowUserList(User context) {
            string formattedText = $"=========== USER LIST [{Repos.UserList.Count}] ===========\n";

            foreach(User user in Repos.UserList) {
                formattedText += $"{user.Host} : {user.UserName}\n";
            }
            await context.SendMessageAsync(formattedText);
        }

        public static async Task JoinRoom(User context) {
            string formattedText = "Server :: Type the number of room\n\n";
            formattedText += $"=========== CHAT ROOMS [{Repos.RoomList.Count}] ===========\n";

            int index = 0;
            foreach(ChatRoom room in Repos.RoomList) {
                if(!string.IsNullOrEmpty(room.Password))
                    formattedText += $"[{index}] {room.RoomName} : {room.UserList.Count} users  ## PW ##\n";
                else
                    formattedText += $"[{index}] {room.RoomName} : {room.UserList.Count} users\n";

                index++;
            }

            await context.SendMessageAsync(formattedText);
            // get room-number
            string response = await context.GetMessageAsync();

            try {
                await context.JoinRoom(Repos.RoomList[int.Parse(response)]);
            } catch(Exception ex) { }
        }
    }
}
