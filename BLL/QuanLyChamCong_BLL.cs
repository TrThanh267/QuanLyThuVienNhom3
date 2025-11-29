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
    internal class QuanLyChamCong_BLL
    {
        private QuanLyChamCong_DAL chamCong_DAL = new QuanLyChamCong_DAL();
        public List<DTO.QuanLyChamCong_DTO> GetListChamCong()
        {
            return chamCong_DAL.GetListChamCong();
        }
        public bool ThemChamCong(ChamCong chamCong)
        {
            if (chamCong_DAL.KiemTraChamCongTonTai(chamCong.MaNhanVien.Value))
            {
                MessageBox.Show("Nhân viên này đã được chấm công trong ngày hôm nay!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (!chamCong_DAL.KiemTraChamCongVoiNhanVien(chamCong.MaNhanVien.Value))
            {
                MessageBox.Show("Chỉ có nhân viên mới có thể chấm công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
            {
                return chamCong_DAL.ThemChamCong(chamCong);
            }
        }
        public bool CapNhapChamCong(ChamCong chamCong)
        {
            return chamCong_DAL.CapNhapChamCong(chamCong);
        }
        public NhanVien layThongTinNhanVienQuaTaiKhoan(int maTaiKhoan)
        {
            return chamCong_DAL.LayThongTinNhanVienQuaTaiKhoan(maTaiKhoan);
        }
        public List<NhanVien> GetNhanViens()
        {
            return chamCong_DAL.GetListNhanVien();
        }
        public List<CaLam> GetCaLams()
        {
            return chamCong_DAL.GetListCaLam();
        }
        public List<LoaiCong> GetLoaiCong()
        {
            return chamCong_DAL.GetListLoaiCong();
        }
        public List<QuanLyChamCong_DTO> LocChamCong(int ngay, int thang, int nam)
        {
            return chamCong_DAL.LocChamCong(ngay, thang, nam);
        }
        public int KiemTraLamChinh(int thang, int nam, int idnv)
        {
            return chamCong_DAL.KiemTraNgayCongLamChinh(thang, nam, idnv);
        }
        public int KiemTraLamThem(int thang, int nam, int idnv)
        {
            return chamCong_DAL.KiemTraNgayCongLamNgoaiGio(thang, nam, idnv);
        }
        public int KiemTraLamNgayLe(int thang, int nam, int idnv)
        {
            return chamCong_DAL.KiemTraNgayCongLamNgaayLe(thang, nam, idnv);
        }
        public List<QuanLyChamCong_DTO> TimKiem(string tukhoa)
        {
            return chamCong_DAL.TimKiem(tukhoa);
        }
        public void XoaCC(int chamCong)
        {
            chamCong_DAL.XoaChamCong(chamCong);
        }
    }
}
