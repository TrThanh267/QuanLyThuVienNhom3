using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using QuanLyThuVienNhom3.DTO;
using QuanLyThuVienNhom3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVienNhom3.DAL
{
    internal class QuanLyPhieuMuonChiTiet_DAL
    {
        public string LastError { get; private set; } = string.Empty;
        private ThuVienNhom3Context _context = new ThuVienNhom3Context();
        public List<QuanLyPhieuMuonChiTiet_DTO> GetListPMCT()
        {
            var listPMCT = _context.ChiTietPhieuMuons.Include(x => x.MaPhieuMuonNavigation)
                                                    .Include(x => x.MaSachNavigation)
                                                    .Where(x => x.MaPhieuMuonNavigation != null
                                                             && x.MaPhieuMuonNavigation.TrangThai != null
                                                             && x.MaPhieuMuonNavigation.TrangThai.ToLower() != "đã trả")
                                                    .Select(x => new QuanLyPhieuMuonChiTiet_DTO
                                                    {
                                                        MaChiTietPhieuMuon = x.MaChiTietPhieuMuon,
                                                        SoLuongMuon = x.SoLuongMuon,
                                                        TinhTrangSach = x.TinhTrangSach,
                                                        Sach = x.MaSachNavigation.TenSach,
                                                        MaPhieuMuon = x.MaPhieuMuon,
                                                        DaGhiNhanTra = x.DaGhiNhanTra ?? false,
                                                    })
                                                    .ToList();

            for (int i = 0; i < listPMCT.Count; i++)
            {
                listPMCT[i].STT = i + 1;
            }

            return listPMCT;

        }
        public int LaySoLuongNoThucTe(int maPhieuMuon)
        {
            var pm = _context.PhieuMuons
                             .AsNoTracking()
                             .FirstOrDefault(x => x.MaPhieuMuon == maPhieuMuon);
            return pm?.SoLuong ?? 0;
        }
        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }
        public Sach GetSachByID(int idSach)
        {
            return _context.Saches.FirstOrDefault(x => x.MaSach == idSach);
        }
        public List<Sach> GetSaches()
        {
            return _context.Saches.ToList();
        }
        public List<PhieuMuon> GetPhieuMuons()
        {
            return _context.PhieuMuons
                   .Where(pm => pm.TrangThai.ToLower() != "Đã trả")
                   .ToList();
        }
        public bool KiemTraSoLuongMuon(ChiTietPhieuMuon chiTietPhieuMuon)
        {
            var phieuMuon = _context.PhieuMuons
                .FirstOrDefault(x => x.MaPhieuMuon == chiTietPhieuMuon.MaPhieuMuon);

            if (phieuMuon == null)
                return false; 

            int tong = _context.ChiTietPhieuMuons
                        .Where(ct => ct.MaPhieuMuon == chiTietPhieuMuon.MaPhieuMuon)
                        .Sum(ct => (int?)ct.SoLuongMuon)
                        .GetValueOrDefault();

            tong += chiTietPhieuMuon.SoLuongMuon ?? 0;
            return tong <= phieuMuon.SoLuongBanDau;
        }
        public bool KiemTraSoLuongSachConLaiTrongKho(ChiTietPhieuMuon chiTietPhieuMuon, int soLuongCu = 0)
        {
            var sach = _context.Saches.FirstOrDefault(x => x.MaSach == chiTietPhieuMuon.MaSach);
            if (sach == null)
            {
                MessageBox.Show("Không tìm thấy sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            _context.Entry(sach).Reload();
            int soLuongMuonMoi = chiTietPhieuMuon.SoLuongMuon ?? 0;
            int soLuongKhoHienTai = sach.SoLuong ?? 0;
            int soLuongKhoSauKhiTraLaiCu = soLuongKhoHienTai + soLuongCu;
            int soLuongConLai = soLuongKhoSauKhiTraLaiCu - soLuongMuonMoi;
            if (soLuongConLai < 0)
            {
                MessageBox.Show($"Số lượng mượn mới ({soLuongMuonMoi}) vượt quá số lượng sách có thể mượn ({soLuongKhoSauKhiTraLaiCu})!",
                                "Lỗi Tồn kho", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            sach.SoLuong = soLuongConLai;
            sach.TrangThai = (soLuongConLai == 0) ? "Hết sách" : "Còn sách";
            _context.SaveChanges();
            if (soLuongConLai > 0)
            {
                MessageBox.Show($"Sách còn lại: {sach.SoLuong}", "Cập nhật Kho", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return true;
        }

        public bool ThemPMCT(ChiTietPhieuMuon chiTietPhieuMuon)
        {
            try
            {
                _context.ChiTietPhieuMuons.Add(chiTietPhieuMuon);
                _context.SaveChanges();
                MessageBox.Show(
                    "Thêm phiếu mượn chi tiết thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Thêm phiếu mượn chi tiết thất bại! Lỗi: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
        }

        public bool TraSach(int maChiTietPhieuMuon)
        {
            var chiTiet = _context.ChiTietPhieuMuons
                                  .Include(ct => ct.MaSachNavigation)
                                  .Include(ct => ct.MaPhieuMuonNavigation)
                                  .FirstOrDefault(ct => ct.MaChiTietPhieuMuon == maChiTietPhieuMuon);

            if (chiTiet == null) return false;
            var sach = chiTiet.MaSachNavigation;
            if (sach != null)
            {
                sach.SoLuong = (sach.SoLuong ?? 0) + (chiTiet.SoLuongMuon ?? 0);

                if (sach.SoLuong > 0 && sach.TrangThai == "Hết sách")
                {
                    sach.TrangThai = "Còn sách";
                }
            }
            chiTiet.TinhTrangSach = "Đã trả";
            var phieuMuon = chiTiet.MaPhieuMuonNavigation;
            bool tatCaDaTra = phieuMuon.ChiTietPhieuMuons.All(ct => ct.TinhTrangSach == "Đã trả");

            if (tatCaDaTra)
            {
                phieuMuon.TrangThai = "Đã trả";
            }

            _context.SaveChanges();
            return true;
        }
        public bool XuLyTraSachHoanThanh(int maChiTietPhieuMuon, string tinhTrangSach)
        {
            LastError = string.Empty;

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var chiTiet = _context.ChiTietPhieuMuons
                                       .Include(ct => ct.MaSachNavigation)
                                       .Include(ct => ct.MaPhieuMuonNavigation)
                                       .FirstOrDefault(ct => ct.MaChiTietPhieuMuon == maChiTietPhieuMuon);
                var phieuMuon = chiTiet.MaPhieuMuonNavigation;
                int soLuongDaTra = chiTiet.SoLuongMuon ?? 0;
                bool daXuLyTruocDo = chiTiet.DaGhiNhanTra.GetValueOrDefault();
                chiTiet.TinhTrangSach = tinhTrangSach;
                if (!daXuLyTruocDo)
                {
                    if (phieuMuon.SoLuong.HasValue)
                    {
                        phieuMuon.SoLuong -= soLuongDaTra;
                    }
                    if (chiTiet.MaSachNavigation != null)
                    {
                        chiTiet.MaSachNavigation.SoLuong += soLuongDaTra;
                    }
                    chiTiet.DaGhiNhanTra = true;
                }
                bool daTraDuTatCa = false;
                if (phieuMuon.SoLuong <= 0)
                {
                    phieuMuon.SoLuong = 0;
                    phieuMuon.TrangThai = "Đã trả";
                    daTraDuTatCa = true;
                }

                _context.SaveChanges();
                transaction.Commit();
                return daTraDuTatCa;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                LastError = "Lỗi hệ thống khi xử lý trả sách: " + ex.Message;
                return false;
            }
        }
        public bool GiamSoLuongNoPM(int maPhieuMuon, int soLuongGiam)
        {
            int affectedRows = _context.PhieuMuons
                .Where(pm => pm.MaPhieuMuon == maPhieuMuon)
                .ExecuteUpdate(setter => setter.SetProperty(
                    pm => pm.SoLuong,
                    pm => pm.SoLuong - soLuongGiam
                ));
            return affectedRows == 1;
        }
        public int CongTonKho(int maSach, int soLuongDaTra)
        {
            return _context.Saches
            .Where(s => s.MaSach == maSach)
            .ExecuteUpdate(setter => setter
            .SetProperty(
                s => s.SoLuong,         
                s => s.SoLuong + soLuongDaTra
            )
            .SetProperty(
                s => s.TrangThai,        
                "Còn sách"  
            )
        );
        }
        public void CapNhatTrangThaiPM(int maPhieuMuon, string trangThaiMoi)
        {
            _context.PhieuMuons
                .Where(p => p.MaPhieuMuon == maPhieuMuon)
                .ExecuteUpdate(setter => setter
                    .SetProperty(p => p.TrangThai, trangThaiMoi)
                    .SetProperty(p => p.SoLuong, 0)
                );
        }
        
        public void SuaChiTietPhieuMuon(ChiTietPhieuMuon ctpm)
        {
            var existingCtpm = _context.ChiTietPhieuMuons.Find(ctpm.MaChiTietPhieuMuon);

            if (existingCtpm != null)
            {
                existingCtpm.SoLuongMuon = ctpm.SoLuongMuon;
                _context.SaveChanges();
            }
        }
        public int CapNhatSoLuongNoAtomic(int maPhieuMuon, int deltaSoLuong)
        {
            return _context.PhieuMuons
                .Where(p => p.MaPhieuMuon == maPhieuMuon)
                .ExecuteUpdate(setter => setter.SetProperty(
                    p => p.SoLuong,
                    p => p.SoLuong + deltaSoLuong
                ));
        }
        public bool CapNhatTonKho(int maSach, int soLuongThayDoi)
        {
            try
            {
                var sach = _context.Saches.Find(maSach);
                if (sach != null)
                {
                    sach.SoLuong += soLuongThayDoi;

                    if (sach.SoLuong < 0)
                    {
                        return false;
                    }
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool CapNhatTonKhoAtomic(int maSach, int soLuongThayDoi)
        {
            int affectedRows = _context.Saches
                .Where(s => s.MaSach == maSach)
                .ExecuteUpdate(setter => setter.SetProperty(
                    s => s.SoLuong,
                    s => s.SoLuong + soLuongThayDoi
                ));
            return affectedRows == 1;
        }
        public ChiTietPhieuMuon GetCTPMById(int ctpmid)
        {
            return _context.ChiTietPhieuMuons
                           .FirstOrDefault(x => x.MaChiTietPhieuMuon == ctpmid);
        }
        public PhieuMuon GetPMById(int pmid)
        {
            return _context.PhieuMuons
                           .FirstOrDefault(x => x.MaPhieuMuon == pmid);
        }
        public bool XoaChiTietPhieuMuon(int maChiTietPhieuMuon)
        {
            try
            {
                var ctpm = _context.ChiTietPhieuMuons.FirstOrDefault(x => x.MaChiTietPhieuMuon == maChiTietPhieuMuon);
                if (ctpm != null)
                {
                    _context.ChiTietPhieuMuons.Remove(ctpm);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<QuanLyPhieuMuonChiTiet_DTO> TimKiemPMCT(string tuKhoa)
        {
            return _context.ChiTietPhieuMuons.Include(x => x.MaSachNavigation)
                                      .Include(x => x.MaPhieuMuonNavigation)
                                    .Where(x => x.MaPhieuMuon.ToString().Equals(tuKhoa) ||
                                                x.MaChiTietPhieuMuon.ToString().Equals(tuKhoa) ||
                                                 (x.MaSachNavigation.TenSach!.Contains(tuKhoa) ||
                                                  x.TinhTrangSach!.Contains(tuKhoa)
                                                  )).ToList()
                                    .Select((x, Index) => new QuanLyPhieuMuonChiTiet_DTO
                                    {
                                        STT = Index + 1,
                                        MaChiTietPhieuMuon = x.MaChiTietPhieuMuon,
                                        SoLuongMuon = x.SoLuongMuon,
                                        TinhTrangSach = x.TinhTrangSach,
                                        Sach = x.MaSachNavigation.TenSach,
                                        MaPhieuMuon = x.MaPhieuMuon,
                                        DaGhiNhanTra = x.DaGhiNhanTra,
                                    }).ToList();
        }
        public void CapNhatSach(Sach sachDaSua)
        {
            try
            {
                _context.Saches.Update(sachDaSua);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật thông tin sách vào DB.", ex);
            }
        }


    }
}