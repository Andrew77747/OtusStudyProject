namespace InteractiveMenu
{
    public static class Commands
    {
        public const string Start = "/start";
        public const string Help = "/help";
        public const string Info = "/info";
        public const string Exit = "/exit";
        public const string Echo = "/echo";
        public const string AddTask = "/addtask";
        public const string ShowTasks = "/showtasks";
        public const string RemoveTask = "/removetask";

        public const string StartDescription =
            $"{Start} - используется для начала работы с ботом. После выполнения команды, " +
            "вы должны ввести свое имя для продолжения работы с ботом";
        public const string HelpDescription =
            $"{Help} - предоставляет информацию о других командах, " +
            "которые используются для взаимодействия с ботом";
        public const string InfoDescription = $"{Info} - предоставляет информацию о версии программы";
        public const string ExitDescription = $"{Exit} - используется для выхода из программы";
        public const string EchoDescription = $"{Echo} - используется для отображения введенного слова";
        public const string AddTaskDescription = $"{AddTask} - позволяет добавить задачу в список";
        public const string ShowTasksDescription = 
            $"{ShowTasks} - позволяет посмотреть все задачи, которые были в список";
        public const string RemoveTaskDescription = 
            $"{RemoveTask} - используется для удаления задач в списке";
    }
}