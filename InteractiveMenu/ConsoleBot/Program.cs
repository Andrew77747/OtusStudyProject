using InteractiveMenu.Exceptions;

namespace InteractiveMenu
{
    public class Program
    {
        private static void Main(string[] args)
        {
            string? name = null;
            string? input = null;
            var tasksCountSet = false;
            var taskLengthSet = false;

            while (true)
            {
                try
                {
                    if (!tasksCountSet)
                    {
                        CommandHandler.TasksCount = CommandHandler.GetTasksCount();
                        tasksCountSet = true;
                    }
                    
                    if (!taskLengthSet)
                    {
                        CommandHandler.TaskLength = CommandHandler.GetTaskLength();
                        taskLengthSet = true;
                    }
                    
                    if (tasksCountSet && taskLengthSet && input == null)
                    {
                        Console.WriteLine(CommandHandler.GetGreeting(Commands.Start, 
                            Commands.Help, Commands.Info, Commands.Exit));
                        input = Console.ReadLine();
                    }

                    if (input == Commands.Exit)
                    {
                        break;
                    }
                    
                    input = CommandHandler.HandleCommand(input, ref name);
                }
                catch (TaskCountLimitException e)
                {
                    Console.WriteLine(e.Message);
                    input = Console.ReadLine();
                }
                catch (TaskLengthLimitException e)
                {
                    Console.WriteLine(e.Message);
                    input = Console.ReadLine();
                }
                catch (DuplicateTaskException e)
                {
                    Console.WriteLine(e.Message);
                    input = Console.ReadLine();
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    if (!tasksCountSet || !taskLengthSet)
                    {
                        continue;
                    }
                    input = Console.ReadLine();
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