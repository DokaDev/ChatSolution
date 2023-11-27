using ConsoleServer.Log;
using ConsoleServer.Model;
using ConsoleServer.Repository;

namespace ConsoleServer.Commands.Terminal {
    [Terminal("userlist", "Display connected user list")]
    public class TerminalUserList {
        public void Execute() {
            if(Repos.UserList.Count == 0)
                Console.WriteLine("No User connected yet..");
            else {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=========== USER LIST ===========");
                Console.ForegroundColor = ConsoleColor.White;

                foreach(User usr in Repos.UserList) {
                    if(usr.CurrentRoom == null) Console.WriteLine($"{usr.Host} : {usr.UserName} (Not in room)");
                    else
                        Console.WriteLine($"{usr.Host} : {usr.UserName} ({usr.CurrentRoom.RoomName})");
                }
            }

            Console.WriteLine();
        }
    }
}
