using StackExchange.Redis;

namespace telegramcryptobot.src
{
    internal class RedisDataBase
    {
        ConnectionMultiplexer redis;
        IDatabase db;

        public RedisDataBase(string host = "localhost", int db = -1, object? asyncState = null)
        {
            this.redis = ConnectionMultiplexer.Connect(host);
            this.db = this.redis.GetDatabase(db, asyncState);
        }

        public object data()
        {
            return db.Database;
        }

        public void setKey(string key, StackExchange.Redis.RedisValue value)
        {
            db.StringSet(key, value);
        }

        public StackExchange.Redis.RedisValue getKey(string key)
        {
            return db.StringGet(key);
        }

        public bool check(string key)
        {
            return getKey(key) != getKey("checker");
        }
    }
}
