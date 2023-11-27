namespace ConsoleServer.Commands {
    [AttributeUsage(AttributeTargets.Method)]
    public class ServerCommandAttribute : Attribute {
        public string Name { get; }

        public ServerCommandAttribute(string name) {
            Name = name;
        }
    }
}
