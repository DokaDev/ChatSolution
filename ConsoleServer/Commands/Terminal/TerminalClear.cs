namespace ConsoleServer.Commands.Terminal {
    [Terminal("clear", "Clear the screen.")]
    public class TerminalClear {
        public void Execute() {
            Console.Clear();
        }
    }
}
