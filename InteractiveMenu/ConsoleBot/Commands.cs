namespace InteractiveMenu
{
    public static class Commands
    {
        public const string Start = "/start";
        public const string Help = "/help";
        public const string Info = "/info";
        public const string Exit = "/exit";
        public const string AddTask = "/addtask";
        public const string ShowTasks = "/showtasks";
        public const string RemoveTask = "/removetask";
        public const string CompleteTask = "/completetask";
        public const string ShowAllTasks = "/showalltasks";

        public const string StartDescription =
            $"{Start} - используется для начала работы с ботом";
        public const string HelpDescription =
            $"{Help} - предоставляет информацию о других командах, " +
            "которые используются для взаимодействия с ботом";
        public const string InfoDescription = $"{Info} - предоставляет информацию о версии программы";
        public const string ExitDescription = $"{Exit} - используется для выхода из программы";
        public const string AddTaskDescription = $"{AddTask} - позволяет добавить задачу в список";
        public const string ShowTasksDescription = 
            $"{ShowTasks} - позволяет посмотреть все активные задачи";
        public const string RemoveTaskDescription = 
            $"{RemoveTask} - используется для удаления задач в списке";
        public const string CompleteTaskDescription = 
            $"{CompleteTask} - используется для для завершения активных задач";
        public const string ShowAllTasksDescription = 
            $"{ShowAllTasks} - позволяет посмотреть все задачи: активные и завершенные";
    }
}