using ConsoleServer.Model;
using ConsoleServer.Repository;

namespace ConsoleServer.Commands.Terminal {
    [Terminal("list rooms", "Display the chat room list on the screen")]
    public class TerminalListRooms {
        public void Execute() {
            if(Repos.RoomList.Count == 0)
                Console.WriteLine("No Rooms Exists yet..");
            else {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=========== CHAT ROOMS ===========");
                Console.ForegroundColor = ConsoleColor.White;
                foreach(ChatRoom room in Repos.RoomList) {
                    if(!string.IsNullOrEmpty(room.Password))
                        Console.WriteLine($"{room.RoomName} : {room.UserList.Count} users  ## PW ##");
                    else
                        Console.WriteLine($"{room.RoomName} : {room.UserList.Count} users");
                }
            }

            Console.WriteLine();
        }
    }
}
