using Nhom3ThuVienBanNhap.DTO;
using QuanLyThuVienNhom3.GUI.UC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyThuVienNhom3.GUI
{
    public partial class FormTrangChu : Form
    {
        public FormTrangChu()
        {
            InitializeComponent();
            label_TenNguoiDung.Text =  UserSession.TaiKhoanHienTai.TenTaiKhoan;
            label_TenNguoiDung.Visible = true;
        }
        private void Button_QuanLySach_Click(object sender, EventArgs e)
        {

            userControl_QuanLySach1.Visible = true;
            userControl_QuanLySach1.BringToFront();
            userControl_QuanLyNhanVien1.Visible = false;
            userControl_QuanLyDocGia1.Visible = false;
            userControl_QuanLyChamCong1.Visible = false;
            userControl_QuanLyTaiKhoan1.Visible = false;
            userControl_ThongKe1.Visible= false;
        }

        private void Button_QuanLyNhanVien_Click(object sender, EventArgs e)
        {
            if (UserSession.TaiKhoanHienTai.MaVaiTro == 1) 
            {
                userControl_QuanLySach1.Visible = false;
                userControl_QuanLyNhanVien1.BringToFront();
                userControl_QuanLyNhanVien1.LoadData();
                userControl_QuanLyNhanVien1.Visible = true;
                userControl_QuanLyDocGia1.Visible = false;
                userControl_QuanLyChamCong1.Visible = false;
                userControl_QuanLyTaiKhoan1.Visible = false;
                userControl_ThongKe1.Visible = false;
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void Button_QuanLyDocGia_Click(object sender, EventArgs e)
        {
            userControl_QuanLyDocGia1.Visible = true;
            userControl_QuanLyDocGia1.BringToFront();
            userControl_QuanLyDocGia1.LoadDaTa();
            userControl_QuanLySach1.Visible = false;
            userControl_QuanLyNhanVien1.Visible = false;
            userControl_QuanLyChamCong1.Visible = false;
            userControl_QuanLyTaiKhoan1.Visible = false;
            userControl_ThongKe1.Visible = false;
        }

        private void Button_QuanLyPhieuMuon_Click(object sender, EventArgs e)
        {
            userControl_QuanLyPhieuMuon1.Visible = true;
            userControl_QuanLyDocGia1.Visible = false;
            userControl_QuanLyPhieuMuon1.BringToFront();
            userControl_QuanLyPhieuMuon1.LoadData();
            userControl_QuanLySach1.Visible = false;
            userControl_QuanLyNhanVien1.Visible = false;
            userControl_QuanLyChamCong1.Visible = false;
            userControl_QuanLyTaiKhoan1.Visible = false;
            userControl_ThongKe1.Visible = false;
        }

        private void Button_QuanLyChamCong_Click(object sender, EventArgs e)
        {
            userControl_QuanLyPhieuMuon1.Visible = false;
            userControl_QuanLyDocGia1.Visible = false;
            userControl_QuanLyChamCong1.BringToFront();
            userControl_QuanLyChamCong1.LoadData();
            userControl_QuanLySach1.Visible = false;
            userControl_QuanLyNhanVien1.Visible = false;
            userControl_QuanLyChamCong1.Visible = true;
            userControl_QuanLyTaiKhoan1.Visible = false;
        }

        private void Button_QuanLyTaiKhoan_Click(object sender, EventArgs e)
        {
            if (UserSession.TaiKhoanHienTai.MaVaiTro == 1)
            {
                userControl_QuanLySach1.Visible = false;
                userControl_QuanLyTaiKhoan1.BringToFront();
                userControl_QuanLyTaiKhoan1.Loaddata();
                userControl_QuanLyNhanVien1.Visible = false;
                userControl_QuanLyDocGia1.Visible = false;
                userControl_QuanLyChamCong1.Visible = false;
                userControl_QuanLyTaiKhoan1.Visible = true;
                userControl_ThongKe1.Visible = false;
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void Button_ThongKe_Click(object sender, EventArgs e)
        {
            userControl_QuanLySach1.Visible = false;
            userControl_ThongKe1.LoadData();
            userControl_ThongKe1.BringToFront();
            userControl_QuanLyNhanVien1.Visible = false;
            userControl_QuanLyDocGia1.Visible = false;
            userControl_QuanLyChamCong1.Visible = false;
            userControl_QuanLyTaiKhoan1.Visible = false;
            userControl_ThongKe1.Visible = true;
        }
    }
}
