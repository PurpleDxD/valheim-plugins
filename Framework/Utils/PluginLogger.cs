using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Purps.Valheim.Framework.Utils {

    public class PluginLogger {
        private static readonly Lazy<PluginLogger> instance = new Lazy<PluginLogger>(() => new PluginLogger());
        private static readonly Dictionary<string, ManualLogSource> loggers = new Dictionary<string, ManualLogSource>();

        private static PluginLogger Instance {
            get {
                return instance.Value;
            }
        }

        public static void Destroy() {
            Debug("Destroying Logger");

            foreach (var logger in loggers) {
                Logger.Sources.Remove(logger.Value);
            }

            loggers.Clear();
        }

        private ManualLogSource GetLogger() {
            var type = new StackFrame(3).GetMethod().DeclaringType;

            if (!loggers.TryGetValue(type.FullName, out ManualLogSource ret)) {
                loggers.Add(type.FullName, Logger.CreateLogSource(type.FullName));
            }

            return ret;
        }

        private static void Log(LogLevel level, object data) => Instance.GetLogger().Log(level, data);

        public static void Fatal(object data) => Log(LogLevel.Fatal, data);

        public static void Error(object data) => Log(LogLevel.Error, data);

        public static void Warning(object data) => Log(LogLevel.Warning, data);

        public static void Message(object data) => Log(LogLevel.Message, data);

        public static void Info(object data) => Log(LogLevel.Info, data);

        public static void Debug(object data) => Log(LogLevel.Debug, data);
    }
}