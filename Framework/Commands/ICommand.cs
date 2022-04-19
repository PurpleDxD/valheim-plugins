using System;

namespace Purps.Valheim.Framework.Commands {
    public interface ICommand {
        string Name { get; }
        string Description { get; }
        Action<string[]> Action { get; }
        bool ShouldPrint { get; }

        void Execute(string commandStr);
    }
}