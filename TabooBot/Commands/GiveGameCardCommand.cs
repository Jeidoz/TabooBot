using System;
using System.Threading.Tasks;
using TabooBot.Extensions;
using Telegram.Bot.Types;

namespace TabooBot.Commands
{
    public sealed class GiveGameCardCommand : Command
    {
        private readonly Random _rand = new Random();

        public override string[] Triggers => new[] { "/card" };

        public override async Task Execute(Message message)
        {
            var randUpTo = TabooChatBot.Database.Connection.GetCardsAmount();

            Message response = null;
            while (response is null)
            {
                try
                {
                    response = await TabooChatBot.BotClient.ForwardMessageAsync(
                        message.Chat.Id,
                        TabooChatBot.CardGalleryChatId,
                        _rand.Next(2, randUpTo + 1));
                }
                catch 
                {
                    // We randomed not suitable message for forwarding ¯\_(ツ)_/¯
                    // Rerand
                }
            }
        }
    }
}