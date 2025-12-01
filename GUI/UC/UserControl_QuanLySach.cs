using Nhom3ThuVienBanNhap.DTO;
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
            CheckPhanQuyen();
        }

        public void CheckPhanQuyen()
        {
            if (UserSession.TaiKhoanHienTai.MaVaiTro == 2)
            {
                Button_Them.Enabled = false;
                Button_Xoa.Enabled = false;
                TextBox_TenSach.Enabled = false;
                guna2TextBox1.Enabled = false;
                guna2TextBox2.Enabled = false;
                ComboBox_LoaiSach.Enabled = false;
                ComboBox_NhaXuatBan.Enabled = false;
            }
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
