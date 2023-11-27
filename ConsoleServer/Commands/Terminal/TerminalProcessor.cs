using System.Reflection;

namespace ConsoleServer.Commands.Terminal {
    public class TerminalProcessor {
        public void Process(string command) {
            List<Type> commandClasses = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(TerminalAttribute), true).Length > 0)
                .ToList();

            foreach(var cls in commandClasses) {
                TerminalAttribute attr = (TerminalAttribute)cls.GetCustomAttribute(typeof(TerminalAttribute));
                if(attr.CommandName == command) {
                    var instance = Activator.CreateInstance(cls);
                    cls.GetMethod("Execute").Invoke(instance, null);
                    return;
                }
            }

            Console.WriteLine("Unknown command\n");
        }
    }
}
