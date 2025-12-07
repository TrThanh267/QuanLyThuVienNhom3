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
    internal class QuanLyChamCong_DAL
    {
        private readonly ThuVienNhom3Context _context = new ThuVienNhom3Context();
        public List<QuanLyChamCong_DTO> GetListChamCong()
        {
            return _context.ChamCongs
                .Include(cc => cc.MaLoaiCongNavigation)
                .Include(cc => cc.MaCaLamNavigation)
                .Include(cc => cc.MaNhanVienNavigation)
                .ToList()
                .Select((x, index) => new QuanLyChamCong_DTO
                {
                    STT = index + 1,
                    MaChamCong = x.MaChamCong,
                    NgayLam = x.NgayLam,
                    ThangLam = x.ThangLam,
                    NamLam = x.NamLam,
                    GioVao = x.GioVao,
                    PhutVao = x.PhutVao,
                    GioRa = x.GioRa,
                    PhutRa = x.PhutRa,
                    LoaiCong = x.MaLoaiCongNavigation?.TenloaiCong,
                    CaLam = x.MaCaLamNavigation?.TenCaLam,
                    NhanVien = x.MaNhanVienNavigation?.TenNhanVien,
                    MaNhanVien = x.MaNhanVienNavigation?.MaNhanVien,
                }).ToList();
        }
        public bool ThemChamCong(ChamCong chamCong)
        {
            try
            {
                _context.ChamCongs.Add(chamCong);
                _context.SaveChanges();
                MessageBox.Show("Thêm chấm công thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm chấm công thất bại! Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public bool KiemTraChamCongTonTai(int maNhanVien)
        {
            var today = DateTime.Today;
            return _context.ChamCongs.Any(cc => cc.MaNhanVien == maNhanVien
                                            && cc.NgayLam == today.Day
                                            && cc.ThangLam == today.Month && cc.NamLam == today.Year);

        }
        public bool KiemTraChamCongVoiNhanVien(int IdNhanVien)
        {
            var nhanVien = _context.NhanViens
                            .FirstOrDefault(nv => nv.MaNhanVien == IdNhanVien);

            if (nhanVien == null)
            {
                MessageBox.Show("Nhân viên không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return nhanVien.MaTaiKhoanNavigation.MaVaiTro == 2;

        }
        public NhanVien LayThongTinNhanVienQuaTaiKhoan(int maTaiKhoan)
        {
            var nhanVien = _context.NhanViens
                            .Include(nv => nv.MaTaiKhoanNavigation)
                            .FirstOrDefault(nv => nv.MaTaiKhoan == maTaiKhoan);


            if (nhanVien == null)
            {
                MessageBox.Show("Không tìm thấy nhân viên cho tài khoản này!",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return nhanVien;
        }
        public List<NhanVien> GetListNhanVien()
        {
            return _context.NhanViens.Include(x => x.MaTaiKhoanNavigation)
                                    .Where(x => x.MaTaiKhoanNavigation.MaVaiTro == 2).ToList();
        }
        public List<CaLam> GetListCaLam()
        {
            return _context.CaLams.ToList();
        }
        public List<LoaiCong> GetListLoaiCong()
        {
            return _context.LoaiCongs.ToList();
        }
        public List<QuanLyChamCong_DTO> LocChamCong(int ngay, int thang, int nam)
        {
            return _context.ChamCongs.Include(x => x.MaCaLamNavigation)
                                      .Include(x => x.MaLoaiCongNavigation)
                                      .Include(x => x.MaNhanVienNavigation)
                                      .ToList().Where(x => x.NgayLam == ngay && x.ThangLam == thang && x.NamLam == nam)
                                      .Select((x, index) => new QuanLyChamCong_DTO
                                      {
                                          STT = index + 1,
                                          MaChamCong = x.MaChamCong,
                                          NgayLam = x.NgayLam,
                                          ThangLam = x.ThangLam,
                                          NamLam = x.NamLam,
                                          GioVao = x.GioVao,
                                          PhutVao = x.PhutVao,
                                          GioRa = x.GioRa,
                                          PhutRa = x.PhutRa,
                                          LoaiCong = x.MaLoaiCongNavigation.TenloaiCong,
                                          CaLam = x.MaCaLamNavigation.TenCaLam,
                                          NhanVien = x.MaNhanVienNavigation.TenNhanVien,
                                          MaNhanVien = x.MaNhanVienNavigation.MaNhanVien
                                      }).ToList();
        }
        public bool CapNhapChamCong(ChamCong newCC)
        {
            try
            {
                var oldCC = _context.ChamCongs.FirstOrDefault(cc => cc.MaChamCong == newCC.MaChamCong);
                if (oldCC == null)
                {
                    MessageBox.Show("Không tìm thấy bản ghi chấm công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                oldCC.MaCaLam = newCC.MaCaLam == null ? oldCC.MaCaLam : newCC.MaCaLam;
                oldCC.MaLoaiCong = newCC.MaLoaiCong == null ? oldCC.MaLoaiCong : newCC.MaLoaiCong;
                oldCC.NamLam = newCC.NamLam == null ? oldCC.NamLam : newCC.NamLam;
                oldCC.ThangLam = newCC.ThangLam == null ? oldCC.ThangLam : newCC.ThangLam;
                oldCC.NgayLam = newCC.NgayLam == null ? oldCC.NgayLam : newCC.NgayLam;
                oldCC.MaNhanVien = newCC.MaNhanVien == null ? oldCC.MaNhanVien : newCC.MaNhanVien;
                oldCC.GioRa = newCC.GioRa == null ? oldCC.GioRa : newCC.GioRa;
                oldCC.PhutRa = oldCC.PhutRa == null ? oldCC.PhutRa : newCC.PhutRa;
                _context.SaveChanges();
                MessageBox.Show("Cập nhật chấm công thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật chấm công: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }
        public int KiemTraNgayCongLamChinh(int thang, int nam, int idnv)
        {
            return _context.ChamCongs
                .Count(cc => cc.MaNhanVien == idnv
                && cc.ThangLam == thang
                && cc.NamLam == nam
                && cc.MaLoaiCongNavigation.TenloaiCong == "Làm chính"
                && cc.GioVao != null
                && cc.PhutVao != null
                && cc.GioRa != null
                && cc.PhutRa != null);
        }
        public int KiemTraNgayCongLamNgoaiGio(int thang, int nam, int idnv)
        {
            return _context.ChamCongs
                .Count(cc => cc.MaNhanVien == idnv
                && cc.ThangLam == thang
                && cc.NamLam == nam
                && cc.MaLoaiCongNavigation.TenloaiCong == "Làm thêm"
                && cc.GioVao != null
                && cc.PhutVao != null
                && cc.GioRa != null
                && cc.PhutRa != null);
        }
        public int KiemTraNgayCongLamNgaayLe(int thang, int nam, int idnv)
        {
            return _context.ChamCongs
                .Count(cc => cc.MaNhanVien == idnv
                && cc.ThangLam == thang
                && cc.NamLam == nam
                && cc.MaLoaiCongNavigation.TenloaiCong == "Làm ngày lễ"
                && cc.GioVao != null
                && cc.PhutVao != null
                && cc.GioRa != null
                && cc.PhutRa != null);
        }
        public List<QuanLyChamCong_DTO> TimKiem(string tk)
        {
            return _context.ChamCongs.Include(nv => nv.MaNhanVienNavigation)
                 .Include(lc => lc.MaLoaiCongNavigation).ToList()
                 .Where(x=>x.MaNhanVien.ToString().Contains(tk) ||
                        x.MaChamCong.ToString().Contains(tk) ||
                        (x.MaNhanVienNavigation.TenNhanVien != null && x.MaNhanVienNavigation.TenNhanVien.ToLower().Contains(tk)) ||
                        (x.MaCaLamNavigation.TenCaLam != null && x.MaCaLamNavigation.TenCaLam.ToLower().Contains(tk)) ||
                        (x.MaLoaiCongNavigation.TenloaiCong != null && x.MaLoaiCongNavigation.TenloaiCong.ToLower().Contains(tk)))
                 .Select((x, index) => new QuanLyChamCong_DTO
                 {
                     STT = index + 1,
                     MaChamCong = x.MaChamCong,
                     NamLam = x.NamLam,
                     ThangLam = x.ThangLam,
                     NgayLam = x.NgayLam,
                     GioVao = x.GioVao,
                     PhutVao = x.PhutVao,
                     GioRa = x.GioRa,
                     PhutRa = x.PhutRa,
                     CaLam = x.MaCaLamNavigation.TenCaLam,
                     LoaiCong = x.MaLoaiCongNavigation.TenloaiCong,
                     MaNhanVien = x.MaNhanVien,
                     NhanVien = x.MaNhanVienNavigation.TenNhanVien
                 }).ToList();
        }
        public void XoaChamCong(int idCC)
        {
            try
            {
                var chamCong = _context.ChamCongs.FirstOrDefault(cc => cc.MaChamCong == idCC);
                if (chamCong != null)
                {
                    _context.ChamCongs.Remove(chamCong);
                    _context.SaveChanges();
                    MessageBox.Show("Xóa chấm công thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy bản ghi chấm công để xóa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa chấm công: " + ex.Message);
            }
        }
    }
}
