using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace WindowsFormsApp1
{
    class MyAccount
    {
        private string UserID = "up112448722";
        private string key = "74FTgNUB0xDlXKB17us6EOPqQ";
        private string secretKey = "9gZbn9aNyvpJZZRv2zu1lgchbE";

        public string GetMyBalance()
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            string message = Convert.ToString(unixTimestamp) + UserID + key;
            byte[] s = Encoding.UTF8.GetBytes(secretKey);
            var hash = new HMACSHA256(s);
            var signature = hash.ComputeHash(Encoding.UTF8.GetBytes(message));
            string str = Encoding.UTF8.GetString(signature);
            string ProxyString = "";
            string URI = @"https://cex.io/api/balance/"; ;
            string Parameters = $"key={key}&signature={str.ToUpper()}&nonce={unixTimestamp.ToString()}";
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            //req.Proxy = new System.Net.WebProxy(ProxyString, true); 
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream(); // создаем поток 
            os.Write(bytes, 0, bytes.Length); // отправляем в сокет 
            os.Close();
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null)
            {
                return "nahui";
            }
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }
    }
}
