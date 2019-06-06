using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using LibraryCex;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text.RegularExpressions;
using Telegram.Bot.Exceptions;
using System.Linq;

namespace WindowsFormsApp1
{
    class Telegram
    {
        private static ITelegramBotClient botClient;
        public static string text_for_client = "";
        private static List<string> commands = new List<string>() { "/course", "/balance", "/register" };
        private static Dictionary<int, bool> register = new Dictionary<int, bool>();
        private static Dictionary<string, string> logs = new Dictionary<string, string>();
        private static string messageFromUser;

        public string Text_for_client { get => text_for_client; set => text_for_client = value; }
        public static string MessageFromUser { get => messageFromUser; set => messageFromUser = value; }

        public void Bot()
        {
            botClient = new TelegramBotClient("640616392:AAFbJ4MLr-EHV4BqdeoSqxxABss1CRUzQxU") { Timeout = TimeSpan.FromSeconds(10) };
            botClient.OnMessage += Message;
            botClient.OnCallbackQuery += BotClient_OnCallbackQuery;
            botClient.StartReceiving();
        }

        private async void BotClient_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            DataBase data = new DataBase();
            List<string> info = data.Getinfo(e.CallbackQuery.From.Id);
            string currency = e.CallbackQuery.Data;
            Cex cex = new Cex();
            if (currency == "USD")
            {
                try
                {
                    try
                    {
                        decimal usd = cex.Balance_USD(info[0], info[1], info[2]);
                        await botClient.AnswerCallbackQueryAsync(
                                    callbackQueryId: e.CallbackQuery.Id,
                                    text: "Ваш баланс: " + Convert.ToString(usd) + "$"
                                    );
                    }
                    catch (AggregateException)
                    {
                        await botClient.AnswerCallbackQueryAsync(
                                    callbackQueryId: e.CallbackQuery.Id,
                                    text: "Ваши регистрационные данные были не верны"
                                    );
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    try
                    {
                        await botClient.AnswerCallbackQueryAsync(
                                                callbackQueryId: e.CallbackQuery.Id,
                                                text: "Вы не зарегистрированы"
                                                );
                    }
                    catch (InvalidParameterException)
                    {
                    }
                }
                catch (InvalidParameterException)
                {
                }
            }
            else if (currency == "XRP")
            {
                try
                {
                    try
                    {
                        decimal xrp = cex.Balance_XRP(info[0], info[1], info[2]);
                        await botClient.AnswerCallbackQueryAsync(
                            callbackQueryId: e.CallbackQuery.Id,
                            text: "Ваш баланс: " + Convert.ToString(xrp) + " XRP"
                            );
                    }
                    catch (AggregateException)
                    {
                        await botClient.AnswerCallbackQueryAsync(
                                    callbackQueryId: e.CallbackQuery.Id,
                                    text: "Ваши регистрационные данные были не верны"
                                    );
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    try
                    {
                        await botClient.AnswerCallbackQueryAsync(
                                                callbackQueryId: e.CallbackQuery.Id,
                                                text: "Вы не зарегистрированы"
                                                );
                    }
                    catch (InvalidParameterException)
                    {
                    }
                }
                catch (InvalidParameterException)
                {
                }
            }
        }

        public static async void Message(object sender, MessageEventArgs e)
        {
            DataBase data = new DataBase();
            List<string> info = data.Getinfo(Convert.ToInt32(e.Message.Chat.Id));
            var text = e?.Message?.Text;
            bool check = true;
            try
            {
                if (register[Convert.ToInt32(e.Message.Chat.Id)] == true)
                {
                    Regex reg = new Regex(@"^\w+, \w+, \w+$");
                    if (!reg.IsMatch(text))
                    {
                        check = false;
                        register.Remove(Convert.ToInt32(e.Message.Chat.Id));
                        await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat,
                        text: "Неверный формат"
                        );
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
                        "/course - вывод курса\n" +
                        "/balance - вывод баланса"
                        );
                }
                else if (text == "/course")
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
                    if (info.Count == 0)
                    {
                        await botClient.SendTextMessageAsync(
                                    chatId: e.Message.Chat,
                                    text: "Введите ваш User Id, Key и Secret key в формате\n" +
                                    "user_id, key, secret_key"
                                    );
                        register[Convert.ToInt32(e.Message.Chat.Id)] = true;
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(
                                    chatId: e.Message.Chat,
                                    text: "Вы уже зарегистрированы"
                                    );
                    }
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
                register.Remove(Convert.ToInt32(e.Message.Chat.Id));
                await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat,
                        text: "Выберите команду\n" +
                        "/register - регистрация\n" +
                        "/course - вывод курса\n" +
                        "/balance - вывод баланса"
                        );
                var txt = text.Split(',').Select(x => x.Where(y => !Char.IsWhiteSpace(y))).Select(x => string.Concat(x)).ToList();
                int username = Convert.ToInt32(e.Message.Chat.Id);
                data.Insert(username, txt[0], txt[1], txt[2]);
            }
        }
    }
}
