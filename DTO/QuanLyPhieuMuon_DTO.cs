using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVienNhom3.DTO
{
    public class QuanLyPhieuMuon_DTO
    {
        public int STT { get; set; }

        [DisplayName("Mã Phiếu Mượn")]
        public int MaPhieuMuon { get; set; }

        [DisplayName("Thời Gian Mượn")]
        public DateOnly? ThoiGianMuon { get; set; }

        [DisplayName("Thời Hạn Trả")]
        public DateOnly? ThoihanTra { get; set; }

        [DisplayName("Số Lượng")]
        public int? SoLuong { get; set; }

        [DisplayName("Trạng Thái")]
        public string? TrangThai { get; set; }

        [DisplayName("Nhân Viên")]
        public string? NhanVien { get; set; }

        [DisplayName("Độc Giả")]
        public string? DocGia { get; set; }
    }
}
