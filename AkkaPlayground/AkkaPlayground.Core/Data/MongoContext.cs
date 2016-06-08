using AkkaPlayground.Projections;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPlayground.Core.Data
{

    public class MongoContext
    {
        private readonly IMongoDatabase _database;

        public IMongoCollection<UserProjection> Users() => _database.GetCollection<UserProjection>("user");
        public IMongoCollection<UserContactsProjection> UserContacts() => _database.GetCollection<UserContactsProjection>("userContacts");

        public MongoContext()
        {
            string mongoConnection = ConfigurationManager.ConnectionStrings["MongoConnection"].ConnectionString;
            var client = new MongoClient(mongoConnection);
            _database = client.GetDatabase("akkaPlayground");
        }
    }
}
