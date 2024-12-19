using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Services;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace api.Repositories
{
    public class CachingRepo(IConnectionMultiplexer connection, ILogger<CachingRepo> _logger) : ICache
    {
        private readonly IConnectionMultiplexer _connection = connection;
        private readonly ILogger<CachingRepo> logger = _logger;
        public async Task<T?> GetFromCacheAsync<T>(string key)
        {
            var db = _connection.GetDatabase();
            var cachedData = await db.StringGetAsync(key);
            if (!cachedData.HasValue) return default;
            return JsonConvert.DeserializeObject<T>(cachedData.ToString(), new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }

        public async Task RemoveByPattern(string pattern)
        {
            var db = _connection.GetDatabase();
            var server = _connection.GetServer(_connection.GetEndPoints().First());
            var keys = server.Keys(pattern: pattern + "*");
            foreach (var key in keys)
            {
                await db.KeyDeleteAsync(key);
            }
        }

        public async Task RemoveCaching(string key)
        {
            var db = _connection.GetDatabase();
            await db.KeyDeleteAsync(key);
        }

        public async Task SetAsync<T>(string key, T values)
        {
            var db = _connection.GetDatabase();
            var serializedData = JsonConvert.SerializeObject(values, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });

            try
            {

                // var data = Encoding.UTF8.GetBytes(serializedData);
                await db.StringSetAsync(key, serializedData, expiry: TimeSpan.FromMinutes(4));
                logger.Log(LogLevel.Information, key);
            }
            catch (Exception ex)
            {
                // Handle Redis operations errors appropriately
                Console.WriteLine($"Error setting cache: {ex.Message}");
            }
        }
    }
}