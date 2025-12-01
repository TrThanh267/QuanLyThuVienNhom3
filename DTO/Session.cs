using QuanLyThuVienNhom3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nhom3ThuVienBanNhap.DTO
{
    public static class UserSession
    {
        public static TaiKhoan? TaiKhoanHienTai { get; set; }
        public static void ClearSession()
        {
            TaiKhoanHienTai = null;
        }
    }
}
