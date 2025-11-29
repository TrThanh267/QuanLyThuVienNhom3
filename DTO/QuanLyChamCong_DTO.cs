using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVienNhom3.DTO
{
    public class QuanLyChamCong_DTO
    {
        public int STT { get; set; }
        [DisplayName("Mã Chấm Công")]
        public int? MaChamCong { get; set; }
        [DisplayName("Ngày Làm")]
        public int? NgayLam { get; set; }
        [DisplayName("Tháng Làm")]
        public int? ThangLam { get; set; }
        [DisplayName("Năm Làm")]
        public int? NamLam { get; set; }
        [DisplayName("Giờ Vào")]
        public int? GioVao { get; set; }
        [DisplayName("Phút Vào")]
        public int? PhutVao { get; set; }
        [DisplayName("Giờ Ra")]
        public int? GioRa { get; set; }
        [DisplayName("Phút Ra")]
        public int? PhutRa { get; set; }
        [DisplayName("Loại Công")]
        public string? LoaiCong { get; set; }
        [DisplayName("Ca Làm")]
        public string? CaLam { get; set; }
        [DisplayName("Nhân Viên")]
        public string? NhanVien { get; set; }
        public int? MaNhanVien { get; set; }
    }
}
