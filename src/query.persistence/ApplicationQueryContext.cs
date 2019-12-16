using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace query.persistence
{
    public class ApplicationQueryContext
    {
        private readonly IMongoDatabase _database;

        public ApplicationQueryContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("mongo"));

            _database = client.GetDatabase(configuration["mongo:database"]);
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
    }
}