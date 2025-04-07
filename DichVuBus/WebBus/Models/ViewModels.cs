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
            public ObjectId Id { get; set; }
            public string userId { get; set; }
            public string hoTen { get; set; }
            public string lop { get; set; }
            public string tuyenDuongId { get; set; }
            public string tenTuyenDuong { get; set; }
        }
        public class TuyenDuongViewModel
        {
            public ObjectId Id { get; set; }
            public string tenTuyen { get; set; }
            public string xeBusId { get; set; }
            public string bienSoXe { get; set; }
        }

        public class LichTrinhViewModel
        {
            public ObjectId Id { get; set; }
            public string tuyenDuongId { get; set; }
            public string tenTuyenDuong { get; set; }
            public string thoiGian { get; set; }
            public string ngay { get; set; }
        }
    }
}