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

        public IMongoCollection<UserDto> Users() => _database.GetCollection<UserDto>("user");
        public IMongoCollection<UserContactsDto> UserContacts() => _database.GetCollection<UserContactsDto>("userContacts");

        public IMongoCollection<UserBadgeEntryDto> UserBadgeEntries() => _database.GetCollection<UserBadgeEntryDto>("userBadges");

        public MongoContext()
        {
            string mongoConnection = ConfigurationManager.ConnectionStrings["MongoConnection"].ConnectionString;
            var client = new MongoClient(mongoConnection);
            _database = client.GetDatabase("akkaPlayground");
        }
    }
}
