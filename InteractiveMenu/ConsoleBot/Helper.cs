using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;

namespace InteractiveMenu
{
    public static class Helper
    {
        public static bool IsUserRegistered(ITelegramBotClient botClient, Chat chat, ToDoUser? user)
        {
            if (user != null) 
                return true;
            botClient.SendMessage(chat, "Пожалуйста, зарегистрируйтесь через /start"); 
            return false;
        }
    }
}