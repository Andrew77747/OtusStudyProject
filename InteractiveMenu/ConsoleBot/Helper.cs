using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;

namespace InteractiveMenu
{
    public static class Helper
    {
        public static int ParseAndValidateInt(string? str, int min, int max)
        {
            if (!int.TryParse(str, out var count))
            {
                throw new ArgumentException("Введено не число");
            }
            
            if (count < min || count > max)
            {
                throw new ArgumentException("Введенное число должно быть в диапазоне от 1 до 100");
            }

            return count;
        }

        public static void ValidateString(string? str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentException("Ввод не может быть пустым.");
            }
        }

        public static bool IsUserRegistered(ITelegramBotClient botClient, Chat chat, ToDoUser? user)
        {
            if (user != null) 
                return true;
            botClient.SendMessage(chat, "Пожалуйста, зарегистрируйтесь через /start"); 
            return false;
        }
    }
}