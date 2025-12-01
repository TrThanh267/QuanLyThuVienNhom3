using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVienNhom3.DTO
{
    public class QuanLyPhieuMuonChiTiet_DTO
    {
        public int STT {  get; set; }

        [DisplayName("Mã Chi Tiết Phiếu Mượn")]
        public int MaChiTietPhieuMuon { get; set; }

        [DisplayName("Số Lượng Mượn")]
        public int? SoLuongMuon { get; set; }

        [DisplayName("Tình Trạng Sách")]
        public string? TinhTrangSach { get; set; }

        [DisplayName("Tên Sách")]
        public string? Sach { get; set; }

        [DisplayName("Mã Phiếu Mượn")]
        public int? MaPhieuMuon { get; set; }
        [DisplayName("Đã trả sách")]
        public bool? DaGhiNhanTra { get; set; }
        [DisplayName("Ngày Trả Sách")]
        public DateTime NgayTraSach { get; set; }
    }
}
