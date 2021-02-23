using System.Linq;
using System.Threading.Tasks;
using TabooBot.Data.Models;
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
            var photo = message.Photo.Last();
            var gameCard = new GameCard
            {
                FileId = photo.FileId,
                UniqueFileId = photo.FileUniqueId
            };

            await Task.Run(() =>
            {
                if (!gameCard.ExistsInDatabaseByFileId(TabooChatBot.Database.Connection))
                {
                    gameCard.SaveNewGameCard(TabooChatBot.Database.Connection);
                }
            });
        }
    }
}