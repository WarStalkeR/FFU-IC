using System;

namespace FFU_Industrial_Lib {
    public static class ModLog {
        private static string _logPrefix => FFU_ILib.ModLoggerPrefix;
        public static void Info(string message) {
            Mafi.Log.Info($"{_logPrefix} {message}");
        }
        public static void Warning(string message) {
            Mafi.Log.Warning($"{_logPrefix} {message}");
        }
        public static void Error(string message) {
            Mafi.Log.Error($"{_logPrefix} {message}");
        }
        public static void Exception(Exception e, string message) {
            Mafi.Log.Exception(e, $"{_logPrefix} {message}");
        }
    }
}
