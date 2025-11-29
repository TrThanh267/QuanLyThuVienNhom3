using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVienNhom3.DTO
{
    public class QuanLyNhanVien_DTO
    {
        public int STT { get; set; }
        [DisplayName("Mã Nhân Viên")]
        public int MaNhanVien { get; set; }
        [DisplayName("Tên Nhân Viên")]
        public string? TenNhanVien { get; set; }
        [DisplayName("Ngày Sinh")]
        public DateOnly? NgaySinh { get; set; }
        [DisplayName("Giới Tính")]
        public string? GioiTinh { get; set; }
        [DisplayName("Số Điện Thoại")]
        public string? SoDienThoai { get; set; }
        [DisplayName("Email")]
        public string? Email { get; set; }
        [DisplayName("Địa Chỉ")]
        public string? DiaChi { get; set; }
        [DisplayName("Trạng Thái")]
        public string? TrangThai { get; set; }
        [DisplayName("Tài Khoản")]
        public string? TaiKhoan { get; set; } = null!;
    }
}
