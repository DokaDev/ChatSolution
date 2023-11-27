using ConsoleServer.Controller;

using(ServerManager svr = new(Authentication.Config.Port)) {
    svr.Start();
    while(true)
        ;
}