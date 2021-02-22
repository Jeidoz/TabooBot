using System.Threading.Tasks;
using TabooBot.Data.Models;
using TabooBot.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TabooBot.Commands
{
    public sealed class HiCommand : Command
    {
        public override string[] Triggers { get; }

        public override bool Contains(Message message)
        {
            return message.Chat.Type == ChatType.Private && message.Text.StartsWith("/start");
        }

        public override async Task Execute(Message message)
        {
            string responseText = $"Ahoj! Když vás někdo pozve do hry, upozorním vás na to!";
            await TabooChatBot.BotClient.SendTextMessageAsync(message.Chat.Id, responseText,
                replyToMessageId: message.MessageId);

            var newPair = new ChatIdToUsernamePair
            {
                ChatId = message.Chat.Id,
                UserId = message.From.Id,
                Username = message.From.Username
            };
            if (!newPair.ExistsInDatabaseByUserId(TabooChatBot.Database.Connection))
            {
                newPair.SaveAsNewPair(TabooChatBot.Database.Connection);
            }
            // TODO: IF USERNAME CHANGES UPDATE (bot diaglog restart)
        }
    }
}
