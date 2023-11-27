using ConsoleServer.Repository;

namespace ConsoleServer.Commands.Terminal {
    [Terminal("stop server", "stop server")]
    public class TerminalStop {
        public void Execute() {
            Context.ServerMGR.StopServer();
        }
    }
}
