using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace consumer.persistence
{
    public class MongoStorage
    {
        private readonly IMongoDatabase _database;

        public MongoStorage(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("mongo"));

            _database = client.GetDatabase(configuration["mongo:database"]);
        }

        public async Task Insert<T>(T model) where T : class
        {
            var collection = _database.GetCollection<T>(typeof(T).Name);

            await collection.InsertOneAsync(model);
        }

        public async Task<T> Get<T>(Expression<Func<T, bool>> filter)
        {
            var collection = _database.GetCollection<T>(typeof(T).Name);

            return await collection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAll<T>() where T : class
        {
            var collection = _database.GetCollection<T>(typeof(T).Name);

            var results = await collection.FindAsync(FilterDefinition<T>.Empty);

            return await results.ToListAsync();
        }

        public async Task Update<T>(Expression<Func<T, bool>> filter, T model) where T : class
        {
            var collection = _database.GetCollection<T>(typeof(T).Name);

            if (await collection.Find(filter).AnyAsync())
            {
                await collection.ReplaceOneAsync(filter, model);
            }
            else
            {
                await Insert(model);
            }
        }

        public async Task Delete<T>(Expression<Func<T, bool>> filter)
        {
            var collection = _database.GetCollection<T>(typeof(T).Name);

            await collection.DeleteOneAsync(filter);
        }
    }
}