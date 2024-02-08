using System;

namespace FFU_Industrial_Capacity {
    public static class ModLog {
        private const string LogPrefix = "[Industrial Capacity]";
        public static void Info(string message) {
            Mafi.Log.Info($"{LogPrefix} {message}");
        }
        public static void Warning(string message) {
            Mafi.Log.Warning($"{LogPrefix} {message}");
        }
        public static void Error(string message) {
            Mafi.Log.Error($"{LogPrefix} {message}");
        }
        public static void Exception(Exception e, string message) {
            Mafi.Log.Exception(e, $"{LogPrefix} {message}");
        }
    }
}
