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
    internal class QuanLyPhieuMuon_BLL
    {
        private QuanLyPhieuMuon_DAL _DAL = new QuanLyPhieuMuon_DAL();
        public List<QuanLyPhieuMuon_DTO> GetListPM()
        {
            return _DAL.GetListPhieuMuon().ToList();
        }
        public bool ThemPM(PhieuMuon phieuMuon)
        {
            return _DAL.ThemPhieuMuon(phieuMuon);
        }
        public List<DocGium> GetDocGia()
        {
            return _DAL.GetDocGia().ToList();
        }
        public List<NhanVien> GetNhanVien()
        {
            return _DAL.GetNhanViens().ToList();
        }
        public bool XoaPhieuMuon(int phieuMuon)
        {
            try
            {
                var pm = _DAL.GetPhieuMuonByID(phieuMuon);
                if (pm == null)
                {
                    MessageBox.Show("Phiếu mượn không tồn tại!",
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return false;
                }
                if (_DAL.KiemTraPMNeuDaTra(phieuMuon))
                {
                    MessageBox.Show("Không thể xóa Phiếu Mượn vì trạng thái là 'Đã trả'!",
                                    "Lỗi Nghiệp Vụ",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return false;
                }
                if (_DAL.KiemTraPhieuMuonDangCoDuLieuBenCTPM(phieuMuon))
                {
                    MessageBox.Show("Không thể xóa phiếu mượn vì còn dữ liệu bên Chi tiết phiếu mượn!",
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return false ;
                }
                _DAL.XoaPhieuMuon(phieuMuon);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa phiếu mượn: " + ex.Message,
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }

        }
        public bool CapNhapPhieuMuon(PhieuMuon phieuMuon)
        {
            try
            {
                // 1. LẤY DỮ LIỆU CŨ ĐỂ SO SÁNH
                var pmHienTai = _DAL.GetPhieuMuonByID(phieuMuon.MaPhieuMuon);
                if (pmHienTai == null)
                {
                    MessageBox.Show("Phiếu mượn không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // 2. RÀNG BUỘC TUYỆT ĐỐI: KHÔNG ĐƯỢC CHỌN "ĐÃ TRẢ" TẠI ĐÂY
                // Trạng thái "Đã trả" chỉ được hệ thống tự động cập nhật khi xử lý xong bên CTPM.
                if (phieuMuon.TrangThai == "Đã trả")
                {
                    MessageBox.Show("Không thể thủ công chuyển trạng thái sang 'Đã trả'!\nTrạng thái này chỉ được cập nhật tự động khi tất cả sách trong Chi tiết phiếu mượn đã được trả.",
                                    "Lỗi Nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                bool coChiTiet = _DAL.KiemTraPhieuMuonDangCoDuLieuBenCTPM(phieuMuon.MaPhieuMuon);

                if (coChiTiet)
                {
                    // --- TRƯỜNG HỢP 1: ĐÃ CÓ CHI TIẾT ---
                    // Chặn tuyệt đối việc sửa Số lượng và Mã Độc giả

                    if (pmHienTai.SoLuong != phieuMuon.SoLuong || pmHienTai.MaDocGia != phieuMuon.MaDocGia)
                    {
                        MessageBox.Show("Không thể thay đổi 'Số lượng' hoặc 'Độc giả' vì Phiếu mượn này đã có dữ liệu sách (Chi tiết)!",
                                        "Cập nhật bị chặn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    // Nếu người dùng cố tình sửa SoLuongBanDau (dù UI có thể đã chặn), ta reset lại bằng giá trị cũ cho an toàn
                    phieuMuon.SoLuongBanDau = pmHienTai.SoLuongBanDau;
                    phieuMuon.SoLuong = pmHienTai.SoLuong; // Giữ nguyên số lượng nợ
                }
                else
                {
                    // --- TRƯỜNG HỢP 2: CHƯA CÓ CHI TIẾT (Được phép sửa thoải mái) ---

                    // TỰ ĐỘNG ĐỒNG BỘ: Số lượng ban đầu = Số lượng (nhập mới)
                    // Vì chưa có sách nào được mượn, nên Nợ bao nhiêu thì Gốc bấy nhiêu.
                    phieuMuon.SoLuongBanDau = phieuMuon.SoLuong;
                }

                // 4. GỌI DAL ĐỂ LƯU
                // Lúc này phieuMuon đã được chuẩn hóa dữ liệu (SoLuongBanDau đã khớp)
                _DAL.SuaPhieuMuon(phieuMuon);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật phiếu mượn: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public List<QuanLyPhieuMuon_DTO> TimKiemPM(string tukhoa)
        {
            return _DAL.TimKiemPM(tukhoa).ToList();
        }

    }
}
