using QuanLyThuVienNhom3.DAL;
using QuanLyThuVienNhom3.DTO;
using QuanLyThuVienNhom3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVienNhom3.BLL
{
    internal class QuanLyChiTietPhieuMuon_BLL
    {
        public string LastError { get; private set; } = string.Empty;
        private QuanLyPhieuMuonChiTiet_DAL _DAL = new QuanLyPhieuMuonChiTiet_DAL();
        public List<QuanLyPhieuMuonChiTiet_DTO> GetListPMCT()
        {
            return _DAL.GetListPMCT().ToList();
        } 
        public bool themCTPM(ChiTietPhieuMuon chiTietPhieuMuon)
        {
            if (!_DAL.KiemTraSoLuongMuon(chiTietPhieuMuon))
            {
                MessageBox.Show("Số lượng sách mượn vượt quá giới hạn cho phép!",
                       "Cảnh báo",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Warning);
                return false;
            }
            if (!_DAL.KiemTraSoLuongSachConLaiTrongKho(chiTietPhieuMuon))
            {
                MessageBox.Show("Số lượng sách mượn vượt quá số lượng sách trong kho!",
                       "Cảnh báo",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Warning);
                return false;
            }
            return _DAL.ThemPMCT(chiTietPhieuMuon);
        }
        public List<Sach> GetSaches()
        {
            return _DAL.GetSaches().ToList();
        }
        public List<PhieuMuon> GetPhieuMuons()
        {
            return _DAL.GetPhieuMuons().ToList();
        }
        public bool TraSach(int maChiTietPhieuMuon, string tinhTrangSach)
        {
            LastError = string.Empty;
            using var transaction = _DAL.BeginTransaction();
            try
            {
                var chiTiet = _DAL.GetCTPMById(maChiTietPhieuMuon);
                if (chiTiet == null || chiTiet.MaPhieuMuon == null)
                    throw new Exception("Không tìm thấy chi tiết hoặc phiếu mượn cha.");

                int maPM = chiTiet.MaPhieuMuon.GetValueOrDefault();
                int maSach = chiTiet.MaSach.GetValueOrDefault();
                int soLuongDaTra = chiTiet.SoLuongMuon.GetValueOrDefault();
                bool daXuLyTruocDo = chiTiet.DaGhiNhanTra.GetValueOrDefault();
                if (!daXuLyTruocDo)
                { 
                    _DAL.GiamSoLuongNoPM(maPM, soLuongDaTra);
                    _DAL.CongTonKho(maSach, soLuongDaTra);
                    chiTiet.DaGhiNhanTra = true;
                }
                chiTiet.TinhTrangSach = tinhTrangSach;
                _DAL.SuaChiTietPhieuMuon(chiTiet);
                int soLuongThucTe = _DAL.LaySoLuongNoThucTe(maPM);

                bool daTraDuTatCa = false;
                if (soLuongThucTe <= 0)
                {
                    _DAL.CapNhatTrangThaiPM(maPM, "Đã trả");
                    daTraDuTatCa = true;
                }

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
        public bool XuLyKhoTonKhiCapNhatCTPM(int maSach, int deltaSoLuong, int maPhieuMuon, out string thongBaoThayDoi)
        {
            thongBaoThayDoi = string.Empty; 


            try
            {
                if (deltaSoLuong != 0)
                {
                    string hanhDongKho = (deltaSoLuong > 0) ? "ĐÃ TRỪ" : "ĐÃ CỘNG LẠI";
                    int soLuongTuyetDoi = Math.Abs(deltaSoLuong);
                    thongBaoThayDoi = $"- Tồn kho Mã Sách {maSach} đã bị {hanhDongKho} {soLuongTuyetDoi} cuốn.";
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool capNhapCTPM(ChiTietPhieuMuon ctpmNEW)
        {
            string thongBaoKho = string.Empty;

            try
            {
                var ctpmOLD = _DAL.GetCTPMById(ctpmNEW.MaChiTietPhieuMuon);

                if (ctpmOLD == null)
                {
                    MessageBox.Show("Không tìm thấy Chi tiết Phiếu Mượn cần cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                var pmCha = _DAL.GetPMById(ctpmOLD.MaPhieuMuon.GetValueOrDefault());

                if (ctpmOLD.DaGhiNhanTra == true || (pmCha != null && pmCha.TrangThai == "Đã trả"))
                {
                    MessageBox.Show("Không thể cập nhật chi tiết đã được ghi nhận trả hoặc phiếu đã hoàn tất!", "Lỗi Nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                int soLuongMoi = ctpmNEW.SoLuongMuon.GetValueOrDefault(0);
                int soLuongCu = ctpmOLD.SoLuongMuon.GetValueOrDefault(0);

                if (soLuongMoi <= 0)
                {
                    MessageBox.Show("Số lượng mượn mới phải lớn hơn 0.", "Lỗi Dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                int deltaSoLuong = soLuongMoi - soLuongCu;

                if (deltaSoLuong != 0)
                {
                    if (!XuLyKhoTonKhiCapNhatCTPM(ctpmOLD.MaSach.GetValueOrDefault(),
                                                 deltaSoLuong,
                                                 ctpmOLD.MaPhieuMuon.GetValueOrDefault(),
                                                 out thongBaoKho))
                    {
                        MessageBox.Show(LastError, "Lỗi Cập nhật Kho", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                _DAL.SuaChiTietPhieuMuon(ctpmNEW);
                string thongBaoChung = "Cập nhật Chi tiết Phiếu Mượn thành công!";
                if (!string.IsNullOrEmpty(thongBaoKho))
                {
                    thongBaoChung += $"\n\nKết quả xử lý tồn kho:\n{thongBaoKho}";
                }

                MessageBox.Show(thongBaoChung, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi trong quá trình cập nhật Chi tiết Phiếu Mượn: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public bool XoaCTPM(int maChiTietPhieuMuon)
        {
            LastError = string.Empty;
            using var transaction = _DAL.BeginTransaction();
            try
            {
                var chiTiet = _DAL.GetCTPMById(maChiTietPhieuMuon);
                if (chiTiet == null)
                {
                    LastError = "Không tìm thấy Chi tiết Phiếu Mượn để xóa.";
                    return false;
                }

                int maPM = chiTiet.MaPhieuMuon.GetValueOrDefault();
                int maSach = chiTiet.MaSach.GetValueOrDefault();
                int soLuongDaMuon = chiTiet.SoLuongMuon.GetValueOrDefault();
                if (chiTiet.DaGhiNhanTra.GetValueOrDefault())
                {
                    LastError = "Không thể xóa chi tiết sách đã được ghi nhận trả.";
                    return false;
                }
                if (!_DAL.CapNhatTonKhoAtomic(maSach, soLuongDaMuon))
                {
                    LastError = "Lỗi khi hoàn trả tồn kho sách.";
                    transaction.Rollback();
                    return false;
                }
                if (!_DAL.GiamSoLuongNoPM(maPM, soLuongDaMuon)) 
                {
                    LastError = "Lỗi khi cập nhật số lượng phiếu mượn cha.";
                    transaction.Rollback();
                    return false;
                }

                _DAL.XoaChiTietPhieuMuon(maChiTietPhieuMuon);
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                LastError = "Lỗi hệ thống khi xóa CTPM: " + ex.Message;
                return false;
            }
        }
        public List<QuanLyPhieuMuonChiTiet_DTO> TimKiemPMCT(string tuKhoa)
        {
            return _DAL.TimKiemPMCT(tuKhoa).ToList();
        }
    }
}
