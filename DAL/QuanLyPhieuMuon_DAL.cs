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
    internal class QuanLyPhieuMuon_DAL
    {
        private ThuVienNhom3Context _context = new ThuVienNhom3Context();
        public List<QuanLyPhieuMuon_DTO> GetListPhieuMuon()
        {
            CapNhatTrangThaiQuaHan();

            var listPM = _context.PhieuMuons
                                 .Include(x => x.MaNhanVienNavigation)
                                 .Include(x => x.MaDocGiaNavigation)
                                 .Where(x => x.TrangThai != null && x.TrangThai.ToLower() != "đã trả")
                                 .Select(x => new QuanLyPhieuMuon_DTO
                                 {
                                     MaPhieuMuon = x.MaPhieuMuon,
                                     ThoiGianMuon = x.ThoiGianMuon,
                                     ThoihanTra = x.ThoihanTra,
                                     SoLuong = x.SoLuong,
                                     TrangThai = x.TrangThai,
                                     NhanVien = x.MaNhanVienNavigation.TenNhanVien,
                                     DocGia = x.MaDocGiaNavigation.TenDocGia,
                                     EmailDocGia = x.MaDocGiaNavigation.Email
                                 })
                                 .ToList();
            for (int i = 0; i < listPM.Count; i++)
            {
                listPM[i].STT = i + 1;
            }

            return listPM;


        }
        public bool KiemTraPhieuMuonXemCoTrongPMCT(int idPM)
        {
            return _context.ChiTietPhieuMuons.Any(x=>x.MaPhieuMuon == idPM);
        }
        
        public List<NhanVien> GetNhanViens()
        {
            return _context.NhanViens.ToList();
        }
        public List<DocGium> GetDocGia()
        {
            return _context.DocGia.ToList();
        }
        public bool ThemPhieuMuon(PhieuMuon phieuMuon)
        {
            try
            {
                _context.PhieuMuons.Add(phieuMuon);
                _context.SaveChanges();
                MessageBox.Show(
                    "Thêm phiếu mượn thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Thêm phiếu mượn thất bại! Lỗi: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
        }
        public List<PhieuMuon> GetPhieuMuon()
        {
            return _context.PhieuMuons
                           .Include(pm => pm.MaNhanVienNavigation)
                           .Include(pm => pm.MaDocGiaNavigation)
                           .ToList();
        }
        public void CapNhatTrangThaiQuaHan()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            var phieuMuons = _context.PhieuMuons
                                     .Where(pm => pm.TrangThai != "Đã trả")
                                     .ToList();

            foreach (var pm in phieuMuons)
            {
                if (today > pm.ThoihanTra)
                {
                    pm.TrangThai = "Quá hạn";
                }
            }
            _context.SaveChanges();
        }
        public PhieuMuon GetPhieuMuonByID(int id)
        {
            return _context.PhieuMuons.FirstOrDefault(x => x.MaPhieuMuon == id);
        }
        public void XoaPhieuMuon(int maphieuMuon)
        {
            try
            {
                var pm = _context.PhieuMuons.FirstOrDefault(x => x.MaPhieuMuon == maphieuMuon);
                if (pm != null)
                {
                    _context.PhieuMuons.Remove(pm);
                    _context.SaveChanges();
                    MessageBox.Show("Xóa phiếu mượn thành công!",
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Phiếu mượn không tồn tại!",
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa phiếu mượn: " + ex.Message);
            }

        }
        public bool KiemTraPhieuMuonDangCoDuLieuBenCTPM(int maphieuMuon)
        {
            return _context.ChiTietPhieuMuons.Any(x=>x.MaPhieuMuon==maphieuMuon);
        }
        public bool KiemTraPMNeuDaTra(int maphieuMuon)
        {
            var phieumuon = _context.PhieuMuons.FirstOrDefault(x => x.MaPhieuMuon == maphieuMuon);

            if (phieumuon == null)
            {
                return false;
            }
            return phieumuon.TrangThai == "Đã trả";
        }
        public bool SuaPhieuMuon(PhieuMuon newPH)
        {
            try
            {
                var oldPM = _context.PhieuMuons.FirstOrDefault(pm => pm.MaPhieuMuon == newPH.MaPhieuMuon);
                if (oldPM != null)
                {
                    oldPM.ThoiGianMuon = newPH.ThoiGianMuon ?? oldPM.ThoiGianMuon;
                    oldPM.ThoihanTra = newPH.ThoihanTra ?? oldPM.ThoihanTra;
                    oldPM.SoLuong = newPH.SoLuong ?? oldPM.SoLuong;
                    oldPM.TrangThai = newPH.TrangThai ?? oldPM.TrangThai;
                    oldPM.MaNhanVien = newPH.MaNhanVien ?? oldPM.MaNhanVien;
                    oldPM.MaDocGia = newPH.MaDocGia ?? oldPM.MaDocGia;

                    _context.PhieuMuons.Update(oldPM);
                    _context.SaveChanges();

                    MessageBox.Show("Sửa phiếu mượn thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return true;
                }
                else
                {
                    MessageBox.Show("Phiếu mượn không tồn tại!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa phiếu mượn: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public bool KiemTraTatCaSachDaTra(int maPhieuMuon)
        {
            var phieuMuon = _context.PhieuMuons
                .FirstOrDefault(pm => pm.MaPhieuMuon == maPhieuMuon);
            if (phieuMuon == null) 
                return false;

            int soLuongDaXuLyTra = _context.ChiTietPhieuMuons
                .Where(ct => ct.MaPhieuMuon == maPhieuMuon && ct.DaGhiNhanTra == true) // <-- Lọc theo cờ mới
                .Sum(ct => (int?)ct.SoLuongMuon)
                .GetValueOrDefault();
            int soLuongGoc = phieuMuon.SoLuongBanDau.GetValueOrDefault();
            return soLuongDaXuLyTra == soLuongGoc;
        }
        public List<QuanLyPhieuMuon_DTO> TimKiemPM(string tuKhoa)
        {
            string tuKhoaLower = tuKhoa.ToLower();

            return _context.PhieuMuons.Include(x => x.MaNhanVienNavigation)
                .Include(x => x.MaDocGiaNavigation)
                .Where(x => x.MaPhieuMuon.ToString().Equals(tuKhoa) || // Giữ nguyên cho Mã PM
                            (
                                (x.MaNhanVienNavigation != null && x.MaNhanVienNavigation.TenNhanVien != null && x.MaNhanVienNavigation.TenNhanVien.ToLower().Contains(tuKhoaLower)) ||
                                (x.MaDocGiaNavigation != null && x.MaDocGiaNavigation.TenDocGia != null && x.MaDocGiaNavigation.TenDocGia.ToLower().Contains(tuKhoaLower)) ||
                                (x.TrangThai != null && x.TrangThai.ToLower().Contains(tuKhoaLower))
                            )
                       )
                .ToList()
                .Select((x, Index) => new QuanLyPhieuMuon_DTO
                {
                    STT = Index + 1,
                    MaPhieuMuon = x.MaPhieuMuon,
                    ThoiGianMuon = x.ThoiGianMuon,
                    ThoihanTra = x.ThoihanTra,
                    SoLuong = x.SoLuong,
                    TrangThai = x.TrangThai,
                    NhanVien = x.MaNhanVienNavigation?.TenNhanVien,
                    DocGia = x.MaDocGiaNavigation?.TenDocGia, 
                    EmailDocGia = x.MaDocGiaNavigation?.Email
                }).ToList();
        }
    }
}
