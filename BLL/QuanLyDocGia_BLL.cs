using DocumentFormat.OpenXml.Wordprocessing;
using QuanLyThuVienNhom3.DAL;
using QuanLyThuVienNhom3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVienNhom3.BLL
{
    internal class QuanLyDocGia_BLL
    {
        private QuanLyDocGia_DAL _quanLyDocGia_DAL = new QuanLyDocGia_DAL();
        public void KiemTraValidate(string SDT, string email)
        {
            if (_quanLyDocGia_DAL.KiemTraSoDienThoaiTonTai(SDT))
            {
                MessageBox.Show("Số điện thoại này đã được đăng kí!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (_quanLyDocGia_DAL.KiemTraEmailTonTai(email))
            {
                MessageBox.Show("Email này đã được đăng kí!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                return;
            }
        }
        public List<DTO.QuanLyDocGia_DTO> GetListDocGia()
        {
            return _quanLyDocGia_DAL.GetListDocGia();
        }
        public int ThemMaDoGia()
        {
            return _quanLyDocGia_DAL.ThemMaDoGia();
        }
        public bool ThemDocGia(DocGium docGia)
        {
            if (_quanLyDocGia_DAL.KiemTraSoDienThoaiTonTai(docGia.SoDienThoai))
            {
                MessageBox.Show("Số điện thoại này đã được đăng kí!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (_quanLyDocGia_DAL.KiemTraEmailTonTai(docGia.Email))
            {
                MessageBox.Show("Email này đã được đăng kí!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
            {
                _quanLyDocGia_DAL.ThemDocGia(docGia);
                return true;
            }
        }
        public void SuaDocGia(DocGium docGia)
        {
            if (_quanLyDocGia_DAL.KiemTraSoDienThoaiTonTaiKhiSua(docGia.SoDienThoai, docGia.MaDocGia))
            {
                MessageBox.Show("Số điện thoại này đã được đăng kí!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (_quanLyDocGia_DAL.KiemTraEmailTonTaiKhiSua(docGia.Email, docGia.MaDocGia))
            {
                MessageBox.Show("Email này đã được đăng kí!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                _quanLyDocGia_DAL.SuaDocGia(docGia);
            }
        }
        public int XoaDocGia(int maDocGia)
        {
            if (_quanLyDocGia_DAL.KiemTraDocGiaCoPhieuMuon(maDocGia))
                return -1;

            bool ketQua = _quanLyDocGia_DAL.XoaDocGia(maDocGia);
            return ketQua ? 1 : 0;
        }

        public byte[] GetHinhAnhDocGia(string maDG)
        {
            return _quanLyDocGia_DAL.GetHinhAnhByMaDocGia(maDG);
        }
        public string GetTrangThaiDocGia(string id)
        {
            return _quanLyDocGia_DAL.GetTrangThaiDocGia(id);
        }
        public DocGium GetDocGiaByMa(string maDocGia)
        {
            if (!int.TryParse(maDocGia, out int ma))
                return null;

            return _quanLyDocGia_DAL.GetDocGiaByMa(ma);
        }
    }
}