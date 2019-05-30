using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Threading;
using Telegram.Bot.Types;
using LibraryCex;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    class Telegram
    {
        private static ITelegramBotClient botClient;
        public static string text_for_client = "";
        private static List<string> commands = new List<string>() { "/curse", "/balance" };

        public string Text_for_client { get => text_for_client; set => text_for_client = value; }

        public void Bot()
        {
            botClient = new TelegramBotClient("640616392:AAFbJ4MLr-EHV4BqdeoSqxxABss1CRUzQxU") { Timeout = TimeSpan.FromSeconds(10) };
            botClient.OnMessage += Message;
            botClient.StartReceiving();
        }

        public static async void Message(object sender, MessageEventArgs e)
        {
            var text = e?.Message?.Text;
            if (text == null)
            {
                return;
            }
            else if (!commands.Contains(text))
            {
                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Выберите команду\n" +
                    "/curse - вывод курса\n" +
                    "/balance - вывод баланса"
                    );
            }
            else if (text == "/curse")
            {
                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: text_for_client
                    );
            }
            else if (text == "/balance")
            {
                Cex cex = new Cex();
                decimal usd = cex.Balance_USD();
                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text:  "Ваш баланc: " + Convert.ToString(usd) + "$"
                    );
            }
        }
    }
}
