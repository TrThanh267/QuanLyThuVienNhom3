using Microsoft.EntityFrameworkCore;
using QuanLyThuVienNhom3.DTO;
using QuanLyThuVienNhom3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVienNhom3.DAL
{
    internal class QLTaiKhoan_DAL
    {
        private readonly ThuVienNhom3Context _context = new ThuVienNhom3Context();
        public TaiKhoan TaiKhoan = new TaiKhoan();
        public List<QLTaiKhoan_DTO> GetListTaiKhoan()
        {
            var list = _context.TaiKhoans
         .Include(x => x.MaVaiTroNavigation)
         .ToList();

            var data = list.Select((x, index) => new QLTaiKhoan_DTO
            {
                STT = index + 1,
                MaTK = x.MaTaiKhoan,
                TenTK = x.TenTaiKhoan,
                MaTKhau = x.MatKhauHash,
                TrangThai = x.TrangThai,
                VaiTro = x.MaVaiTroNavigation?.TenVaiTro
            }).ToList();

            return data;
        }

        public bool ThemTk(TaiKhoan tk)
        {
            try
            {
                _context.TaiKhoans.Add(tk);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("DAL lỗi: " + ex.Message + "\n" + (ex.InnerException?.Message ?? ""),
                                "DEBUG", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public TaiKhoan LayTaiKhoanTheoMa(int maTK)
        {
            return _context.TaiKhoans.Find(maTK);
        }

        public int DemSoAdmin()
        {
            return _context.TaiKhoans.Count(t => t.MaVaiTro == 1);
        }

        // DAL (hoặc Repository)
        public bool XoaTK(int maTK)
        {
            try
            {
                var tk = _context.TaiKhoans.FirstOrDefault(x => x.MaTaiKhoan == maTK);
                if (tk == null) return false;

                _context.TaiKhoans.Remove(tk);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex) // Lỗi khóa ngoại (có nhân viên đang dùng)
            {
                // Quan trọng nhất: Reset lại toàn bộ trạng thái của DbContext
                // để lần SaveChanges() tiếp theo (khi Thêm) không bị dính lỗi cũ
                foreach (var entry in _context.ChangeTracker.Entries().ToList())
                {
                    entry.State = EntityState.Detached;
                }

                MessageBox.Show("Không thể xóa tài khoản này vì đang có nhân viên sử dụng!",
                                "Lỗi ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            catch (Exception ex)
            {
                // Reset context khi có lỗi bất ngờ
                foreach (var entry in _context.ChangeTracker.Entries().ToList())
                {
                    entry.State = EntityState.Detached;
                }

                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
                return false;
            }
        }
        public bool KiemTraTenDaTonTai(string name)
        {
            return _context.TaiKhoans.Any(n => n.TenTaiKhoan == name);
        }
        public bool KiemTraMKDaTonTai(string mk)
        {
            return _context.TaiKhoans.Any(n => n.MatKhauHash == mk);
        }
        // Tìm kiếm tài khoản theo từ khóa (tên đăng nhập, mật khẩu gốc, trạng thái, vai trò)
        public List<QLTaiKhoan_DTO> TimKiemTaiKhoan(string tuKhoa)
        {
            return _context.TaiKhoans.Include(tk => tk.MaVaiTroNavigation)
                 .Where(tk => tk.MaVaiTro.ToString().Equals(tuKhoa) ||
                 (tk.TenTaiKhoan!.Contains(tuKhoa) || tk.MatKhauHash!.Contains(tuKhoa) ||
          tk.MaVaiTroNavigation!.TenVaiTro.Contains(tuKhoa)))
          .ToList()
          .Select((x, Index) => new QLTaiKhoan_DTO
          {
              STT = Index + 1,
              MaTK = x.MaTaiKhoan,
              TenTK = x.TenTaiKhoan,
              MaTKhau = x.MatKhauHash,
              TrangThai = x.TrangThai,
              VaiTro = x.MaVaiTroNavigation != null ? x.MaVaiTroNavigation.TenVaiTro : "Không có"
          }).ToList();



        }
        public List<QLTaiKhoan_DTO> LocAddmin()
        {
            return _context.TaiKhoans.Include(tk => tk.MaVaiTroNavigation)
                     .Where(tk => tk.MaVaiTroNavigation.MaVaiTro == 1)
                     .ToList()
                     .Select((x, Index) => new QLTaiKhoan_DTO
                     {
                         STT = Index + 1,
                         MaTK = x.MaTaiKhoan,
                         TenTK = x.TenTaiKhoan,
                         MaTKhau = x.MatKhauHash,
                         TrangThai = x.TrangThai,
                         VaiTro = x.MaVaiTroNavigation != null ? x.MaVaiTroNavigation.TenVaiTro : "Không có"
                     }).ToList();
        }
        public List<QLTaiKhoan_DTO> LocNhanVien()
        {
            return _context.TaiKhoans.Include(tk => tk.MaVaiTroNavigation)
                     .Where(tk => tk.MaVaiTroNavigation.MaVaiTro == 2)
                     .ToList()

                     .Select((x, Index) => new QLTaiKhoan_DTO
                     {
                         STT = Index + 1,
                         MaTK = x.MaTaiKhoan,
                         TenTK = x.TenTaiKhoan,
                         MaTKhau = x.MatKhauHash,
                         TrangThai = x.TrangThai,
                         VaiTro = x.MaVaiTroNavigation != null ? x.MaVaiTroNavigation.TenVaiTro : "Không có"
                     }).ToList();
        }
        public bool CapNhatTK(TaiKhoan tkCapNhat)
        {
            try
            {
                var tk = _context.TaiKhoans.FirstOrDefault(t => t.MaTaiKhoan == tkCapNhat.MaTaiKhoan);
                if (tk == null) return false;

                // Cập nhật tên
                tk.TenTaiKhoan = tkCapNhat.TenTaiKhoan ?? tk.TenTaiKhoan;

                // Cập nhật MẬT KHẨU (đã được băm sẵn ở BLL)
                tk.MatKhauHash = tkCapNhat.MatKhauHash;

                // Cập nhật trạng thái
                tk.TrangThai = tkCapNhat.TrangThai ?? tk.TrangThai;
                // Cập nhật vai trò
                if (tkCapNhat.MaVaiTro.HasValue)
                    tk.MaVaiTro = tkCapNhat.MaVaiTro.Value;

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi DAL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
