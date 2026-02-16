using System;
using logandtrac.Services;
using logandtrac.Logging;
using Serilog;

namespace logandtrac;

class Program
{
    static void Main(string[] args)
    {
        // Настраиваем логгер
        var logger = LoggerConfig.Configure();
        
        try
        {
            var taskManager = new TaskManagerService(logger);
            
            Console.WriteLine("=== TASK MANAGER ===");
            Console.WriteLine("Система управления задачами с логированием (Serilog)");
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
                        logger.Information("Приложение завершает работу по команде пользователя");
                        Console.WriteLine("\nДо свидания!");
                        Console.WriteLine($"Логи сохранены в директории Logs/");
                        break;
                        
                    default:
                        logger.Warning("Неизвестная команда: {Command}", input);
                        Console.WriteLine($"Неизвестная команда. Введите 'help' для списка команд.");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.Fatal(ex, "Критическая ошибка в приложении");
            Console.WriteLine($"Произошла критическая ошибка: {ex.Message}");
        }
        finally
        {
            // Явно закрываем логгер
            Log.CloseAndFlush();
        }
    }

    static void AddTask(TaskManagerService taskManager)
    {
        Console.Write("Введите название задачи: ");
        string? title = Console.ReadLine();
        
        if (!string.IsNullOrEmpty(title))
        {
            taskManager.AddTask(title);
        }
    }

    static void RemoveTask(TaskManagerService taskManager)
    {
        Console.Write("Введите название задачи для удаления: ");
        string? title = Console.ReadLine();
        
        if (!string.IsNullOrEmpty(title))
        {
            taskManager.RemoveTask(title);
        }
    }

    static void CompleteTask(TaskManagerService taskManager)
    {
        Console.Write("Введите название задачи для отметки выполнения: ");
        string? title = Console.ReadLine();
        
        if (!string.IsNullOrEmpty(title))
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