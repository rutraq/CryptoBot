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
        private static string startText = "Выберите команду\n" +
                    "/register - регистрация\n" +
                    "/course - вывод курса\n" +
                    "/balance - вывод баланса\n" +
                    "/info - Информация о рынке\n" +
                    "/sum - Установка суммы ставки в XRP. Минимум - 40";
        private static ITelegramBotClient botClient;
        public static string text_for_client = "";
        private static List<string> commands = new List<string>() { "/course", "/balance", "/register", "/info", "/sum" };
        private static Dictionary<int, bool> register = new Dictionary<int, bool>();
        private static Dictionary<long, int> infoForDelete = new Dictionary<long, int>();
        private static List<long> addSum = new List<long>();


        public string Text_for_client { get => text_for_client; set => text_for_client = value; }

        public void Bot()
        {
            botClient = new TelegramBotClient("640616392:AAFbJ4MLr-EHV4BqdeoSqxxABss1CRUzQxU") { Timeout = TimeSpan.FromSeconds(10) };
            botClient.OnMessage += Message;
            botClient.OnCallbackQuery += BotClient_OnCallbackQuery;
            botClient.StartReceiving();
        }

        public void Probe(EventHandler<MessageEventArgs> Method)
        {
            botClient.OnMessage += Method;
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
            try
            {
                await botClient.EditMessageReplyMarkupAsync(
                        chatId: e.CallbackQuery.From.Id,
                        messageId: infoForDelete[e.CallbackQuery.From.Id]
                        );
                await botClient.EditMessageTextAsync(
                    chatId: e.CallbackQuery.From.Id,
                    messageId: infoForDelete[e.CallbackQuery.From.Id],
                    text: startText
                    );
                infoForDelete.Remove(e.CallbackQuery.From.Id);
            }
            catch (KeyNotFoundException)
            {
            }
            catch (MessageIsNotModifiedException)
            {
            }
        }

        public static async void Message(object sender, MessageEventArgs e)
        {
            DataBase data = new DataBase();
            List<string> info = data.Getinfo(Convert.ToInt32(e.Message.Chat.Id));
            var text = e?.Message?.Text;
            bool checkReg = true;
            bool checkSum = true;

            //Check registration
            try
            {
                if (register[Convert.ToInt32(e.Message.Chat.Id)] == true)
                {
                    Regex reg = new Regex(@"^\w+, \w+, \w+$");
                    if (!reg.IsMatch(text))
                    {
                        checkReg = false;
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
                checkReg = false;
            }

            //check sum
            if (!addSum.Contains(e.Message.Chat.Id))
            { 
                checkSum = false;
            }
            else
            {
                if (info.Count == 0)
                {
                    await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat,
                            text: "Вы не зарегистрированы\n" +
                            "Пройдите регистрацию для выполнения этой команды"
                            );
                    checkSum = false;
                    addSum.Remove(e.Message.Chat.Id);
                }
            }

            //all messages
            if (!checkReg && !checkSum)
            {
                if (text == null)
                {
                    return;
                }
                else if (!commands.Contains(text))
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat,
                        text: startText
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
                    var keyboard_message = await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat,
                        replyMarkup: keyboard,
                        text: "Выберите валюту"
                        );
                    infoForDelete[e.Message.Chat.Id] = keyboard_message.MessageId;
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
                else if (text == "/info")
                {
                    Cex cex = new Cex();
                    InfoFromStock infoFromStock = new InfoFromStock();
                    GetCurrency currency = new GetCurrency();
                    var orderBook = cex.Order();
                    decimal course = Convert.ToDecimal(currency.ParseJSON()["lprice"].Replace(".", ","));
                    decimal asks = infoFromStock.GetAsks(8);
                    decimal bids = infoFromStock.GetBids(8) / course;
                    await botClient.SendTextMessageAsync(
                                chatId: e.Message.Chat,
                                text: $"Объём на покупку: {bids} XRP\n" +
                                $"Объём на продажу: {asks} XRP"
                        );
                }
                else if (text == "/sum")
                {
                    await botClient.SendTextMessageAsync(
                                chatId: e.Message.Chat,
                                text: "Введите сумму для ставок");
                    addSum.Add(e.Message.Chat.Id);
                }
            }
            else if (checkReg)
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
                        text: startText
                        );
                var txt = text.Split(',').Select(x => x.Where(y => !Char.IsWhiteSpace(y))).Select(x => string.Concat(x)).ToList();
                int username = Convert.ToInt32(e.Message.Chat.Id);
                data.Insert(username, txt[0], txt[1], txt[2]);
            }
            else if (checkSum)
            {
                try
                {
                    int sum = Convert.ToInt32(e.Message.Text);
                    if (sum < 40)
                    {
                        throw new FormatException();
                    }
                    data.Insert(Convert.ToInt32(e.Message.Chat.Id), sum);
                    addSum.Remove(e.Message.Chat.Id);
                    await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat,
                            text: "Ваша сумма для ставок принята");
                    await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat,
                            text: startText);
                }
                catch (FormatException)
                {
                    await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat,
                            text: "Сумма должна быть указана целым числом и быть больше либо равно 40\n" +
                            "Попробуйте ещё раз /sum");
                    addSum.Remove(e.Message.Chat.Id);
                }
            }
        }
    }
}
