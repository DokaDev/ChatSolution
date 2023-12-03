using ConsoleServer.Log;
using ConsoleServer.Repository;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ConsoleServer.UserService;

namespace ConsoleServer.Model {
    public class User : IDisposable {
        /// <summary>
        /// Property related to User Context
        /// </summary>
        public TcpClient Socket { get; }
        public NetworkStream Stream { get; private set; }
        public bool IsRunning { get; private set; } = false;

        /// <summary>
        /// Property related to Chatroom
        /// </summary>
        public bool IsInRoom { get; private set; } = false;
        public ChatRoom? CurrentRoom { get; set; }

        /// <summary>
        /// Property related to User Service
        /// </summary>
        public string UserName { get; private set; } = "Not Assigned";
        public string? Host { get; }

        // just field..
        private string message = String.Empty;

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public User(TcpClient socket) {
            Socket = socket;

            if(Socket.Connected == false)
                return;
            IsRunning = true;

            Host = ((IPEndPoint?)Socket.Client.RemoteEndPoint)?.Address.ToString();
            Stream = Socket.GetStream();

            Logger.Log($"New socket connected. {Host}");

            Task.Run(RunAsync);
            Task.Run(HeartBeat);
        }

        public async Task HeartBeat() {
            while(IsRunning) {
                Debug.WriteLine("Check Connection");
                if(!Socket.Connected) {
                    Dispose();
                    return;
                }
                await Task.Delay(1000);
            }

        }

        public async Task RunAsync() {
            // step1. get username
            string tmp;
            bool isInvalidName;

            await SendMessageAsync("Welcome! Plz type your name!");

            do {
                isInvalidName = false;
                tmp = await GetMessageAsync();

                // desc. name validation
                if(IsUserNameTaken(tmp)) {
                    await SendMessageAsync($"Name '{tmp}' is already exist! plz use another name.");
                    isInvalidName = true;
                }

                //await Task.Delay(100);

            } while(isInvalidName);

            UserName = tmp;

            await SendMessageAsync($"Good to see you, {UserName}!");
            await SendMessageAsync($"Your IP: {Host}");

            // common loop
            while(IsRunning) {
                message = await GetMessageAsync();

                // todo. handle message _ ClientCommand(in room)
                if(IsInRoom) {
                    if(message.ToLower() == "q") {  // handle 'Quit' pf.
                        await LeaveRoom();
                        break;
                    } else {
                        if(message.StartsWith('#')) {   // handle Command(Prefix = '#')
                            switch(message.ToLower()) {
                                case "#userlist":
                                    await RoomCommand.ShowUserList(this);
                                    break;
                                case "#w":
                                    break;
                                case "#leave":
                                    await LeaveRoom();
                                    break;
                                default:
                                    await SendMessageAsync($"[{message}] is Unknown command!");
                                    break;
                            }
                        } else {    // broadcast
                            await CurrentRoom.BroadCast($"{UserName} : {message}");
                        }
                    }
                } else {
                    switch(message.ToLower()) {
                        case "#rooms":
                            await SendMessageAsync("List Room Start!");
                            break;
                        case "#join":
                            //await SendMessageAsync("Join Room Start!");
                            await LobbyCommand.JoinRoom(this);
                            break;
                        case "#userlist":
                            //await SendMessageAsync("UserList Start!");
                            await LobbyCommand.ShowUserList(this);
                            break;

                        default:
                            await SendMessageAsync($"Server(Whisper) :: [{message}] is Unknown Command");
                            break;
                    }
                }
            }
        }

        public async Task FindRoom(string roomName) {
            ChatRoom? room = Repos.RoomList.FirstOrDefault((r) => r.RoomName == roomName);
            if(room != null)
                await JoinRoom(room);
            else
                await SendMessageAsync($"Server :: A chat room named '{roomName} does not exist!'");
        }

        public async Task JoinRoom(ChatRoom room) {
            if(IsInRoom)
                return;

            CurrentRoom = room;

            if(!string.IsNullOrEmpty(room.Password)) {
                await SendMessageAsync($"Server :: Room [{room.RoomName}] has a password. Plz enter the password!");

                int count = 0;

                while(count < 3) {
                    string pw = await GetMessageAsync();

                    if(!string.Equals(pw, room.Password)) {
                        if(count <= 2) {
                            await SendMessageAsync($"Server :: Incorrect password! Please try again {count}");
                        } else {
                            await SendMessageAsync($"Server :: Incorrect password! plz try next time..");
                        }

                        count++;
                    } else
                        break;
                }
            }
            room.UserList.Add(this);
            CurrentRoom = room;
            IsInRoom = true;

            await SendMessageAsync($"Server :: Entered room [{room.RoomName}]!");
            await CurrentRoom.BroadCast($"Server :: User {UserName} connected..!");
        }

        public async Task LeaveRoom() {
            if(IsInRoom) {
                if(CurrentRoom.Admin == this) {
                    await CurrentRoom.DeleteRoom(this);
                } else {
                    await CurrentRoom.BroadCast($"Server :: User [{UserName}] leaved from the room [{CurrentRoom.RoomName}]");
                    CurrentRoom.UserList.Remove(this);
                    IsInRoom = false;
                    CurrentRoom = null;
                }
            }
        }

        private bool IsUserNameTaken(string usrName) {
            return Repos.UserList.Any(usr => usr.UserName == usrName);
        }

        public async Task<string> GetMessageAsync() {
            byte[] rcvBuf;
            int nBytes;

            rcvBuf = new byte[1024];
            nBytes = await Stream.ReadAsync(rcvBuf, 0, rcvBuf.Length);
            string msg = Encoding.Default.GetString(rcvBuf, 0, nBytes);

            return msg;
        }

        public async Task SendMessageAsync(string msg) {
            byte[] sndBuf = Encoding.Default.GetBytes($"{msg}");
            await Stream.WriteAsync(sndBuf, 0, sndBuf.Length);
        }

        public void Dispose() {
            Socket?.Dispose();
            Stream?.Dispose();

            LeaveRoom();

            Logger.Log($"{Host} - {UserName} Disconnected.");

            Repos.UserList.Remove(this);

            IsRunning = false;
        }
    }
}