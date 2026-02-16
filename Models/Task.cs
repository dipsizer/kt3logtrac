using System;

namespace logandtrac.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsCompleted { get; set; }

    public TaskItem(string title)
    {
        Id = new Random().Next(1000, 9999);
        Title = title;
        CreatedAt = DateTime.Now;
        IsCompleted = false;
    }

    public override string ToString()
    {
        string status = IsCompleted ? "✓" : "○";
        return $"[{Id}] {Title} - {status} ({CreatedAt:dd.MM.yyyy HH:mm})";
    }
}