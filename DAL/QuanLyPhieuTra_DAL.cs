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
    internal class QuanLyPhieuTra_DAL
    {
        private ThuVienNhom3Context _context =new ThuVienNhom3Context();
        public List<QuanLyPhieuTra_DTO> GetListPhieuTra()
        {
            var listKetQua = _context.PhieuMuons.Include(pm => pm.MaNhanVienNavigation)
                                                .Include(pm => pm.MaDocGiaNavigation)
                                                .Include(pm => pm.ChiTietPhieuMuons) 
                                                .ThenInclude(ctpm => ctpm.MaSachNavigation)
                                                .Where(pm => pm.TrangThai == "Đã trả")
                                                .SelectMany(pm => pm.ChiTietPhieuMuons.Select(ctpm => new QuanLyPhieuTra_DTO()
                                                {
                                                    MaPhieuMuon = pm.MaPhieuMuon,
                                                    ThoiGianMuon = pm.ThoiGianMuon,
                                                    ThoihanTra = pm.ThoihanTra,
                                                    SoLuong = pm.SoLuongBanDau,
                                                    TrangThai = pm.TrangThai,
                                                    Sach = ctpm.MaSachNavigation.TenSach,
                                                    SoLuongMuon = ctpm.SoLuongMuon,
                                                    TinhTrangSach = ctpm.TinhTrangSach,
                                                    NhanVien = pm.MaNhanVienNavigation.TenNhanVien,
                                                    DocGia = pm.MaDocGiaNavigation.TenDocGia,
                                                    NgayTraSach = ctpm.NgayTraSach.GetValueOrDefault()
                                                }))
                                                .ToList();
            var listKetQuaCoSTT = listKetQua
                .Select((x, index) =>
                {
                    x.STT = index + 1;
                    return x;
                })
                .ToList();

            return listKetQuaCoSTT;
        }
        public List<QuanLyPhieuTra_DTO> timKiem(string tukhoa)
        {
            tukhoa = tukhoa?.Trim().ToLower() ?? "";
            var danhSach = GetListPhieuTra();
            if (string.IsNullOrEmpty(tukhoa))
                return danhSach;

            var ketQua = danhSach.Where(x =>
                   x.MaPhieuMuon.ToString().Contains(tukhoa)
                || (x.TrangThai != null && x.TrangThai.ToLower().Contains(tukhoa))
                || (x.Sach != null && x.Sach.ToLower().Contains(tukhoa))
                || (x.NhanVien != null && x.NhanVien.ToLower().Contains(tukhoa))
                || (x.DocGia != null && x.DocGia.ToLower().Contains(tukhoa))
                || (x.TinhTrangSach != null && x.TinhTrangSach.ToLower().Contains(tukhoa))
                || (x.SoLuongMuon != null && x.SoLuongMuon.ToString().Contains(tukhoa))
                || (x.SoLuong != null && x.SoLuong.ToString().Contains(tukhoa))
                || (x.ThoiGianMuon != null && x.ThoiGianMuon.Value.ToString("dd/MM/yyyy").Contains(tukhoa))
                || (x.ThoihanTra != null && x.ThoihanTra.Value.ToString("dd/MM/yyyy").Contains(tukhoa))
            ).ToList();

            return ketQua;
        }

    }
}
