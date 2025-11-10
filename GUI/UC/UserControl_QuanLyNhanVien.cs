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
    public partial class UserControl_QuanLyNhanVien : UserControl
    {
        public UserControl_QuanLyNhanVien()
        {
            InitializeComponent();
        }

        private void Button_ThemNhanVien_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinNhanVien.Visible = true;
        }

        private void Button_Huy_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinNhanVien.Visible = false;
        }

        private void Button_CapNhap_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinNhanVien.Visible = true;
        }
    }
}
