using InteractiveMenu.Interfaces;

namespace InteractiveMenu
{
    public class UserService : IUserService
    {
        private readonly Dictionary<long, ToDoUser> _users = new();
        
        public ToDoUser RegisterUser(long telegramUserId, string telegramUserName)
        {
            if (_users.ContainsKey(telegramUserId))
            {
                throw new ArgumentException($"Пользователь с ID {telegramUserId} уже зарегистрирован");
            }

            var newUser = new ToDoUser(telegramUserId, telegramUserName);

            _users.Add(telegramUserId, newUser);
            return newUser;
        }

        public ToDoUser? GetUser(long telegramUserId)
        {
            return _users.TryGetValue(telegramUserId, out var user) ? user : null;
        }
    }
}