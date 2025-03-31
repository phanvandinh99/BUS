
docker-compose up -d

db.Users.insertMany([
  { "username": "admin", "password": "12345", "role": "Admin" },
  { "username": "hs1", "password": "12345", "role": "HocSinh" }
]);
db.HocSinh.insertOne({ "hoTen": "Nguyen Van A", "lop": "10A1", "tuyenDuongId": "tuyen1", "userId": "hs1" });
db.XeBus.insertOne({ "bienSo": "29B-12345", "trangThai": "Đang hoạt động" });
db.TuyenDuong.insertOne({ "tenTuyen": "Tuyến Nội thành", "xeBusId": "29B-12345" });
db.LichTrinh.insertOne({ "tuyenDuongId": "tuyen1", "thoiGian": "07:00", "ngay": "2025-04-01" });