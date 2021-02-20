using TabooBot.Commands;
using TabooBot.Commands.Callback;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TabooBot
{
    public sealed class TabooChatBot : IHostedService
    {
        public static ITelegramBotClient BotClient;

        private readonly List<Command> _commands;
        private readonly List<CallbackCommand> _callbackCommands;

        #region Initialization
        public TabooChatBot()
        {
            InitializeListOfBotCommands(out _commands);
            InitializeListOfBotCallbackCommands(out _callbackCommands);
            InitializeBotClientInstance(Program.Configuration["BotToken"]);

            BotClient.OnMessage += OnMessage;
            BotClient.OnMessageEdited += OnMessage;
            //BotClient.OnCallbackQuery += OnCallbackQuery;
        }

        private void InitializeListOfBotCommands(out List<Command> commands)
        {
            var types = GetAllTypesInTheNamespace("TabooBot.Commands")
                .Where(type => type.IsSubclassOf(typeof(Command)))
                .ToArray();
            commands = new List<Command>(types.Length);
            foreach (var type in types)
            {
                commands.Add((Command)Activator.CreateInstance(type));
            }
        }

        private void InitializeListOfBotCallbackCommands(out List<CallbackCommand> commands)
        {
            var types = GetAllTypesInTheNamespace("TabooBot.Commands.Callback")
                .Where(type => type.IsSubclassOf(typeof(CallbackCommand)))
                .ToArray();
            commands = new List<CallbackCommand>(types.Length);
            foreach (var type in types)
            {
                commands.Add((CallbackCommand)Activator.CreateInstance(type));
            }
        }

        private Type[] GetAllTypesInTheNamespace(string @namespace)
        {
            return Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(type => type.Namespace == @namespace)
                .ToArray();
        }

        private static void InitializeBotClientInstance(string botToken)
        {
            BotClient = new TelegramBotClient(botToken);
        }
        #endregion

        private async void OnMessage(object sender, MessageEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Message.Text) && e.Message.NewChatMembers == null)
            {
                return;
            }

            var chatAdmins = await BotClient.GetChatAdministratorsAsync(e.Message.Chat.Id);
            if (chatAdmins.All(admin => admin.User.Id != BotClient.BotId))
            {
                // Do nothing until bot is not admin
                return;
            }

            foreach (var command in _commands.Where(command => command.Contains(e.Message)))
            {
                string commandIdentifier = command.Triggers != null
                    ? command.Triggers.First()
                    : command.GetType().Name;
                Log.Logger.Information($"{e.Message.From} (@{e.Message.Chat.Username}) requested command {commandIdentifier}: {e.Message.Text}");

                try
                {
                    await command.Execute(e.Message);
                }
                catch (Exception ex)
                {
                    string errorTemplate = $"Unpredictable error occured ({nameof(ex)}): {ex.Message}\n" +
                                           $"Stack Trace: {ex.StackTrace}";
                    Log.Error(ex, errorTemplate);
                }

                return;
            }
        }

        #region Polling Methods
        private void StartPolling(CancellationToken cancellationToken)
        {
            BotClient.StartReceiving(cancellationToken: cancellationToken);
            Log.Logger.Information("Bot žačal číst vzkazy...");
        }
        private void StopPolling()
        {
            BotClient.StopReceiving();
            Log.Logger.Information("Bot přestal pracovat...");
            Log.CloseAndFlush();
        }
        #endregion

        #region IHostService Implementations
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                try
                {
                    StartPolling(cancellationToken);
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                    }

                    StopPolling();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("During bot work has been thrown error:");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.Source);
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(ex.InnerException);
                }
            }, cancellationToken);
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(StopPolling, cancellationToken);
        }
        #endregion
    }
}
