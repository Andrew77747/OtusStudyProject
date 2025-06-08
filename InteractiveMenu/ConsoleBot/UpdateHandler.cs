using InteractiveMenu.Exceptions;
using InteractiveMenu.Interfaces;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;

namespace InteractiveMenu
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly IUserService _userService;
        private readonly IToDoService _toDoService;

        public UpdateHandler(IUserService userService, IToDoService toDoService)
        {
            _userService = userService;
            _toDoService = toDoService;
        }

        public void HandleUpdateAsync(ITelegramBotClient botClient, Update update)
        {
            var input = update.Message.Text;
            var chat = update.Message.Chat;
            var from = update.Message.From;

            try
            {
                var currentUser = _userService.GetUser(from.Id);
                
                switch (input)
                {
                    case Commands.Start:
                        HandleStartCommand(botClient, chat, from, currentUser);
                        break;
                    case Commands.Help:
                        HandleHelpCommand(botClient, chat);
                        break;
                    case Commands.Info:
                        HandleInfoCommand(botClient, chat);
                        break;
                    case { } when input.StartsWith(Commands.AddTask):
                        HandleAddTaskCommand(botClient, chat, input, currentUser);
                        break;
                    case Commands.ShowTasks:
                        HandleShowActiveTasksCommand(botClient, chat, currentUser);
                        break;
                    case Commands.ShowAllTasks:
                        HandleShowAllTasksCommand(botClient, chat, currentUser);
                        break;
                    case { } when input.StartsWith(Commands.RemoveTask):
                        HandleRemoveTaskCommand(botClient, chat, input, currentUser);
                        break;
                    case { } when input.StartsWith(Commands.CompleteTask):
                        HandleCompleteTaskCommand(botClient, chat, input, currentUser);
                        break;
                    case Commands.Exit:
                        HandleExitCommand(botClient, chat);
                        break;
                    default:
                        HandleDefaultCommand(botClient, chat, currentUser);
                        break;
                }
            }
            catch (TaskCountLimitException e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            catch (TaskLengthLimitException e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            catch (DuplicateTaskException e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        private void HandleStartCommand(ITelegramBotClient botClient, Chat chat, User from, ToDoUser? currentUser)
        {
            if (currentUser == null)
            {
                _userService.RegisterUser(from.Id, from.Username!);
                botClient.SendMessage(chat, "Вы зарегистрированы!\nВам доступны команды:" +
                                            $"\n{string.Join(", ", 
                Commands.AddTask, Commands.ShowTasks, Commands.RemoveTask, Commands.Help, 
                Commands.Info, Commands.CompleteTask, Commands.CompleteTask, 
                Commands.ShowAllTasks, Commands.Exit)}");
            }
            else
            {
                botClient.SendMessage(chat, "Вы уже зарегистрированы!");
            }
        }
        
        private void HandleHelpCommand(ITelegramBotClient botClient, Chat chat)
        {
            botClient.SendMessage(chat, "Информация о командах:" +
                                        $"\n{Commands.StartDescription}\n{Commands.HelpDescription}" +
                                        $"\n{Commands.InfoDescription}\n{Commands.ExitDescription}" +
                                        $"\n{Commands.AddTaskDescription}\n{Commands.ShowTasksDescription}" +
                                        $"\n{Commands.RemoveTaskDescription}\n{Commands.CompleteTaskDescription}" +
                                        $"\n{Commands.ShowAllTasksDescription}");
        }

        private void HandleInfoCommand(ITelegramBotClient botClient, Chat chat)
        {
            botClient.SendMessage(chat, "Версия бота 1.0");
        }
        
        private void HandleAddTaskCommand(ITelegramBotClient botClient, Chat chat, string input, ToDoUser? currentUser)
        {
            if (!Helper.IsUserRegistered(botClient, chat, currentUser)) 
                return;

            var taskDescription = input.Substring(Commands.AddTask.Length).Trim();
            
            if (string.IsNullOrWhiteSpace(taskDescription))
            {
                botClient.SendMessage(chat, "Ошибка: вы не указали описание задачи");
                return;
            }

            var task = _toDoService.Add(currentUser!, taskDescription);
            botClient.SendMessage(chat, $"Задача добавлена: {task.Name}");
        }

        private void HandleShowActiveTasksCommand(ITelegramBotClient botClient, Chat chat, ToDoUser? currentUser)
        {
            if (!Helper.IsUserRegistered(botClient, chat, currentUser)) 
                return;

            var tasks = _toDoService.GetActiveByUserId(currentUser!.UserId);
            
            if (tasks.Count == 0)
            {
                botClient.SendMessage(chat, "Список задач пуст");
                return;
            }
            
            var tasksInfo = tasks
                .Select(task => $"{task.Name} - {task.CreatedAt} - {task.Id}").ToList();
            var message = "Ваши задачи:\n" + string.Join("\n", tasksInfo);
            botClient.SendMessage(chat, message);
        }
        
        private void HandleShowAllTasksCommand(ITelegramBotClient botClient, Chat chat, ToDoUser? currentUser)
        {
            if (!Helper.IsUserRegistered(botClient, chat, currentUser)) 
                return;

            var tasks = _toDoService.GetAllByUserId(currentUser!.UserId);
            
            if (tasks.Count == 0)
            {
                botClient.SendMessage(chat, "Список задач пуст");
                return;
            }
            
            var tasksInfo = tasks
                .Select(task => $"({task.State}) {task.Name} - {task.CreatedAt} - {task.Id}").ToList();
            var message = "Ваши задачи:\n" + string.Join("\n", tasksInfo);
            botClient.SendMessage(chat, message);
        }

        private void HandleRemoveTaskCommand(ITelegramBotClient botClient, Chat chat, string input, ToDoUser? currentUser)
        {
            if (!Helper.IsUserRegistered(botClient, chat, currentUser)) 
                return;

            var tasks = _toDoService.GetActiveByUserId(currentUser!.UserId).ToList();
            
            if (tasks.Count == 0)
            {
                botClient.SendMessage(chat, "Список задач пуст");
                return;
            }

            var taskToRemoveIndex = input.Substring(Commands.RemoveTask.Length).Trim();

            if (!int.TryParse(taskToRemoveIndex, out var taskNumber) || taskNumber < 1 || taskNumber > tasks.Count)
            {
                botClient.SendMessage(chat, $"Некорректный номер. Введите число от 1 до {tasks.Count}");
                return;
            }

            var taskToRemove = tasks[taskNumber - 1];
            _toDoService.Delete(taskToRemove.Id);
            botClient.SendMessage(chat, $"Задача удалена: {taskToRemove.Name}");
        }
        
        private void HandleCompleteTaskCommand(ITelegramBotClient botClient, Chat chat, string input, ToDoUser? currentUser)
        {
            if (!Helper.IsUserRegistered(botClient, chat, currentUser))
                return;

            var tasks = _toDoService.GetActiveByUserId(currentUser!.UserId).ToList();
            
            if (tasks.Count == 0)
            {
                botClient.SendMessage(chat, "Список активных задач пуст");
                return;
            }
            
            var taskIdString = input.Substring(Commands.CompleteTask.Length).Trim();
            
            if (!Guid.TryParse(taskIdString, out var taskId))
            {
                botClient.SendMessage(chat, "Некорректный формат ID. " +
                                            "Используйте GUID (например: 3fa85f64-5717-4562-b3fc-2c963f66afa6)");
                return;
            }
            
            var taskToComplete = tasks.FirstOrDefault(t => t.Id == taskId);
            if (taskToComplete == null)
            {
                botClient.SendMessage(chat, "Задача не найдена или уже завершена");
                return;
            }
            
            _toDoService.MarkCompleted(taskId);
            botClient.SendMessage(chat, $"Задача: {taskToComplete.Name} завершена");
        }
        
        private void HandleExitCommand(ITelegramBotClient botClient, Chat chat)
        {
            botClient.SendMessage(chat, "Завершение работы...");
            Environment.Exit(0);
        }

        private void HandleDefaultCommand(ITelegramBotClient botClient, Chat chat, ToDoUser? currentUser)
        {
            var commands = currentUser != null
                ? new[] { Commands.AddTask, Commands.ShowTasks, Commands.RemoveTask, Commands.Help, 
                    Commands.Info, Commands.Exit, Commands.CompleteTaskDescription, Commands.ShowAllTasksDescription }
                : new[] { Commands.Start, Commands.Help, Commands.Info, Commands.Exit };
            
            botClient.SendMessage(chat, $"Вы ввели неизвестную команду! " +
                                        $"Доступные команды: {string.Join(", ", commands)}");
        }
    }
}