namespace ConsoleServer.Commands.Terminal {
    [AttributeUsage(AttributeTargets.Class)]
    public class TerminalAttribute : Attribute {
        public string CommandName { get; private set; }
        public string Description { get; private set; }

        public TerminalAttribute(string commandName, string description) {
            CommandName = commandName;
            Description = description;
        }
    }
}
