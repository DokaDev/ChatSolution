namespace ConsoleServer.Controller {
    public class Terminal {
        public void StartTerminal() {
            Task.Run(GetCommandAsync);
        }

        public async Task GetCommandAsync() {
            while(true) {
                await Console.Out.WriteAsync(">> ");
                string? input = await Console.In.ReadLineAsync();
                // todo. Handle User-Command Logic
            }
        }

        public void StopTerminal() {

        }
    }
}
