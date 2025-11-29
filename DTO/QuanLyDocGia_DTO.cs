
using QuanLyThuVienNhom3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVienNhom3.DTO
{
    public class QuanLyDocGia_DTO
    {
        public int STT { get; set; }
        [DisplayName("Mã Độc Giả")]
        public int MaDocGia { get; set; }
        [DisplayName("Tên Độc Giả")]
        public string? TenDocGia { get; set; }
        [DisplayName("Ngày Sinh")]
        public DateOnly? NgaySinh { get; set; }
        [DisplayName("Giới Tính")]
        public string? GioiTinh { get; set; }
        [DisplayName("Hình Ảnh")]
        public byte[]? HinhAnh { get; set; }
        [DisplayName("Địa Chỉ")]
        public string? DiaChi { get; set; }
        [DisplayName("Số Điện Thoại")]
        public string? SoDienThoai { get; set; }
        [DisplayName("Email")]
        public string? Email { get; set; }
        [DisplayName("Trạng Thái")]
        public string? TrangThai { get; set; }
    }
}