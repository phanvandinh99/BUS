using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebBus.Models
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string role { get; set; }
    }

    public class HocSinh
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string hoTen { get; set; }
        public string lop { get; set; }
        public string tuyenDuongId { get; set; }
        public string userId { get; set; }
    }

    public class XeBus
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string bienSo { get; set; }
        public string trangThai { get; set; }
    }

    public class TuyenDuong
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string tenTuyen { get; set; }
        public string xeBusId { get; set; }
    }

    public class LichTrinh
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string tuyenDuongId { get; set; }
        public string thoiGian { get; set; }
        public string ngay { get; set; }
    }
}