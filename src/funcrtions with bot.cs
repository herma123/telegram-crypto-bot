using telegramcryptobot.src;

namespace telegramcryptobot
{
    internal class funcrtionsWithBot
    {
        public string start()
        {
            return "Ключи и пароли нигде не сохраняются!\n\n/encrypt [key] [data] - шифрование данных по ключу (ключ вычисляется до пробела), все остальное - данные\n/decrypt [key] - дешифрование данных по ключу\n/genkey [length] - генерация ключа. в [length] длина ключа - не меньше 1 и не больше 500 символов, иначе сгенерируется обычный ключ в 15 символов.";
        }

        public string genKey(int length = 15)
        {
            return new Cryptography().genText(length);
        }

        public string encrypt(string user_id, string key, string data, RedisDataBase redis)
        {
            Cryptography cryptography = new Cryptography();
            redis.setKey(user_id, cryptography.toAes256(data, cryptography.getKey(key)));
            return $"Данные добавлены в зашифрованном виде под ключом {key}";
        }

        public string decrypt(string user_id, string key, RedisDataBase redis)
        {
            Cryptography cryptography = new Cryptography();
            return cryptography.fromAes256(redis.getKey(user_id), cryptography.getKey(key));
        }
    }
}
