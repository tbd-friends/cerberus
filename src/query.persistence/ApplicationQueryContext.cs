using System.Threading.Tasks;
using MongoDB.Driver;

namespace query.persistence
{
    public class ApplicationQueryContext
    {
        private readonly IMongoDatabase _database;

        public ApplicationQueryContext()
        {
            var client = new MongoClient("mongodb://localhost:27017");

            _database = client.GetDatabase("cerberus");
        }

        public async Task Insert<T>(T model) where T : class
        {
            var collection = _database.GetCollection<T>(typeof(T).Name);

            await collection.InsertOneAsync(model);
        }
    }
}