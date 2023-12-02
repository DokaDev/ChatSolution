using ConsoleServer.Log;
using ConsoleServer.Repository;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

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

            do {
                isInvalidName = false;

                await SendMessageAsync("Welcome! Plz type your name!");
                tmp = await GetMessageAsync();

                // desc. name validation
                if(IsUserNameTaken(tmp)) {
                    await SendMessageAsync($"Name '{tmp}' is already exist! plz use another name.");
                    isInvalidName = true;
                }

                await Task.Delay(100);

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
                        LeaveRoom();
                    } else {
                        if(message.StartsWith('#')) {   // handle Command
                            // todo. validation command

                            // todo. handle command
                        } else {    // broadcast
                            await CurrentRoom.BroadCast($"{UserName} : {message}");
                        }
                    }
                } // join Room1

                // if else then
                // todo. handle command(validation => handle) _ ClientCommand(not in room)
                //Logger.Log($"{usr.UserName} : {message}");
            }
        }

        public async Task FindRoom(string roomName) {
            ChatRoom? room = Repos.RoomList.FirstOrDefault((r) => r.RoomName == roomName);
            if(room != null)
                await JoinRoom(room);
            else
                await SendMessageAsync($"A chat room named '{roomName} does not exist!'");
        }

        public async Task JoinRoom(ChatRoom room) {
            if(IsInRoom)
                return;

            CurrentRoom = room;

            if(!string.IsNullOrEmpty(room.Password)) {
                await SendMessageAsync($"Room [{room.RoomName}] has a password. Plz enter the password!");

                int count = 0;

                while(count < 3) {
                    string pw = await GetMessageAsync();

                    if(!string.Equals(pw, room.Password)) {
                        await SendMessageAsync($"Incorrect password! Please try again");
                        count++;
                    } else
                        break;
                }
            }

            await SendMessageAsync($"Entered room [{room.RoomName}]!");

            room.UserList.Add(this);
            IsInRoom = true;
        }

        public void LeaveRoom() {
            if(IsInRoom) {
                CurrentRoom.UserList.Remove(this);
                IsInRoom = false;
                CurrentRoom = null;
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

            Logger.Log($"{Host} - {UserName} Disconnected.");

            Repos.UserList.Remove(this);

            IsRunning = false;
        }
    }
}