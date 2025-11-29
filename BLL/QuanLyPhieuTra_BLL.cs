using QuanLyThuVienNhom3.DAL;
using QuanLyThuVienNhom3.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVienNhom3.BLL
{
    internal class QuanLyPhieuTra_BLL
    {
        private QuanLyPhieuTra_DAL _DAL = new QuanLyPhieuTra_DAL();
        public List<QuanLyPhieuTra_DTO> GetListPhieuTra()
        {
            return _DAL.GetListPhieuTra().ToList();
        }
        public List<QuanLyPhieuTra_DTO> TimKiemPT(string tuKhoa)
        {
            return _DAL.timKiem(tuKhoa);
        }
    }
}
