using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaiAdmin.Common.CacheHelper
{
    /// <summary>
    /// Redis助手
    /// </summary>
    public class RedisCacheHelper : ICacheHelper
    {
        //public IDatabase _cache;

        //private ConnectionMultiplexer _connection;

        //private readonly string _instance;
        //public RedisCacheHelper(RedisCacheOptions options, int database = 0)
        //{
        //    _connection = ConnectionMultiplexer.Connect(options.Configuration);
        //    _cache = _connection.GetDatabase(database);
        //    _instance = options.InstanceName;
        //}

        public RedisCacheHelper(string connectionString)
        {
            var csRedis = new CSRedis.CSRedisClient(connectionString);
            RedisHelper.Initialization(csRedis);
        }

        public bool Exists(string key)
        {
            return RedisHelper.Exists(key);
            //return _cache.KeyExists(_instance + key);
        }

        public void Set<T>(string key, T value)
        {
            RedisHelper.Set(key, value);
            //_cache.StringSet(_instance + key, CommonHelper.ObjectToJsonString(value));
        }

        public T Get<T>(string key)
        {
            return RedisHelper.Get<T>(key);
            //return CommonHelper.JsonStringToObject<T>(_cache.StringGet(_instance + key));
        }

        public void Delete(string key)
        {
            RedisHelper.Del(key);
            //_cache.KeyDelete(_instance + key);
        }

        public void Expire(string key, DateTime dateTime)
        {
            RedisHelper.Expire(key, dateTime - DateTime.Now);
            //_cache.KeyExpire(_instance + key, dateTime);
        }
        public void Expire(string key, TimeSpan timeSpan)
        {
            RedisHelper.Expire(key, timeSpan);
            //_cache.KeyExpire(_instance + key, timeSpan);
        }
        public void HashAdd(string key, string hashKey, object hashValue)
        {

            //string value = CommonHelper.ObjectToJsonString(hashValue);
            //_cache.HashSet(_instance + key, hashKey, value);
            RedisHelper.HSet(key, hashKey, hashValue);
        }

        public T HashGet<T>(string key, string hashKey)
        {
            //var value = _cache.HashGet(_instance + key, hashKey);
            //return CommonHelper.JsonStringToObject<T>(value);
            return RedisHelper.HGet<T>(key, hashKey);

        }

        public object HashGet(string key, string hashKey, Type type)
        {
            var value = RedisHelper.HGet(key, hashKey);
            //var value = _cache.HashGet(_instance + key, hashKey);
            return CommonHelper.JsonStringToObject(value, type);
        }

        public bool HashExists(string key, string hashKey)
        {
            return RedisHelper.HExists(key, hashKey);
            //return _cache.HashExists(_instance + key, hashKey);
        }

        public void HashRemove(string key, string hashKey)
        {
            RedisHelper.HDel(key, hashKey);
            //_cache.HashDelete(_instance + key, hashKey);
        }

        public void SetAdd(string key, params string[] values)
        {
            RedisHelper.SAdd(key, values);
            //RedisValue[] redisValues = values.Select(e => (RedisValue)e).ToArray();
            //_cache.SetAdd(_instance + key, redisValues);
        }
        public void SetRemove(string key, params string[] values)
        {
            RedisHelper.SRem(key, values);
            //RedisValue[] redisValues = values.Select(e => (RedisValue)e).ToArray();
            //_cache.SetRemove(_instance + key, redisValues);
        }
        public bool SetContains(string key, string value)
        {
            return RedisHelper.SIsMember(key, value);
            //return _cache.SetContains(_instance + key, value);
        }

        public HashSet<string> SetGet(string key)
        {
            return new HashSet<string>(RedisHelper.SMembers(key));
            //return new HashSet<string>(_cache.SetMembers(_instance + key).Select(e => e.ToString()));
        }

        public void SortedSetAdd(string key, string value, double score)
        {
            //_cache.SortedSetAdd(_instance + key, value, score);
        }
        public void SortedSetRemove(string key, string value)
        {
            //_cache.SortedSetRemove(_instance + key, value);
        }
    }
}
