using System.Security.Cryptography;
using System.Text;

namespace telegramcryptobot.src
{
    internal class Cryptography
    {


        public string genText(int length = 15)
        {
            string text = "";
            string randomletters = "`~!1@2#3$4%5^6&7*8(9)0_-+=QqWwEeRrTtYyUuIiOoPp{[}]|AaSsDdFfGgHhJjKkLl:;'ZzXxCcVvBbNnMm<,>.?/";
            if (length < 1 | length > 500)
            {
                length = 15;
            }
            for (int x = 0; x < length; x++)
            {
                text += randomletters[new Random().Next(randomletters.Length-1)];
            }
            return text;
        }

        public byte[] getKey(string text)
        {
            SHA256 sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.Unicode.GetBytes(text));
        }

        public byte[] getIV()
        {
            Aes aes = Aes.Create();
            aes.GenerateIV();
            return aes.IV;
        }

        public byte[] toAes256(string src, byte[] key, byte[] IV = null)
        {
            Aes aes = Aes.Create();
            if (IV == null) aes.GenerateIV();
            else aes.IV = IV;
            aes.Key = key;
            byte[] encrypted;
            ICryptoTransform crypt = aes.CreateEncryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, crypt, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(src);
                    }
                }
                encrypted = ms.ToArray();
            }
            return encrypted.Concat(aes.IV).ToArray();
        }

        public string fromAes256(byte[] shifr, byte[] key)
        {
            byte[] bytesIv = new byte[16];
            byte[] mess = new byte[shifr.Length - 16];
            for (int i = shifr.Length - 16, j = 0; i < shifr.Length; i++, j++)
                bytesIv[j] = shifr[i];

            for (int i = 0; i < shifr.Length - 16; i++)
                mess[i] = shifr[i];
            Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = bytesIv;
            string text = "";
            byte[] data = mess;
            ICryptoTransform crypt = aes.CreateDecryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (CryptoStream cs = new CryptoStream(ms, crypt, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        text = sr.ReadToEnd();
                    }
                }
            }
            return text;
        }
    }
}
