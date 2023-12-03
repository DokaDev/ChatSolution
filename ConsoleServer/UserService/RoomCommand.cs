using ConsoleServer.Model;
using ConsoleServer.Repository;
using System.Linq;

namespace ConsoleServer.UserService {
    public static class RoomCommand {
        /// <summary>
        /// Command: #userlist
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task ShowUserList(User context) {
            //Console.WriteLine($"=========== USER LIST [{Repos.UserList.Count}] ===========");
            string formattedText = $"=========== USER LIST [{context.CurrentRoom.UserList.Count}] ===========\n";

            foreach(User user in context.CurrentRoom.UserList) {
                formattedText += $"{user.Host} : {user.UserName}\n";
            }

            await context.SendMessageAsync(formattedText);
        }
    }
}
