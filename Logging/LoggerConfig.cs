using System;
using System.Diagnostics;
using System.IO;

namespace logandtrac.Logging
{
    public static class LoggerConfig
    {
        private static string logDirectory = "Logs";
        private static string? currentLogFile; // делаем поле nullable

        public static void Configure()
        {
            // Очищаем существующие слушатели
            Trace.Listeners.Clear();
            
            // Создаем директорию для логов, если её нет
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Создаем имя файла с временной меткой
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            currentLogFile = Path.Combine(logDirectory, $"taskmanager_{timestamp}.log");

            // Настраиваем слушатели
            ConfigureConsoleListener();
            ConfigureFileListener();
            
            // Автоматически сбрасывать буфер после каждой записи
            Trace.AutoFlush = true;
            
            // Логируем старт приложения
            LogInformation("Приложение TaskManager запущено");
            LogTrace($"Директория логов: {Path.GetFullPath(logDirectory)}");
            if (currentLogFile != null)
            {
                LogTrace($"Файл лога: {currentLogFile}");
            }
        }

        private static void ConfigureConsoleListener()
        {
            var consoleListener = new ConsoleTraceListener();
            consoleListener.Name = "ConsoleLogger";
            Trace.Listeners.Add(consoleListener);
        }

        private static void ConfigureFileListener()
        {
            try
            {
                if (currentLogFile != null)
                {
                    var fileListener = new TextWriterTraceListener(currentLogFile);
                    fileListener.Name = "FileLogger";
                    
                    // Добавляем форматирование для файлового лога
                    fileListener.TraceOutputOptions = TraceOptions.DateTime | TraceOptions.ThreadId;
                    
                    Trace.Listeners.Add(fileListener);
                }
            }
            catch (Exception ex)
            {
                LogCritical($"Не удалось создать файл лога: {ex.Message}");
            }
        }

        // Методы для логирования с разными уровнями
        public static void LogInformation(string message)
        {
            Trace.TraceInformation($"[INFO] {DateTime.Now:HH:mm:ss.fff} - {message}");
        }

        public static void LogWarning(string message)
        {
            Trace.TraceWarning($"[WARN] {DateTime.Now:HH:mm:ss.fff} - {message}");
        }

        public static void LogError(string message)
        {
            Trace.TraceError($"[ERROR] {DateTime.Now:HH:mm:ss.fff} - {message}");
        }

        public static void LogCritical(string message)
        {
            // Используем Trace.Fail для критических ошибок вместо TraceEvent
            Trace.Fail($"[CRITICAL] {DateTime.Now:HH:mm:ss.fff} - {message}");
        }

        public static void LogTrace(string message)
        {
            Trace.WriteLine($"[TRACE] {DateTime.Now:HH:mm:ss.fff} - {message}");
        }

        public static void LogOperationStart(string operation)
        {
            LogTrace($"Начало операции {operation}");
        }

        public static void LogOperationEnd(string operation, string result = "")
        {
            string resultMsg = string.IsNullOrEmpty(result) ? "" : $" Результат: {result}";
            LogTrace($"Конец операции {operation}{resultMsg}");
        }
    }
}