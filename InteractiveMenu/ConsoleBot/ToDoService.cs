using InteractiveMenu.Enums;
using InteractiveMenu.Exceptions;
using InteractiveMenu.Interfaces;

namespace InteractiveMenu;

public class ToDoService : IToDoService
{
    private readonly int _maxTasks;
    private readonly int _maxLength;
    private readonly List<ToDoItem> _tasks = new();

    public ToDoService(int maxTasks, int maxLength)
    {
        _maxTasks = maxTasks;
        _maxLength = maxLength;
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

        if (_tasks.Count >= _maxTasks)
            throw new TaskCountLimitException(_maxTasks);

        if (name.Length > _maxLength)
            throw new TaskLengthLimitException(name.Length, _maxLength);

        if (_tasks.Any(t => t.User.UserId == user.UserId && t.Name == name))
            throw new DuplicateTaskException(name);

        var newTask = new ToDoItem(user, name);
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