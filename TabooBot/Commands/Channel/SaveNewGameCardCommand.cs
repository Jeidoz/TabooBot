using System.Threading.Tasks;
using TabooBot.Extensions;
using Telegram.Bot.Types;

namespace TabooBot.Commands.Channel
{
    public sealed class SaveNewGameCardCommand : Command
    {
        public override string[] Triggers { get; }

        public override bool Contains(Message message)
        {
            return message.Photo != null 
                && message.Chat.Id == TabooChatBot.CardGalleryChatId;
        }

        public override async Task Execute(Message message)
        {
            await Task.Run(() => TabooChatBot.Database.Connection.UpdateCardsAmount(message.MessageId));
        }
    }
}