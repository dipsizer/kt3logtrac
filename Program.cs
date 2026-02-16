using System;
using System.Diagnostics;
using logandtrac.Services;
using logandtrac.Logging;

namespace logandtrac
{
    class Program
    {
        static void Main(string[] args)
        {
            // Настраиваем логирование
            LoggerConfig.Configure();
            
            LoggerConfig.LogInformation("=== ЗАПУСК TASK MANAGER ===");
            LoggerConfig.LogTrace($"Версия: 1.0.0");
            LoggerConfig.LogTrace($"OS: {Environment.OSVersion}");
            LoggerConfig.LogTrace($".NET Version: {Environment.Version}");
            
            var taskManager = new TaskManagerService();
            
            Console.WriteLine("=== TASK MANAGER ===");
            Console.WriteLine("Система управления задачами с логированием");
            ShowHelp();
            
            bool isRunning = true;
            
            while (isRunning)
            {
                Console.Write("\n> ");
                string? input = Console.ReadLine()?.Trim().ToLower();
                
                if (string.IsNullOrEmpty(input))
                {
                    continue;
                }
                
                switch (input)
                {
                    case "add":
                        AddTask(taskManager);
                        break;
                        
                    case "remove":
                        RemoveTask(taskManager);
                        break;
                        
                    case "list":
                        taskManager.ListTasks();
                        break;
                        
                    case "complete":
                        CompleteTask(taskManager);
                        break;
                        
                    case "stats":
                        taskManager.ShowStats();
                        break;
                        
                    case "help":
                        ShowHelp();
                        break;
                        
                    case "exit":
                        isRunning = false;
                        LoggerConfig.LogInformation("Приложение завершает работу по команде пользователя");
                        LoggerConfig.LogTrace($"Время завершения: {DateTime.Now:HH:mm:ss}");
                        Console.WriteLine("\nДо свидания!");
                        Console.WriteLine($"Логи сохранены в директории Logs/");
                        break;
                        
                    default:
                        LoggerConfig.LogWarning($"Неизвестная команда: \"{input}\"");
                        Console.WriteLine($"Неизвестная команда. Введите 'help' для списка команд.");
                        break;
                }
            }
            
            // Принудительно сбрасываем буферы логов
            Trace.Flush();
        }

        static void AddTask(TaskManagerService taskManager)
        {
            Console.Write("Введите название задачи: ");
            string? title = Console.ReadLine();
            
            if (title != null)
            {
                taskManager.AddTask(title);
            }
        }

        static void RemoveTask(TaskManagerService taskManager)
        {
            Console.Write("Введите название задачи для удаления: ");
            string? title = Console.ReadLine();
            
            if (title != null)
            {
                taskManager.RemoveTask(title);
            }
        }

        static void CompleteTask(TaskManagerService taskManager)
        {
            Console.Write("Введите название задачи для отметки выполнения: ");
            string? title = Console.ReadLine();
            
            if (title != null)
            {
                taskManager.CompleteTask(title);
            }
        }


static void ShowHelp()
        {
            Console.WriteLine("\nДоступные команды:");
            Console.WriteLine("  add     - Добавить новую задачу");
            Console.WriteLine("  remove  - Удалить задачу");
            Console.WriteLine("  list    - Показать список задач");
            Console.WriteLine("  complete - Отметить задачу как выполненную");
            Console.WriteLine("  stats   - Показать статистику");
            Console.WriteLine("  help    - Показать это сообщение");
            Console.WriteLine("  exit    - Выход из программы");
        }
    }
}