namespace TabooBot.Data.Models
{
    public sealed class ChatIdToUsernamePair
    {
        // Need to write a user
        public long ChatId { get; set; }
        // Need if user change username
        // UPDATED: Remove someday, because ChatId is same as UserId in private dialogs
        public long UserId { get; set; }
        public string Username { get; set; }
    }
}