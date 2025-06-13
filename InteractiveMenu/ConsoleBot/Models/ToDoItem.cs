using InteractiveMenu.Enums;

namespace InteractiveMenu
{
    public class ToDoItem
    {
        public Guid Id { get; set; }
        public ToDoUser User { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public ToDoItemState State { get; set; }
        public DateTime? StateChangedAt { get; set; }

        public ToDoItem(ToDoUser user, string name)
        {
            Id = Guid.NewGuid();
            User = user;
            Name = name;
            CreatedAt = DateTime.UtcNow;
            State = ToDoItemState.Active;
            StateChangedAt = null;
        }
    }
}