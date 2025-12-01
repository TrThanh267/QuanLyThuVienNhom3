using BCrypt.Net;
using QuanLyThuVienNhom3.DAL;
using QuanLyThuVienNhom3.DTO;
using QuanLyThuVienNhom3.Models;
using Nhom3ThuVienBanNhap.DTO;

namespace QuanLyThuVienNhom3.BLL
{
    public class Login_BLL
    {
        private Login_DAL _dal = new Login_DAL();

        public string Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return "Vui lòng nhập tên tài khoản và mật khẩu.";
            }
            var taiKhoan = _dal.GetByUsername(username);
            if (taiKhoan == null)
            {
                return "Tài khoản không tồn tại.";
            }
            if (taiKhoan.TrangThai != "Hoạt động")
            {
                return "Tài khoản đang bị khóa.";
            }
            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, taiKhoan.MatKhauHash);

            if (!isPasswordCorrect)
            {
                return "Mật khẩu không chính xác.";
            }
            UserSession.TaiKhoanHienTai = taiKhoan;

            return "";
        }
    }
}