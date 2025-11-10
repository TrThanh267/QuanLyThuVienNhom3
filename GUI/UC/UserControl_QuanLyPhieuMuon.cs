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
    public partial class UserControl_QuanLyPhieuMuon : UserControl
    {
        public UserControl_QuanLyPhieuMuon()
        {
            InitializeComponent();
        }

        private void Button_Them_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinPhieuMuon.Visible = true;
        }

        private void Button_CapNhap_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinPhieuMuon.Visible = true;
        }

        private void Button_Huy_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinPhieuMuon.Visible = false;
        }

        private void Button_ThemCTPM_Click(object sender, EventArgs e)
        {
            GroupBox_ChiTietPhieuMuon.Visible = true;
        }

        private void Button_CapNhapCTPM_Click(object sender, EventArgs e)
        {
            GroupBox_ChiTietPhieuMuon.Visible = true;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            GroupBox_ChiTietPhieuMuon.Visible = false;
        }
    }
}
