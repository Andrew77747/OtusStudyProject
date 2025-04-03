namespace InteractiveMenu
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(ConsoleBot.GetGreeting(Commands.Start, Commands.Help, Commands.Info, Commands.Exit));
            ConsoleBot.StartMenu();
        }
    }
}
