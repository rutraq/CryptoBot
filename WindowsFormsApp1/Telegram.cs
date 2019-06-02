using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using LibraryCex;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace WindowsFormsApp1
{
    class Telegram
    {
        private static ITelegramBotClient botClient;
        public static string text_for_client = "";
        private static List<string> commands = new List<string>() { "/curse", "/balance", "/register" };

        public string Text_for_client { get => text_for_client; set => text_for_client = value; }

        public void Bot()
        {
            botClient = new TelegramBotClient("640616392:AAFbJ4MLr-EHV4BqdeoSqxxABss1CRUzQxU") { Timeout = TimeSpan.FromSeconds(10) };
            botClient.OnMessage += Message;
            botClient.OnCallbackQuery += BotClient_OnCallbackQuery;
            botClient.StartReceiving();
        }

        private async void BotClient_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            string currency = e.CallbackQuery.Data;
            Cex cex = new Cex();
            if (currency == "USD")
            {
                decimal usd = cex.Balance_USD();
                await botClient.AnswerCallbackQueryAsync(
                    callbackQueryId: e.CallbackQuery.Id,
                    text: "Ваш баланс: " + Convert.ToString(usd) + "$"
                    );
            }
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
                    "/register - регистрация\n" +
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
                var keyboard = new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("USD"),
                        InlineKeyboardButton.WithCallbackData("XRP")
                    }
                }
                );
                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    replyMarkup: keyboard,
                    text: "Выберите валюту"
                    );
            }
            else if (text == "/register")
            {
                await botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Введите ваш User Id"
                    );
            }
        }
    }
}
