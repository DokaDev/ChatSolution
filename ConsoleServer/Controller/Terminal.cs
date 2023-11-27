namespace ConsoleServer.Controller {
    public class Terminal : IDisposable {
        public ServerManager Context { get; }

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        /// <param name="context">Get Server Context</param>
        public Terminal(ServerManager context) {
            Context = context;
        }

        public void StartTerminal() {
            Task.Run(GetCommandAsync);
        }

        public async Task GetCommandAsync() {
            while(true) {
                await Console.Out.WriteAsync(">> ");
                string? input = await Console.In.ReadLineAsync();

                await HandleCommand(input);
            }
        }

        public async Task HandleCommand(string? input) {
            // todo. Handle Command Logic
            await Task.Delay(1000);
        }

        public void StopTerminal() {
            Dispose();
        }

        public void Dispose() {

        }
    }
}
