using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyThuVienNhom3.GUI.UC
{
    public partial class UserControl_QuanLyDocGia : UserControl
    {
        public UserControl_QuanLyDocGia()
        {
            InitializeComponent();
        }

        private void Button_Them_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinDocGia.Visible = true;
        }

        private void Button_CapNhap_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinDocGia.Visible = true;
        }

        private void Button_Huy_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinDocGia.Visible = false;
        }

        private void Button_QuetQRThongTinDocGia_Click(object sender, EventArgs e)
        {
            GroupBox_ThongTinChiTietDocGia.Visible = true;
        }

        private void Button_HuyThongTin_Click(object sender, EventArgs e)
        {
            GroupBox_ThongTinChiTietDocGia.Visible = false;
        }
    }
}
