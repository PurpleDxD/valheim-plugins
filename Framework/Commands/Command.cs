using System;
using System.Linq;

namespace Purps.Valheim.Framework.Commands {
    public class Command : ICommand {
        public Command(string name, string description, Action<string[]> action, bool shouldPrint = true) {
            Name = name;
            Description = description;
            Action = action;
            ShouldPrint = shouldPrint;
        }

        public string Name { get; }
        public string Description { get; }
        public Action<string[]> Action { get; }
        public bool ShouldPrint { get; }

        public void Execute(string commandStr) {
            var sanitizedCommandStr =
                string.Join(" ", commandStr.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries));
            var commands = sanitizedCommandStr.Split(' ').Skip(1).ToArray();
            commands = Array.ConvertAll(commands, c => c.ToLower());
            Action.Invoke(commands);
        }
    }
}