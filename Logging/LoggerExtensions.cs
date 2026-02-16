using Serilog;

namespace logandtrac.Logging;

public static class LoggerExtensions
{
    public static void LogOperationStart(this ILogger logger, string operation)
    {
        logger.Debug("▶ Начало операции {Operation}", operation);
    }

    public static void LogOperationEnd(this ILogger logger, string operation, string result = "")
    {
        string resultMsg = string.IsNullOrEmpty(result) ? "" : $" Результат: {result}";
        logger.Debug("◀ Конец операции {Operation}{Result}", operation, resultMsg);
    }
}