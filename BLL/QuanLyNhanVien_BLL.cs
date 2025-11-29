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
    internal class QuanLyNhanVien_BLL
    {
        private QuanLyNhanVien_DAL _quanLyNhanVien_DAL = new QuanLyNhanVien_DAL();
        public List<QuanLyNhanVien_DTO> GetNhanViens()
        {
            return _quanLyNhanVien_DAL.GetListNhanVien();
        }
        public bool themNhanVien(NhanVien nhanVien)
        {
            if (_quanLyNhanVien_DAL.KiemTraSoDienToaiTonTai(nhanVien.SoDienThoai))
            {
                MessageBox.Show("Số điện thoại này đã được đăng kí!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (_quanLyNhanVien_DAL.KiemTraEmailTonTai(nhanVien.Email))
            {
                MessageBox.Show("Email này đã được đăng kí!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (_quanLyNhanVien_DAL.KiemTraTaiKhoanDangKy(nhanVien.MaTaiKhoan))
            {
                MessageBox.Show("Tài khoản này đã được đăng kí cho nhân viên khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
            {
                _quanLyNhanVien_DAL.ThemNhanVien(nhanVien);
                return true;
            }
        }
        public bool XoaNhanVien(int maNhanVien)
        {
            if (_quanLyNhanVien_DAL.KiemTraNhanVienVoiPhieuMuon(maNhanVien))
            {
                MessageBox.Show("Nhân viên này đang có phiếu mượn, không thể xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (_quanLyNhanVien_DAL.KiemTraNhanVienVoiChamCong(maNhanVien))
            {
                MessageBox.Show("Nhân viên này đang có chấm công, không thể xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
            {
                return _quanLyNhanVien_DAL.XoaNhanVien(maNhanVien);
            }
        }
        public List<QuanLyNhanVien_DTO> LocNhanVienTheoTrangThai()
        {
            return _quanLyNhanVien_DAL.nhanVienDangHoatDong();
        }
        public List<QuanLyNhanVien_DTO> TimKiemNhanVien(string keyword)
        {
            return _quanLyNhanVien_DAL.TimKiemNhanVien(keyword);
        }
        public bool CapNhapNhanVien(NhanVien nhanVien)
        {
            return _quanLyNhanVien_DAL.CapNhapNhanVien(nhanVien);
        }
        public List<TaiKhoan> TaiKhoanNhanVien()
        {
            return _quanLyNhanVien_DAL.TaiKhoanNhanVien();
        }
        public List<QuanLyNhanVien_DTO> LocNhanVienTheoTrangThai(string TrangThai)
        {
            if (TrangThai == "Đang làm việc")
            {
                return _quanLyNhanVien_DAL.nhanVienDangHoatDong();
            }
            else if (TrangThai == "Đã nghỉ")
            {
                return _quanLyNhanVien_DAL.nhanVienNgungHoatDong();
            }
            else
            {
                return _quanLyNhanVien_DAL.GetListNhanVien();
            }
        }
    }
}
