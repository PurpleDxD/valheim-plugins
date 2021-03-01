using System.Collections.Generic;
using System.Linq;
using Purps.Valheim.Framework.Utils;
using Purps.Valheim.Utils;

namespace Purps.Valheim.Framework.Commands {
    public class CommandProcessor {
        public List<ICommand> Commands { get; } = new List<ICommand>();

        public void addCommand(ICommand command) {
            Commands.Add(command);
        }

        public void clearCommands() {
            Commands.Clear();
        }

        public void printCommands(string[] parameters) {
            Commands.FindAll(command => command.ShouldPrint)
                .ForEach(command => ConsoleUtils.WriteToConsole($"{command.Name} => {command.Description}"));
        }

        public void executeCommand(string commandStr) {
            var command = Commands.FirstOrDefault(e => e.Name.Equals(commandStr.Split(' ').First()));
            command?.Execute(commandStr);
        }
    }
}