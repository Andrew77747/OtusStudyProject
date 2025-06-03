using InteractiveMenu.Enums;
using InteractiveMenu.Exceptions;
using InteractiveMenu.Interfaces;

namespace InteractiveMenu;

public class ToDoService : IToDoService
{
    private readonly List<ToDoItem> _tasks = new();
    
    public static int TasksCount { get; set; }
    public static int TaskLength { get; set; }

    public static int GetTasksCount()
    {
        Console.WriteLine("Введите максимально допустимое количество задач");
        var input = Console.ReadLine();
        Helper.ValidateString(input);
        return Helper.ParseAndValidateInt(input, 1, 100);
    }
        
    public static int GetTaskLength()
    {
        Console.WriteLine("Введите максимально допустимую длину задачи");
        var input = Console.ReadLine();
        Helper.ValidateString(input);
        return Helper.ParseAndValidateInt(input, 1, 100);
    }
    
    public IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId)
    {
        return _tasks.Where(t => t.User.UserId == userId).ToList();
    }

    public IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId)
    {
        return _tasks.Where(t => t.User.UserId == userId && t.State == ToDoItemState.Active).ToList();
    }

    public ToDoItem Add(ToDoUser user, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Некорректное описание задачи");

        if (_tasks.Count >= TasksCount)
            throw new TaskCountLimitException(TasksCount);

        if (name.Length > TaskLength)
            throw new TaskLengthLimitException(name.Length, TaskLength);

        if (_tasks.Any(t => t.User.UserId == user.UserId && t.Name == name))
            throw new DuplicateTaskException(name);

        var newTask = ToDoItem.Create(user, name);
        _tasks.Add(newTask);
        return newTask;
    }

    public void MarkCompleted(Guid id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
            throw new ArgumentException("Задача не найдена");

        task.State = ToDoItemState.Completed;
        task.StateChangedAt = DateTime.UtcNow;
    }

    public void Delete(Guid id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
            throw new ArgumentException("Задача не найдена");

        _tasks.Remove(task);
    }
}