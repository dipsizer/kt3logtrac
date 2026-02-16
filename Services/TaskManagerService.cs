using System;
using System.Collections.Generic;
using System.Linq;
using logandtrac.Models;
using logandtrac.Logging;

namespace logandtrac.Services
{
    public class TaskManagerService
    {
        private List<TaskItem> tasks = new List<TaskItem>();

        public void AddTask(string title)
        {
            LoggerConfig.LogOperationStart("AddTask");
            LoggerConfig.LogTrace($"Проверка названия: \"{title}\"");

            if (string.IsNullOrWhiteSpace(title))
            {
                LoggerConfig.LogWarning("Попытка добавить задачу с пустым названием");
                Console.WriteLine("Ошибка: Название задачи не может быть пустым!");
                LoggerConfig.LogOperationEnd("AddTask", "Отменено - пустое название");
                return;
            }

            if (tasks.Any(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase)))
            {
                LoggerConfig.LogWarning($"Попытка добавить дубликат задачи: \"{title}\"");
                Console.WriteLine("Ошибка: Задача с таким названием уже существует!");
                LoggerConfig.LogOperationEnd("AddTask", "Отменено - дубликат");
                return;
            }

            var task = new TaskItem(title);
            tasks.Add(task);
            
            LoggerConfig.LogInformation($"Задача \"{title}\" успешно добавлена. ID: {task.Id}");
            LoggerConfig.LogTrace($"Всего задач после добавления: {tasks.Count}");
            LoggerConfig.LogOperationEnd("AddTask", "Успешно");
            
            Console.WriteLine($"✓ Задача \"{title}\" успешно добавлена!");
        }

        public void RemoveTask(string title)
        {
            LoggerConfig.LogOperationStart("RemoveTask");
            LoggerConfig.LogTrace($"Поиск задачи для удаления: \"{title}\"");

            var task = tasks.FirstOrDefault(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (task == null)
            {
                LoggerConfig.LogError($"Задача \"{title}\" не найдена для удаления");
                Console.WriteLine($"Ошибка: Задача \"{title}\" не найдена!");
                LoggerConfig.LogOperationEnd("RemoveTask", "Ошибка - задача не найдена");
                return;
            }

            tasks.Remove(task);
            LoggerConfig.LogInformation($"Задача \"{title}\" успешно удалена");
            LoggerConfig.LogTrace($"Всего задач после удаления: {tasks.Count}");
            LoggerConfig.LogOperationEnd("RemoveTask", "Успешно");
            
            Console.WriteLine($"✓ Задача \"{title}\" удалена!");
        }

        public void ListTasks()
        {
            LoggerConfig.LogOperationStart("ListTasks");
            LoggerConfig.LogTrace($"Текущее количество задач: {tasks.Count}");

            Console.WriteLine("\n=== СПИСОК ЗАДАЧ ===");
            
            if (tasks.Count == 0)
            {
                LoggerConfig.LogInformation("Список задач пуст");
                Console.WriteLine("Список задач пуст.");
            }
            else
            {
                for (int i = 0; i < tasks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {tasks[i]}");
                }
                LoggerConfig.LogInformation($"Выведено задач: {tasks.Count}");
            }
            
            LoggerConfig.LogOperationEnd("ListTasks");
        }
        public void CompleteTask(string title)
        {
            LoggerConfig.LogOperationStart("CompleteTask");
            LoggerConfig.LogTrace($"Поиск задачи для завершения: \"{title}\"");
            
            var task = tasks.FirstOrDefault(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            
            if (task == null)
            {
                LoggerConfig.LogError($"Задача \"{title}\" не найдена для завершения");
                Console.WriteLine($"Ошибка: Задача \"{title}\" не найдена!");
                LoggerConfig.LogOperationEnd("CompleteTask", "Ошибка - задача не найдена");
                return;
            }
            
            if (task.IsCompleted)
            {
                LoggerConfig.LogWarning($"Задача \"{title}\" уже была выполнена");
                Console.WriteLine($"Задача \"{title}\" уже отмечена как выполненная!");
                LoggerConfig.LogOperationEnd("CompleteTask", "Уже выполнена");
                return;
            }
            
            task.IsCompleted = true;
            LoggerConfig.LogInformation($"Задача \"{title}\" отмечена как выполненная");
            LoggerConfig.LogOperationEnd("CompleteTask", "Успешно");
            
            Console.WriteLine($"✓ Задача \"{title}\" отмечена как выполненная!");
        }

        public void ShowStats()
        {
            LoggerConfig.LogOperationStart("ShowStats");
            
            int completed = tasks.Count(t => t.IsCompleted);
            int active = tasks.Count - completed;
            
            Console.WriteLine($"\n=== СТАТИСТИКА ===");
            Console.WriteLine($"Всего задач: {tasks.Count}");
            Console.WriteLine($"Активных: {active}");
            Console.WriteLine($"Завершённых: {completed}");
            Console.WriteLine($"Процент выполнения: {(tasks.Count > 0 ? (completed * 100 / tasks.Count) : 0)}%");
            
            LoggerConfig.LogInformation($"Статистика: всего={tasks.Count}, активных={active}, завершено={completed}");
            LoggerConfig.LogOperationEnd("ShowStats");
        }
    }
}