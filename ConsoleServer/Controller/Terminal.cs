using ConsoleServer.Commands.Terminal;

namespace ConsoleServer.Controller {
    public class Terminal : IDisposable {
        public ServerManager Context { get; }
        public TerminalProcessor Processor { get; }

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        /// <param name="context">Get Server Context</param>
        public Terminal(ServerManager context) {
            Context = context;
            Processor = new();
        }

        public void StartTerminal() {
            Task.Run(GetCommandAsync);
        }

        public async Task GetCommandAsync() {
            while(true) {
                Console.ForegroundColor = ConsoleColor.Red;
                await Console.Out.WriteAsync(">> ");
                string? input = await Console.In.ReadLineAsync();
                Console.ForegroundColor = ConsoleColor.White;

                HandleCommand(input);
            }
        }

        public void HandleCommand(string? input) {
            // todo. Handle Command Logic
            Processor.Process(input);
        }

        public void StopTerminal() {
            Dispose();
        }

        public void Dispose() {

        }
    }
}
