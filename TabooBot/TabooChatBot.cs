using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TabooBot
{
    public sealed class TabooChatBot : IHostedService
    {
        public static ITelegramBotClient BotClient;

        public TabooChatBot()
        {
            InitializeBotClientInstance(Program.Configuration["BotToken"]);
        }
        private static void InitializeBotClientInstance(string botToken)
        {
            BotClient = new TelegramBotClient(botToken);
        }

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
    }
}
