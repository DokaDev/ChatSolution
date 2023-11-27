using ConsoleServer.Log;

namespace ConsoleServer.Commands.Terminal {
    [Terminal("log", "Display logs")]
    public class TerminalLog {
        public void Execute() {
            Console.WriteLine(Logger.log);
            Console.WriteLine();
        }
    }
}
