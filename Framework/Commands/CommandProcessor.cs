using System.Collections.Generic;
using System.Linq;
using Purps.Valheim.Framework.Utils;
using UnityEngine;

namespace Purps.Valheim.Framework.Commands {
    public class CommandProcessor {
        public List<ICommand> Commands { get; } = new List<ICommand>();

        public void AddCommand(ICommand command) {
            Commands.Add(command);
        }

        public void ClearCommands() {
            Commands.Clear();
        }

        public void PrintCommands(string[] parameters) {
            Commands.FindAll(command => command.ShouldPrint)
                .ForEach(command => ConsoleUtils.WriteToConsole($"{command.Name} => {command.Description}"));
        }

        public void ExecuteCommand(string commandStr) {
            var command =
                Commands.FirstOrDefault(e => e.Name.ToLower().Equals(commandStr.Split(' ').First().ToLower()));
            command?.Execute(commandStr.ToLower());
        }
    }
}