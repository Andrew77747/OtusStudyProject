using Otus.ToDoList.ConsoleBot;

namespace InteractiveMenu
{
    public class Program
    {
        private static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    var settings = ToDoConfigurator.GetSettings();
                    var toDoService = new ToDoService(settings.tasksCount, settings.taskLength);
                    var botClient = new ConsoleBotClient();
                    var userService = new UserService();
                    var handler = new UpdateHandler(userService, toDoService);

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