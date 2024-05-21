using LEARN.db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Pomelo.EntityFrameworkCore.MySql.Internal;
using StackExchange.Redis;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace LEARN.redis
{
    public class redishelp
    {
        private readonly redisoption _redisoption;
        private ConnectionMultiplexer redis;
        private StackExchange.Redis.IDatabase db;

        public redishelp(IOptions<redisoption> redisoption)
        {
            _redisoption = redisoption.Value;
            redis = ConnectionMultiplexer.Connect(_redisoption.redisconnection);
        }
        
        //public redishelp(string redisconnection)
        //{        
        //    redis = ConnectionMultiplexer.Connect(redisconnection);
        //}

   
        public StackExchange.Redis.IDatabase getdb()
        {
           return redis.GetDatabase();
        }
    }
}
