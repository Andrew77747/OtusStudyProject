namespace InteractiveMenu
{
    public class ConsoleBot
    {
        public static string GetGreeting(params string[] commands) =>
            $"Введите любую из доступных команд: {string.Join(", ", commands)}";

        public static void StartMenu()
        {
            string? name = null;
            string? input = Console.ReadLine();

            while (input != Commands.Exit)
            {
                switch (input)
                {
                    case Commands.Start:
                        Console.WriteLine("Введите свое имя");
                        name = Console.ReadLine();
                        Console.WriteLine($"Здравствуйте, {name}! Вам доступна команда {Commands.Echo}");
                        input = Console.ReadLine();
                        break;

                    case string cmd when cmd.StartsWith(Commands.Echo):
                        if (string.IsNullOrEmpty(name))
                        {
                            Console.WriteLine($"Команда {Commands.Echo} не доступна, " +
                                              $"сначала вы должны запустить приложение с помощью команды {Commands.Start}");
                            input = Console.ReadLine();
                            break;
                        }

                        var argument = input.Length > 5 
                            ? input.Substring(5) 
                            : "";
                        Console.WriteLine(string.IsNullOrWhiteSpace(argument)
                            ? $"Введите текст после команды {Commands.Echo}"
                            : argument.Trim());
                        input = Console.ReadLine();
                        break;

                    case Commands.Help:
                        Console.WriteLine($"{(string.IsNullOrEmpty(name) 
                            ? "Здесь" 
                            : name + ", здесь")} вы можете ознакомиться с информацией о том, как пользоваться программой");
                        input = Console.ReadLine();
                        break;

                    case Commands.Info:
                        Console.WriteLine($"{(string.IsNullOrEmpty(name) 
                            ? "Версия" 
                            : name + ", версия")} 1.0, 05.03.2025");
                        input = Console.ReadLine();
                        break;

                    default:
                        var greetingMessage = GetGreeting(Commands.Start, Commands.Help, Commands.Info, Commands.Exit) + (string.IsNullOrEmpty(name) 
                            ? "" 
                            : $", {Commands.Echo}");

                        Console.WriteLine($"{(string.IsNullOrEmpty(name) 
                            ? "Вы" 
                            : name + ", вы")} ввели неправильную команду. {greetingMessage}");
                        input = Console.ReadLine();
                        break;
                }
            }
        }
    }
}
