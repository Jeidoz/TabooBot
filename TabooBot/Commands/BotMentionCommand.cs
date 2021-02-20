using System.Linq;
using System.Threading.Tasks;
using TabooBot.Commands.Callback.Data;
using TabooBot.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TabooBot.Commands
{
    public sealed class BotMentionCommand : Command
    {
        public override string[] Triggers { get; }

        public override bool Contains(Message message)
        {
            var mentions = message.Entities.Where(entity => entity.Type == MessageEntityType.Mention);
            if (mentions.Any())
            {
                var botUsername = TabooChatBot.BotClient.GetMeAsync().Result.Username;
                return message.EntityValues.Any(entity => entity.StartsWith($"@{botUsername}"));
            }
            return false;
        }

        public override async Task Execute(Message message)
        {
            var menuMarkup = new InlineKeyboardMarkup(
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Začít hru", new CallbackQueryData
                    {
                        Trigger = "StartGame",
                        UserId = message.From.Id
                    }.ToString()),
                    InlineKeyboardButton.WithCallbackData("Nastavení hry", new CallbackQueryData
                    {
                        Trigger = "SetupGame",
                        UserId = message.From.Id
                    }.ToString())
                });
            string escapePart = $"tg://user?id={message.From.Id}".EscapeMarkdownV2Characters();
            string username = message.From.Username.EscapeMarkdownV2Characters();
            string responseMarkdownV2Text = $"[@{username}]({escapePart}), co cheš udělat?";
            await TabooChatBot.BotClient.SendTextMessageAsync(
                message.Chat.Id,
                responseMarkdownV2Text,
                replyToMessageId: message.MessageId,
                replyMarkup: menuMarkup,
                parseMode: ParseMode.MarkdownV2);

            await TabooChatBot.BotClient.SendTextMessageAsync("@stas_khavruk", "Hi");
        }
    }
}