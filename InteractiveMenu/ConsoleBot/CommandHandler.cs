using InteractiveMenu.Exceptions;

namespace InteractiveMenu
{
    public class CommandHandler
    {
        private static readonly List<string?> TaskList = new();
        public static int TasksCount { get; set; }
        public static int TaskLength { get; set; }

        public static string GetGreeting(params string[] commands) =>
            $"Введите любую из доступных команд: {string.Join(", ", commands)}";

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

        public static string? HandleCommand(string? input, ref string? name)
        {
            return input switch
            {
                Commands.Start => HandleStartCommand(out name),
                { } when input.StartsWith(Commands.Echo) => HandleEchoCommand(input, name),
                Commands.Help => HandleHelpCommand(name),
                Commands.Info => HandleInfoCommand(name),
                Commands.AddTask => HandleAddTaskCommand(name),
                Commands.ShowTasks => HandleShowTasksCommand(name),
                Commands.RemoveTask => HandleRemoveTaskCommand(name),
                _ => HandleDefaultCommand(name)
            };
        }
        
        private static string? HandleStartCommand(out string? name)
        {
            Console.WriteLine("Введите свое имя");
            name = Console.ReadLine();
            Console.WriteLine($"Здравствуйте, {name}! Вам доступны команды {Commands.Echo}, " +
                              $"{Commands.AddTask}, {Commands.ShowTasks}, {Commands.RemoveTask}, " +
                              $"{Commands.Help}, {Commands.Info}, {Commands.Exit}");
            return Console.ReadLine();
        }

        private static string? HandleEchoCommand(string input, string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine($"Команда {Commands.Echo} не доступна, " +
                                  $"сначала вы должны запустить приложение с помощью команды {Commands.Start}");
                return Console.ReadLine();
            }

            var argument = input.Length > 5 ? input.Substring(5) : "";
            Console.WriteLine(string.IsNullOrWhiteSpace(argument)
                ? $"Введите текст после команды {Commands.Echo}"
                : argument.Trim());
            return Console.ReadLine();
        }

        private static string? HandleHelpCommand(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Вы можете использовать следующие команды:\n" +
                                  $"{Commands.StartDescription}\n{Commands.HelpDescription}" +
                                  $"\n{Commands.InfoDescription}\n{Commands.ExitDescription}");
            }
            else
            {
                Console.WriteLine($"{name}, вы можете использовать следующие команды:\n" +
                                  $"{Commands.StartDescription}\n{Commands.HelpDescription}\n" +
                                  $"{Commands.InfoDescription}\n{Commands.ExitDescription}\n" +
                                  $"{Commands.EchoDescription}\n{Commands.AddTaskDescription}\n" +
                                  $"{Commands.ShowTasksDescription}\n{Commands.RemoveTaskDescription}");
            }
            return Console.ReadLine();
        }

        private static string? HandleInfoCommand(string? name)
        {
            Console.WriteLine($"{(string.IsNullOrEmpty(name) ? "Версия" : name + ", версия")} 1.0, 05.03.2025");
            return Console.ReadLine();
        }

        private static string? HandleAddTaskCommand(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine($"Команда {Commands.AddTask} не доступна, " +
                                  $"сначала вы должны запустить приложение с помощью команды {Commands.Start}");
                return Console.ReadLine();
            }

            Console.WriteLine("Пожалуйста, введите описание задачи");
            var task = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(task))
            {
                Console.WriteLine("Вы ввели некорректное значение. Введите команду заново");
                return Console.ReadLine();
            }

            if (TaskList.Count == TasksCount)
            {
                throw new TaskCountLimitException(TasksCount);
            }
            
            if (task.Length > TaskLength)
            {
                throw new TaskLengthLimitException(task.Length, TaskLength);
            }
            
            if (TaskList.Contains(task))
            {
                throw new DuplicateTaskException(task);
            }
            
            TaskList.Add(task);
            Console.WriteLine($"Задача \"{task}\" успешно добавлена");
            return Console.ReadLine();
        }

        private static string? HandleShowTasksCommand(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine($"Команда {Commands.ShowTasks} не доступна, " +
                                  $"сначала вы должны запустить приложение с помощью команды {Commands.Start}");
                return Console.ReadLine();
            }

            if (TaskList.Count == 0)
            {
                Console.WriteLine("Список задач пуст - показывать нечего!");
                return Console.ReadLine();
            }

            for (var i = 0; i < TaskList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {TaskList[i]}");
            }

            return Console.ReadLine();
        }

        private static string? HandleRemoveTaskCommand(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine($"Команда {Commands.RemoveTask} не доступна, " +
                                  $"сначала вы должны запустить приложение с помощью команды {Commands.Start}");
                return Console.ReadLine();
            }

            if (TaskList.Count == 0)
            {
                Console.WriteLine("Список задач пуст - удалять нечего!");
                return Console.ReadLine();
            }

            Console.WriteLine("Вот ваш список задач: ");

            for (var i = 0; i < TaskList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {TaskList[i]}");
            }

            Console.WriteLine("Введите номер для удаления задачи");
            var taskNumberFromInput = Console.ReadLine();

            if (!int.TryParse(taskNumberFromInput, out var taskNumber))
            {
                Console.WriteLine($"Ошибка: нужно ввести номер существующей задачи от 1 до {TaskList.Count}. " +
                                  "Введите команду заново");
                return Console.ReadLine();
            }

            if (taskNumber < 1 || taskNumber > TaskList.Count)
            {
                Console.WriteLine($"Ошибка: номер должен быть от 1 до {TaskList.Count}. " +
                                  "Введите команду заново");
                return Console.ReadLine();
            }

            var taskIndexToRemove = taskNumber - 1;
            var taskToRemove = TaskList[taskIndexToRemove];
            TaskList.RemoveAt(taskIndexToRemove);
            Console.WriteLine($"Задача \"{taskToRemove}\" удалена");

            return Console.ReadLine();
        }

        private static string? HandleDefaultCommand(string? name)
        {
            var greetingMessage = GetGreeting(Commands.Start, Commands.Help, Commands.Info, Commands.Exit) +
                                  (string.IsNullOrEmpty(name) ? "" : $", {Commands.Echo}, " +
                                                                     $"{Commands.AddTask}, {Commands.ShowTasks}, " +
                                                                     $"{Commands.RemoveTask}");

            Console.WriteLine(
                $"{(string.IsNullOrEmpty(name) ? "Вы" : name + ", вы")} ввели неправильную команду. {greetingMessage}");
            return Console.ReadLine();
        }
    }
}