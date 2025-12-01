using QuanLyThuVienNhom3.DTO;
using QuanLyThuVienNhom3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVienNhom3.DAL
{
    internal class ThongKe_DAL
    {
        private readonly ThuVienNhom3Context _context = new ThuVienNhom3Context();
        public List<ThongKe_DTO> GetThongKePhieuMuon()
        {
            var ngayBatDau = DateOnly.FromDateTime(DateTime.Now.AddDays(-30));

            var ThongKe = _context.PhieuMuons
                                    .Where(pm => pm.ThoiGianMuon.HasValue && pm.ThoiGianMuon.Value >= ngayBatDau)
                                    .GroupBy(pm => pm.ThoiGianMuon.Value)
                                    .Select(g => new ThongKe_DTO
                                    {
                                        NgayMuon = g.Key,
                                        TongSoLuotMuon = g.Count()
                                    })
                                    .OrderBy(dto => dto.NgayMuon)
                                    .ToList();
            return ThongKe;
        }
        public List<ThongKe_DTO> GetThongKePhieuMuon(DateOnly ngayBatDau, DateOnly ngayKetThuc)
        {
            var ThongKe = _context.PhieuMuons
                .Where(pm => pm.ThoiGianMuon.HasValue
                          && pm.ThoiGianMuon.Value >= ngayBatDau
                          && pm.ThoiGianMuon.Value <= ngayKetThuc)
                .GroupBy(pm => pm.ThoiGianMuon.Value)
                .Select(g => new ThongKe_DTO
                {
                    NgayMuon = g.Key,
                    TongSoLuotMuon = g.Count()
                })
                .OrderBy(dto => dto.NgayMuon)
                .ToList();

            return ThongKe;
        }
        public int DemSoMuonCaoNhat()
        {
            int maxCount = _context.PhieuMuons
                .Where(pm => pm.ThoiGianMuon.HasValue)
                .AsEnumerable() 
                .GroupBy(pm => pm.ThoiGianMuon.Value)
                .Select(g => g.Count())
                .DefaultIfEmpty(0)
                .Max();

            return maxCount;

        }

    }
}
