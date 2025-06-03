using InteractiveMenu.Exceptions;
using Otus.ToDoList.ConsoleBot;

namespace InteractiveMenu
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var tasksCountSet = false;
            var taskLengthSet = false;
            
            var userService = new UserService();
            var toDoService = new ToDoService();
            var botClient = new ConsoleBotClient();
            var handler = new UpdateHandler(userService, toDoService);

            while (true)
            {
                try
                {
                    if (!tasksCountSet)
                    {
                        ToDoService.TasksCount = ToDoService.GetTasksCount();
                        tasksCountSet = true;
                    }
                    
                    if (!taskLengthSet)
                    {
                        ToDoService.TaskLength = ToDoService.GetTaskLength();
                        taskLengthSet = true;
                    }

                    botClient.StartReceiving(handler);
                }

                catch (Exception e)
                {
                    Console.WriteLine("Произошла непредвиденная ошибка:");
                    Console.WriteLine(e.GetType().Name);
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                
                    if (e.InnerException != null)
                    {
                        Console.WriteLine("Внутреннее исключение:");
                        Console.WriteLine(e.InnerException.GetType().Name);
                        Console.WriteLine(e.InnerException.Message);
                    }
                    return;
                }
            }
        }
    }
}