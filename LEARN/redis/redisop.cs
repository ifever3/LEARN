using StackExchange.Redis;

namespace LEARN.redis
{
    public class redisop 
    {
        private readonly IDatabase _db;
        public redisop(redishelp redishelp)
        {    
            _db=redishelp.getdb();
        }

        public bool SetValue(string key, string value)
        {
            return _db.StringSet(key, value);
        }

        public string GetValue(string key)
        {
            return _db.StringGet(key);
        }

        public bool DeleteKey(string key)
        {
            return _db.KeyDelete(key);
        }
    }
}
