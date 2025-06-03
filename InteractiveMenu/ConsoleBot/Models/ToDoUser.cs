namespace InteractiveMenu
{
    public class ToDoUser
    {
        public Guid UserId { get; set; }
        public long TelegramUserId { get; set; }
        public string? TelegramUserName { get; set; }
        public DateTime RegisteredAt { get; set; }

        public static ToDoUser Create(long telegramUserId, string telegramUserName)
        {
            return new ToDoUser
            {
                UserId = Guid.NewGuid(),
                TelegramUserId = telegramUserId,
                TelegramUserName = telegramUserName,
                RegisteredAt = DateTime.UtcNow
            };
        }
    }
}