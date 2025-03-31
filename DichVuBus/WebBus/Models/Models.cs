using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebBus.Models
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class HocSinh
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string HoTen { get; set; }
        public string Lop { get; set; }
        public string TuyenDuongId { get; set; }
        public string UserId { get; set; }
    }

    public class XeBus
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string BienSo { get; set; }
        public string TrangThai { get; set; }
    }

    public class TuyenDuong
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string TenTuyen { get; set; }
        public string XeBusId { get; set; }
    }

    public class LichTrinh
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string TuyenDuongId { get; set; }
        public string ThoiGian { get; set; }
        public string Ngay { get; set; }
    }
}