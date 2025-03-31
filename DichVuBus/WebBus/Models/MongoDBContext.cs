using MongoDB.Driver;

namespace WebBus.Models
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            _database = client.GetDatabase("DataDichVuBus");
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<HocSinh> HocSinh => _database.GetCollection<HocSinh>("HocSinh");
        public IMongoCollection<XeBus> XeBus => _database.GetCollection<XeBus>("XeBus");
        public IMongoCollection<TuyenDuong> TuyenDuong => _database.GetCollection<TuyenDuong>("TuyenDuong");
        public IMongoCollection<LichTrinh> LichTrinh => _database.GetCollection<LichTrinh>("LichTrinh");
    }
}