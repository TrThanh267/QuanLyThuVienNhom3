using QuanLyThuVienNhom3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVienNhom3.DAL
{
    public class Login_DAL
    {
        private readonly ThuVienNhom3Context _context;

        public Login_DAL()
        {
            _context = new ThuVienNhom3Context();
        }
        public TaiKhoan? GetByUsername(string username)
        {
            return _context.TaiKhoans
                           .FirstOrDefault(tk => tk.TenTaiKhoan == username);
        }
    }
}
