using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WindowsFormsApp1
{
    class SignatureGenerator
    {
        /// <summary>
        /// Compute signature.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="apiKey">API key.</param>
        /// <param name="nonce">Nonce.</param>
        /// <param name="apiSecret">API secret.</param>
        /// <returns>String that represents generated signature.</returns>
        public string Compute(string userId, string apiKey, string apiSecret)
        {
            byte[] secretBytes = Encoding.UTF8.GetBytes(apiSecret);
            HMAC hmac = new HMACSHA256(secretBytes);
            int nonce = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            var bytes = Encoding.UTF8.GetBytes($"{nonce}{userId}{apiKey}");
            var hash = hmac.ComputeHash(bytes);

            return string.Concat(hash.Select(b => b.ToString("X2")));
        }
    }
}
