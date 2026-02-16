using System;
using System.Collections.Generic;
using System.Linq;
using logandtrac.Models;
using logandtrac.Logging;
using Serilog;

namespace logandtrac.Services;

public class TaskManagerService
{
    private List<TaskItem> tasks = new List<TaskItem>();
    private readonly ILogger _logger;

    public TaskManagerService(ILogger logger)
    {
        _logger = logger.ForContext<TaskManagerService>();
    }

    public void AddTask(string title)
    {
        _logger.LogOperationStart("AddTask");
        _logger.Debug("Проверка названия: {Title}", title);

        if (string.IsNullOrWhiteSpace(title))
        {
            _logger.Warning("Попытка добавить задачу с пустым названием");
            Console.WriteLine("Ошибка: Название задачи не может быть пустым!");
            _logger.LogOperationEnd("AddTask", "Отменено - пустое название");
            return;
        }

        if (tasks.Any(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase)))
        {
            _logger.Warning("Попытка добавить дубликат задачи: {Title}", title);
            Console.WriteLine("Ошибка: Задача с таким названием уже существует!");
            _logger.LogOperationEnd("AddTask", "Отменено - дубликат");
            return;
        }

        var task = new TaskItem(title);
        tasks.Add(task);
        
        _logger.Information("Задача {Title} успешно добавлена. ID: {TaskId}", title, task.Id);
        _logger.Debug("Всего задач после добавления: {TaskCount}", tasks.Count);
        _logger.LogOperationEnd("AddTask", "Успешно");
        
        Console.WriteLine($"✓ Задача \"{title}\" успешно добавлена!");
    }

    public void RemoveTask(string title)
    {
        _logger.LogOperationStart("RemoveTask");
        _logger.Debug("Поиск задачи для удаления: {Title}", title);

        var task = tasks.FirstOrDefault(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        if (task == null)
        {
            _logger.Error("Задача {Title} не найдена для удаления", title);
            Console.WriteLine($"Ошибка: Задача \"{title}\" не найдена!");
            _logger.LogOperationEnd("RemoveTask", "Ошибка - задача не найдена");
            return;
        }

        tasks.Remove(task);
        _logger.Information("Задача {Title} успешно удалена", title);
        _logger.Debug("Всего задач после удаления: {TaskCount}", tasks.Count);
        _logger.LogOperationEnd("RemoveTask", "Успешно");
        
        Console.WriteLine($"✓ Задача \"{title}\" удалена!");
    }

    public void ListTasks()
    {
        _logger.LogOperationStart("ListTasks");
        _logger.Debug("Текущее количество задач: {TaskCount}", tasks.Count);

        Console.WriteLine("\n=== СПИСОК ЗАДАЧ ===");
        
        if (tasks.Count == 0)
        {
            _logger.Information("Список задач пуст");
            Console.WriteLine("Список задач пуст.");
        }
        else
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i]}");
            }
            _logger.Information("Выведено задач: {TaskCount}", tasks.Count);
        }
        
        _logger.LogOperationEnd("ListTasks");
    }

    public void CompleteTask(string title)
    {
        _logger.LogOperationStart("CompleteTask");
        _logger.Debug("Поиск задачи для завершения: {Title}", title);
        
        var task = tasks.FirstOrDefault(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        
        if (task == null)
        {
            _logger.Error("Задача {Title} не найдена для завершения", title);
            Console.WriteLine($"Ошибка: Задача \"{title}\" не найдена!");
            _logger.LogOperationEnd("CompleteTask", "Ошибка - задача не найдена");
            return;
        }
        
        if (task.IsCompleted)
        {
            _logger.Warning("Задача {Title} уже была выполнена", title);
            Console.WriteLine($"Задача \"{title}\" уже отмечена как выполненная!");
            _logger.LogOperationEnd("CompleteTask", "Уже выполнена");
            return;
        }
        
        task.IsCompleted = true;
        _logger.Information("Задача {Title} отмечена как выполненная", title);
        _logger.LogOperationEnd("CompleteTask", "Успешно");
        
        Console.WriteLine($"✓ Задача \"{title}\" отмечена как выполненная!");
    }

    public void ShowStats()
    {
        _logger.LogOperationStart("ShowStats");
        
        int completed = tasks.Count(t => t.IsCompleted);
        int active = tasks.Count - completed;
        int percentComplete = tasks.Count > 0 ? (completed * 100 / tasks.Count) : 0;
        
        Console.WriteLine($"\n=== СТАТИСТИКА ===");
        Console.WriteLine($"Всего задач: {tasks.Count}");
        Console.WriteLine($"Активных: {active}");
        Console.WriteLine($"Завершённых: {completed}");
        Console.WriteLine($"Процент выполнения: {percentComplete}%");
        
        _logger.Information("Статистика: всего={Total}, активных={Active}, завершено={Completed}, процент={Percent}%", 
            tasks.Count, active, completed, percentComplete);
        _logger.LogOperationEnd("ShowStats");
    }
}