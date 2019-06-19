using System;
using System.Collections.Specialized;
using System.Net;

namespace WindowsFormsApp1
{
    class PlaceOrder
    {
        public void MakeOrder(string username, string key, string secretKey, string amount, string price, string type)
        {
            SignatureGenerator sg = new SignatureGenerator();
            int unixTime = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            string url = "https://cex.io/api/place_order/XRP/USD/";

            using (var webClient = new WebClient())
            {
                // Создаём коллекцию параметров
                var pars = new NameValueCollection();

                // Добавляем необходимые параметры в виде пар ключ, значение
                pars.Add("key", key);
                pars.Add("signature", sg.Compute(username, key, secretKey));
                pars.Add("nonce", Convert.ToString(unixTime));
                pars.Add("type", type);
                pars.Add("amount", amount);
                pars.Add("price", price);

                // Посылаем параметры на сервер
                // Может быть ответ в виде массива байт
                var response = webClient.UploadValues(url, pars);
            }
        }
    }
}
