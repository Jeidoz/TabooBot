using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TabooBot.Commands
{
    public sealed class HelpCommand : Command
    {
        public override string[] Triggers => new[] { "/help" };

        public override async Task Execute(Message message)
        {
            const string response = "The list of available commands:\n" +
                                    "/help – Show this message";
            await TabooChatBot.BotClient.SendTextMessageAsync(
                message.Chat.Id,
                response,
                replyToMessageId: message.MessageId);
        }
    }
}