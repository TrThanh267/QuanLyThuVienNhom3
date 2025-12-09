using Microsoft.Data.SqlClient;
using Nhom3ThuVienBanNhap.DTO;
using QuanLyThuVienNhom3.GUI.UC;
using QuanLyThuVienNhom3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyThuVienNhom3.GUI
{
    public partial class FormTrangChu : Form
    {
        private readonly ThuVienNhom3Context _context = new ThuVienNhom3Context();
        public FormTrangChu()
        {
            InitializeComponent();
            label_TenNguoiDung.Text = UserSession.TaiKhoanHienTai.TenTaiKhoan;
            label_TenNguoiDung.Visible = true;
            DocTaiKhoan();
            LoadForm();
        }
        public void DocTaiKhoan()
        {
            var taikhoan = _context.NhanViens
                .FirstOrDefault(tk => tk.MaTaiKhoan == UserSession.TaiKhoanHienTai.MaTaiKhoan); // Lấy thông tin nhân viên từ bảng NhanVien dựa trên MaTaiKhoan
            Label_IDNhanVien.Text = taikhoan?.MaNhanVien.ToString(); // Hiển thị mã nhân viên vào Label_IDNhanVien
            Label_TenNhanVien.Text = taikhoan?.TenNhanVien;
            Label_NgaySinhNV.Text = taikhoan?.NgaySinh?.ToString("dd/MM/yyyy");
            Label_GioiTinh.Text = taikhoan?.GioiTinh;
            Label_SoDienThoai.Text = taikhoan?.SoDienThoai;
            Label_Email.Text = taikhoan?.Email;
            Label_DiaChi.Text = taikhoan?.DiaChi;
            Label_TrangThai.Text = taikhoan?.TrangThai;
            Label_TaiKhoan.Text = UserSession.TaiKhoanHienTai.TenTaiKhoan;

        }
        public void LoadForm()
        {
            userControl_QuanLySach1.LoadDataToDataGridView();
            userControl_QuanLyNhanVien1.LoadData();
            userControl_QuanLyDocGia1.LoadDaTa();
            userControl_QuanLyPhieuMuon1.LoadData();
            userControl_QuanLyChamCong1.LoadData();
            userControl_QuanLyTaiKhoan1.Loaddata();
            userControl_QuanLyTaiKhoan1.Loaddata();
        }
        private void Button_QuanLySach_Click(object sender, EventArgs e)
        {

            userControl_QuanLySach1.Visible = true;
            userControl_QuanLySach1.BringToFront();
            userControl_QuanLySach1.LoadDataToDataGridView();
            userControl_QuanLyNhanVien1.Visible = false;
            userControl_QuanLyDocGia1.Visible = false;
            userControl_QuanLyChamCong1.Visible = false;
            userControl_QuanLyTaiKhoan1.Visible = false;
            userControl_ThongKe1.Visible = false;
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

        private void btn_Luu_Click(object sender, EventArgs e)
        {
            string taiKhoan = txt_TK.Text.Trim();
            string matKhauCu = txt_MK.Text;
            string matKhauMoi = txt_MKMoi.Text;
            string nhapLai = txt_NhapLai.Text;  // nếu bạn có ô nhập lại

            // 2. Validate cơ bản
            if (string.IsNullOrEmpty(taiKhoan) || string.IsNullOrEmpty(matKhauCu) || string.IsNullOrEmpty(matKhauMoi))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (matKhauMoi != nhapLai)
            {
                MessageBox.Show("Mật khẩu mới và nhập lại không khớp!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Regex.IsMatch(matKhauMoi, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"))
            {
                MessageBox.Show("\"Mật khẩu phải ≥8 ký tự, có chữ, số và ký tự đặc biệt!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (matKhauMoi == matKhauCu)
            {
                MessageBox.Show("Mật khẩu mới không được trùng với mật khẩu cũ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                string query = "SELECT MatKhauHash FROM TaiKhoan WHERE TenTaiKhoan = @tk";
                string connect = @"Server=TROUBLE\SQLEXPRESS03;Database=ThuVienNhom3;Trusted_Connection=True;TrustServerCertificate=True";

                using (SqlConnection conn = new SqlConnection(connect))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@tk", taiKhoan);
                        object result = cmd.ExecuteScalar();

                        if (result == null)
                        {
                            MessageBox.Show("Tài khoản không tồn tại!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string matKhauTrongDB = result.ToString();
                        bool matKhauCuDung = BCrypt.Net.BCrypt.Verify(matKhauCu, matKhauTrongDB);
                        if (!matKhauCuDung)
                        {
                            MessageBox.Show("Mật khẩu cũ không đúng!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    string matKhauMoiHash = BCrypt.Net.BCrypt.HashPassword(matKhauMoi);

                    string updateQuery = "UPDATE TaiKhoan SET MatKhauHash = @mk WHERE TenTaiKhoan = @tk";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@mk", matKhauMoiHash);
                        cmd.Parameters.AddWithValue("@tk", taiKhoan);
                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Đổi mật khẩu thành công!\nVui lòng đăng nhập lại.", "Thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Visible = false; // hoặc mở lại form đăng nhập
                        }
                        else
                        {
                            MessageBox.Show("Đổi mật khẩu thất bại!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label_TenNguoiDung_Click(object sender, EventArgs e)
        {
            GroupBox_ThongTinCaNhan.Visible = true;
        }

        private void btn_Huy_Click(object sender, EventArgs e)
        {
            GroupBox_DoiMatKhau.Visible = false;
        }

        private void Button_Huy_Click(object sender, EventArgs e)
        {
            GroupBox_ThongTinCaNhan.Visible = false;
        }

        private void Button_DoiMaKhau_Click(object sender, EventArgs e)
        {
            GroupBox_DoiMatKhau.Visible = true;
        }
    }
}
