using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using LibraryCex;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text.RegularExpressions;
using System.IO;

namespace WindowsFormsApp1
{
    class Telegram
    {
        private static ITelegramBotClient botClient;
        public static string text_for_client = "";
        private static List<string> commands = new List<string>() { "/curse", "/balance", "/register" };
        private static Dictionary<string, bool> register = new Dictionary<string, bool>();

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
            else if (currency == "XRP")
            {
                decimal xrp = cex.Balance_XRP();
                await botClient.AnswerCallbackQueryAsync(
                    callbackQueryId: e.CallbackQuery.Id,
                    text: "Ваш баланс: " + Convert.ToString(xrp) + "$"
                    );
            }
        }

        public static async void Message(object sender, MessageEventArgs e)
        {
            var text = e?.Message?.Text;
            bool check = true;
            try
            {
                if (register[e.Message.Chat.Username] == true)
                {
                    Regex reg = new Regex(@"^\w+, \w+, \w+$");
                    if (!reg.IsMatch(text))
                    {
                        check = false;
                        register.Remove(e.Message.Chat.Username);
                    }
                }
            }
            catch (KeyNotFoundException)
            {
                check = false;
            }
            if (!check)
            {
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
                        text: "Введите ваш User Id, Key и Secret key в формате\n" +
                        "user_id, key, secret_key"
                        );
                    register[e.Message.Chat.Username] = true;
                } 
            }
            else
            {
                await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat,
                        text: "Вы зарегистрированы"
                        );
                await botClient.DeleteMessageAsync(
                    chatId: e.Message.Chat,
                    messageId: e.Message.MessageId
                    );
                register.Remove(e.Message.Chat.Username);
            }
        }
    }
}
