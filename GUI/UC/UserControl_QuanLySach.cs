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
    public partial class UserControl_QuanLySach : UserControl
    {
        public UserControl_QuanLySach()
        {
            InitializeComponent();
        }

        private void Button_ThemSach_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinSach.Visible = true;
        }

        private void Button_CapNhap_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinSach.Visible = true;
        }

        private void Button_Huy_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinSach.Visible = false;
        }
    }
}
