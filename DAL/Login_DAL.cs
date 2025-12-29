using Microsoft.EntityFrameworkCore;
using QuanLyThuVienNhom3.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVienNhom3.DAL
{
    public class Login_DAL
    {
        private readonly ThuVienNhom3Context _context;
        private static readonly ConcurrentDictionary<string, (string Code, DateTime Expiry)> _otpStore
            = new ConcurrentDictionary<string, (string, DateTime)>();

        public Login_DAL()
        {
            _context = new ThuVienNhom3Context();
        }
        public TaiKhoan? GetByUsername(string username)
        {
            return _context.TaiKhoans
                           .FirstOrDefault(tk => tk.TenTaiKhoan == username);
        }
        public string GenerateAndSaveOtp(string email)
        {
            Random rand = new Random();
            string otpCode = rand.Next(100000, 999999).ToString();
            var expiry = DateTime.Now.AddMinutes(5);
            _otpStore[email] = (otpCode, expiry);

            return otpCode;
        }
        public bool VerifyOtpAndResetPassword(string email, string enteredOtp, string newPassword)
        {
            if (!_otpStore.TryGetValue(email, out var otpData))
            {
                return false;
            }
            if (otpData.Code != enteredOtp || otpData.Expiry < DateTime.Now)
            {
                return false;
            }
            using (var context = new ThuVienNhom3Context())
            {
                var user = context.TaiKhoans
                                  .Include(tk => tk.NhanVien)
                                  .FirstOrDefault(tk => tk.NhanVien.Email == email);

                if (user == null) return false;
                user.MatKhauHash = HashPassword(newPassword);

                context.SaveChanges();
                _otpStore.TryRemove(email, out _);

                return true;
            }
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password.Trim());
        }
        public TaiKhoan? GetTaiKhoanByEmail(string email)
        {
            return _context.TaiKhoans
                           .Include(tk => tk.NhanVien)
                           .FirstOrDefault(tk => tk.NhanVien.Email == email);
        }
    }
}
