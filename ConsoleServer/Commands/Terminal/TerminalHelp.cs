using System.Reflection;

namespace ConsoleServer.Commands.Terminal {
    [Terminal("help", "Help command")]
    public class TerminalHelp {
        public void Execute() {
            List<Type> commandClasses = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(TerminalAttribute), true).Length > 0)
                .ToList();

            Console.WriteLine("=========== HELP ===========");

            foreach(var cls in commandClasses) {
                TerminalAttribute attr = (TerminalAttribute)cls.GetCustomAttribute(typeof(TerminalAttribute));
                Console.WriteLine($"{attr.CommandName} - {attr.Description}");
            }

            Console.WriteLine();
        }
    }
}
