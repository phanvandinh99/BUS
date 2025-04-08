using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBus.Models
{
    public class ViewModels
    {
        public class HocSinhViewModel
        {
            public string Id { get; set; }
            public string userId { get; set; }
            public string hoTen { get; set; }
            public string lop { get; set; }
            public string tuyenDuongId { get; set; }
            public string tenTuyenDuong { get; set; }
        }
        public class TuyenDuongViewModel
        {
            public string Id { get; set; }
            public string tenTuyen { get; set; }
            public string xeBusId { get; set; }
            public string bienSoXe { get; set; }
        }

        public class LichTrinhViewModel
        {
            public string Id { get; set; }
            public string tuyenDuongId { get; set; }
            public string tenTuyenDuong { get; set; }
            public string thoiGian { get; set; }
            public string ngay { get; set; }
        }

        public class UserLichTrinhViewModel
        {
            public string UserId { get; set; }
            public string Username { get; set; }
            public string Role { get; set; }
            public string HoTen { get; set; } // Từ HocSinh
            public string TenTuyenDuong { get; set; } // Từ TuyenDuong
            public string ThoiGian { get; set; } // Từ LichTrinh
            public string Ngay { get; set; } // Từ LichTrinh
        }
    }
}