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
    internal class QuanLyNhanVien_DAL
    {
        private readonly ThuVienNhom3Context _context = new ThuVienNhom3Context();
        public List<QuanLyNhanVien_DTO> GetListNhanVien()
        {
            return _context.NhanViens.Include(nv => nv.MaTaiKhoanNavigation)
                                    .Where(nv => nv.MaTaiKhoanNavigation.MaVaiTro == 2)
                                    .ToList()
                                    .Select((x, Index) => new QuanLyNhanVien_DTO
                                    {
                                        STT = Index + 1,
                                        MaNhanVien = x.MaNhanVien,
                                        TenNhanVien = x.TenNhanVien,
                                        NgaySinh = x.NgaySinh,
                                        GioiTinh = x.GioiTinh,
                                        DiaChi = x.DiaChi,
                                        SoDienThoai = x.SoDienThoai,
                                        Email = x.Email,
                                        TrangThai = x.TrangThai,
                                        TaiKhoan = x.MaTaiKhoanNavigation != null ? x.MaTaiKhoanNavigation.TenTaiKhoan : "Không có"
                                    }).ToList();
        }
        public bool ThemNhanVien(NhanVien nhanVien)
        {
            try
            {
                _context.NhanViens.Add(nhanVien);
                _context.SaveChanges();
                MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm nhân viên thất bại! Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public bool CapNhapNhanVien(NhanVien nhanVien)
        {
            try
            {
                MessageBox.Show("Mã nhân viên cần tìm: " + nhanVien.MaNhanVien);
                var existingNhanVien = _context.NhanViens.FirstOrDefault(x => x.MaNhanVien == nhanVien.MaNhanVien);
                if (existingNhanVien != null)
                {
                    existingNhanVien.TenNhanVien = nhanVien.TenNhanVien == null ? existingNhanVien.TenNhanVien : nhanVien.TenNhanVien;
                    existingNhanVien.NgaySinh = nhanVien.NgaySinh == null ? existingNhanVien.NgaySinh : nhanVien.NgaySinh;
                    existingNhanVien.GioiTinh = nhanVien.GioiTinh == null ? existingNhanVien.GioiTinh : nhanVien.GioiTinh;
                    existingNhanVien.DiaChi = nhanVien.DiaChi == null ? existingNhanVien.DiaChi : nhanVien.DiaChi;
                    existingNhanVien.SoDienThoai = nhanVien.SoDienThoai == null ? existingNhanVien.SoDienThoai : nhanVien.SoDienThoai;
                    existingNhanVien.Email = nhanVien.Email == null ? existingNhanVien.Email : nhanVien.Email;
                    existingNhanVien.TrangThai = nhanVien.TrangThai == null ? existingNhanVien.TrangThai : nhanVien.TrangThai;
                    existingNhanVien.MaTaiKhoan = nhanVien.MaTaiKhoan == null ? existingNhanVien.MaTaiKhoan : nhanVien.MaTaiKhoan;
                    _context.NhanViens.Update(existingNhanVien);
                    _context.SaveChanges();
                    MessageBox.Show("Cập nhật nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Nhân viên không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cập nhật nhân viên thất bại! Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public bool XoaNhanVien(int maNhanVien)
        {
            try
            {
                var nhanVien = _context.NhanViens.FirstOrDefault(x => x.MaNhanVien == maNhanVien);
                if (nhanVien != null)
                {
                    _context.NhanViens.Remove(nhanVien);
                    _context.SaveChanges();
                    MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Nhân viên không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa nhân viên thất bại! Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public bool KiemTraNhanVienVoiPhieuMuon(int maNhanVien)
        {
            return _context.PhieuMuons.Any(pm => pm.MaNhanVien == maNhanVien);
        }
        public bool KiemTraNhanVienVoiChamCong(int maNhanVien)
        {
            return _context.ChamCongs.Any(cc => cc.MaNhanVien == maNhanVien);
        }
        public bool KiemTraSoDienToaiTonTai(string SDT)
        {
            return _context.NhanViens.Any(nv => nv.SoDienThoai == SDT);
        }
        public bool KiemTraEmailTonTai(string email)
        {
            return _context.NhanViens.Any(nv => nv.Email == email);
        }
        public bool KiemTraTaiKhoanDangKy(int? maTaiKhoan)
        {
            return _context.NhanViens.Any(nv => nv.MaTaiKhoan == maTaiKhoan);
        }
        public List<TaiKhoan> TaiKhoanNhanVien()
        {
            return _context.TaiKhoans.Where(tk => tk.MaVaiTro == 2 && tk.TrangThai == "Hoạt động").ToList();
        }
        public List<QuanLyNhanVien_DTO> nhanVienDangHoatDong()
        {
            return _context.NhanViens.Include(nv => nv.MaTaiKhoanNavigation)
                                    .Where(nv => nv.TrangThai == "Đang làm việc" && nv.MaTaiKhoanNavigation.MaVaiTro == 2)
                                    .ToList()
                                    .Select((x, Index) => new QuanLyNhanVien_DTO
                                    {
                                        STT = Index + 1,
                                        MaNhanVien = x.MaNhanVien,
                                        TenNhanVien = x.TenNhanVien,
                                        NgaySinh = x.NgaySinh,
                                        GioiTinh = x.GioiTinh,
                                        DiaChi = x.DiaChi,
                                        SoDienThoai = x.SoDienThoai,
                                        Email = x.Email,
                                        TrangThai = x.TrangThai,
                                        TaiKhoan = x.MaTaiKhoanNavigation != null ? x.MaTaiKhoanNavigation.TenTaiKhoan : "Không có"
                                    }).ToList();
        }
        public List<QuanLyNhanVien_DTO> nhanVienNgungHoatDong()
        {
            return _context.NhanViens.Include(nv => nv.MaTaiKhoanNavigation)
                                    .Where(nv => nv.TrangThai == "Đã nghỉ" && nv.MaTaiKhoanNavigation.MaVaiTro == 2)
                                    .ToList()
                                    .Select((x, Index) => new QuanLyNhanVien_DTO
                                    {
                                        STT = Index + 1,
                                        MaNhanVien = x.MaNhanVien,
                                        TenNhanVien = x.TenNhanVien,
                                        NgaySinh = x.NgaySinh,
                                        GioiTinh = x.GioiTinh,
                                        DiaChi = x.DiaChi,
                                        SoDienThoai = x.SoDienThoai,
                                        Email = x.Email,
                                        TrangThai = x.TrangThai,
                                        TaiKhoan = x.MaTaiKhoanNavigation != null ? x.MaTaiKhoanNavigation.TenTaiKhoan : "Không có"
                                    }).ToList();
        }

        public List<QuanLyNhanVien_DTO> TimKiemNhanVien(string tuKhoa)
        {
            return _context.NhanViens.Include(nv => nv.MaTaiKhoanNavigation)
                                    .Where(nv => nv.MaTaiKhoan.ToString().Equals(tuKhoa) ||
                                                 (nv.TenNhanVien!.Contains(tuKhoa) ||
                                                  nv.SoDienThoai!.Contains(tuKhoa) ||
                                                  nv.MaTaiKhoanNavigation!.TenTaiKhoan.Contains(tuKhoa) ||
                                                  nv.Email!.Contains(tuKhoa)))
                                    .ToList()
                                    .Select((x, Index) => new QuanLyNhanVien_DTO
                                    {
                                        STT = Index + 1,
                                        MaNhanVien = x.MaNhanVien,
                                        TenNhanVien = x.TenNhanVien,
                                        NgaySinh = x.NgaySinh,
                                        GioiTinh = x.GioiTinh,
                                        DiaChi = x.DiaChi,
                                        SoDienThoai = x.SoDienThoai,
                                        Email = x.Email,
                                        TrangThai = x.TrangThai,
                                        TaiKhoan = x.MaTaiKhoanNavigation != null ? x.MaTaiKhoanNavigation.TenTaiKhoan : "Không có"
                                    }).ToList();
        }
    }
}
