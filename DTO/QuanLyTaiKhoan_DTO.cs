using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVienNhom3.DTO
{
    public class QLTaiKhoan_DTO
    {
        [DisplayName("STT")]
        public int STT { get; set; }

        [DisplayName("Mã tài khoản")]
        public int MaTK { get; set; }

        [DisplayName("Tên tài khoản")]
        public string? TenTK { get; set; }

        [DisplayName("Mật khẩu")]
        public string? MaTKhau { get; set; }

        [DisplayName("Trạng thái")]
        public string? TrangThai { get; set; }

        [DisplayName("Vai trò")]
        public string? VaiTro { get; set; }
    }
}
