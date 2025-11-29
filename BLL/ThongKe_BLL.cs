using QuanLyThuVienNhom3.DAL;
using QuanLyThuVienNhom3.DAL;
using QuanLyThuVienNhom3.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVienNhom3.BLL
{
    internal class ThongKe_BLL
    {
        private ThongKe_DAL _DAL = new ThongKe_DAL();
        public List<ThongKe_DTO> ThongKePhieuMuon()
        {
            return _DAL.GetThongKePhieuMuon();
        }
        public List<ThongKe_DTO> ThongKePM(DateOnly ngayBatDau, DateOnly ngayKetThuc)
        {
            List<ThongKe_DTO> duLieuThoTuCSDL = _DAL.GetThongKePhieuMuon(ngayBatDau, ngayKetThuc);

            // 2. TẠO DANH SÁCH TẤT CẢ CÁC NGÀY TRONG KHOẢNG THỜI GIAN
            List<DateOnly> tatCaCacNgay = new List<DateOnly>();
            DateOnly currentDay = ngayBatDau;

            while (currentDay <= ngayKetThuc)
            {
                tatCaCacNgay.Add(currentDay);
                // Tăng ngày lên 1
                currentDay = currentDay.AddDays(1);
            }

            // 3. HỢP NHẤT DỮ LIỆU THÔ VÀ DANH SÁCH NGÀY ĐẦY ĐỦ
            List<ThongKe_DTO> ketQuaCuoiCung = tatCaCacNgay
                .GroupJoin(
                    duLieuThoTuCSDL, // Dữ liệu thô từ CSDL
                    tatCaCacNgayItem => tatCaCacNgayItem, // Key: Ngày trong danh sách đầy đủ
                    duLieuThoItem => duLieuThoItem.NgayMuon, // Key: Ngày trong dữ liệu thô
                    (ngay, duLieu) => new ThongKe_DTO // Tạo đối tượng DTO cuối cùng
                    {
                        NgayMuon = ngay,
                        // Lấy TongSoLuotMuon nếu có, nếu không có (DefaultIfEmpty) thì gán 0
                        TongSoLuotMuon = duLieu.DefaultIfEmpty(new ThongKe_DTO { TongSoLuotMuon = 0 }).First().TongSoLuotMuon
                    }
                )
                // Đảm bảo dữ liệu được sắp xếp theo ngày
                .OrderBy(d => d.NgayMuon)
                .ToList();

            return ketQuaCuoiCung;
        }
    }
}
