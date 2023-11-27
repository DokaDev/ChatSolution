using ConsoleServer.Controller;

using(ServerManager svr = new(Authentication.Config.Port)) {
    Task.Run(svr.StartServerAsync);
}