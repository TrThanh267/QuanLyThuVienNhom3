using QuanLyThuVienNhom3.BLL;
using QuanLyThuVienNhom3.DTO;
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

namespace QuanLyThuVienNhom3.GUI.UC
{
    public partial class UserControl_QuanLyTaiKhoan : UserControl
    {
        private readonly QLTaiKhoan_BLL thuVien = new QLTaiKhoan_BLL();
        private readonly ThuVienNhom3Context _context = new ThuVienNhom3Context();

        public UserControl_QuanLyTaiKhoan()
        {
            InitializeComponent();
            Loaddata();
            LoadComboBoxVaiTro();

        }

        bool isValid = false;
        public void Loaddata()
        {
            DataGridView_DachSachTaiKhoan.DataSource = thuVien.GetLTaiKhoan();
            LoadComboBoxVaiTro();

        }

        private void LoadComboBoxVaiTro()
        {
            ComboBox_VaiTro.DataSource = _context.VaiTros.ToList();
            ComboBox_VaiTro.DisplayMember = "TenVaiTro";  
            ComboBox_VaiTro.ValueMember = "MaVaiTro"; 
        }
        public void ClearThongTin()
        {
            TextBox_TenTaiKhoan.Clear();
            TextBox_MatKhau.Clear();
            radioButton_Online.Checked = false;
            radioButton_Offline.Checked = false;
            ComboBox_VaiTro.SelectedIndex = -1;
        }



        private void ComBox_LocTaiKhoanTheoVaiTro_SelectedIndexChanged(object sender, EventArgs e)
        {

            int maVaiTro = (int)ComBox_LocTaiKhoanTheoVaiTro.SelectedIndex;
            var danhsach = thuVien.LocTKtheoVT(maVaiTro);
            DataGridView_DachSachTaiKhoan.DataSource = danhsach;
        }

        private void Button_Them_Click(object sender, EventArgs e)
        {
            TextBox_TenTaiKhoan.Clear();
            TextBox_MatKhau.Clear();
            TextBox_MatKhau.PlaceholderText = "";
            ComboBox_VaiTro.SelectedIndex = -1;
            radioButton_Online.Checked = true;
            radioButton_Offline.Checked = false;
            GroupBox_NhapThongTinTaiKhoan.Visible = true;
            isValid = true;
        }

        private void DataGridView_DachSachTaiKhoan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void UserControl_QuanLyTaiKhoan_Load_1(object sender, EventArgs e)
        {
            LoadComboBoxVaiTro();
            Loaddata();
        }

        private void GroupBox_NhapThongTinTaiKhoan_Click(object sender, EventArgs e)
        {

        }

        private void ComboBox_VaiTro_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Button_Luu_Click(object sender, EventArgs e)
        {

            if (isValid)
            {

                if (string.IsNullOrWhiteSpace(TextBox_TenTaiKhoan.Text))
                {
                    MessageBox.Show("Chưa nhập tên tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TextBox_TenTaiKhoan.Focus(); return;
                }
                if (string.IsNullOrWhiteSpace(TextBox_MatKhau.Text))
                {
                    MessageBox.Show("Chưa nhập mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TextBox_MatKhau.Focus(); return;
                }
                if (ComboBox_VaiTro.SelectedIndex == -1)
                {
                    MessageBox.Show("Chưa chọn vai trò!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
                }
                if (!radioButton_Online.Checked && !radioButton_Offline.Checked)
                {
                    MessageBox.Show("Chưa chọn trạng thái!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
                }


                string tenTK = TextBox_TenTaiKhoan.Text.Trim();
                if (tenTK.Length < 4 || tenTK.Length > 30 || tenTK.All(char.IsDigit))
                {
                    MessageBox.Show("Tên tài khoản phải 4-30 ký tự và không được toàn số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string mk = TextBox_MatKhau.Text.Trim();
                if (!Regex.IsMatch(mk, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"))
                {
                    MessageBox.Show("Mật khẩu phải ≥8 ký tự, có chữ, số và ký tự đặc biệt!", "Yêu cầu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                if (MessageBox.Show("Bạn có chắc chắn muốn thêm tài khoản này?", "Xác nhận thêm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                TaiKhoan tk = new TaiKhoan
                {
                    TenTaiKhoan = tenTK,
                    MatKhauHash = mk, 
                    TrangThai = radioButton_Online.Checked ? "Hoạt động" : "Không hoạt động",
                    MaVaiTro = Convert.ToInt32(ComboBox_VaiTro.SelectedValue)
                };

                if (thuVien.ThemTaiKhoan(tk))
                {
                    MessageBox.Show("Thêm tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearThongTin();
                    Loaddata();
                    GroupBox_NhapThongTinTaiKhoan.Visible = false;
                }
                else
                {
                    MessageBox.Show("Thêm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


            else 
            {
                if (DataGridView_DachSachTaiKhoan.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn tài khoản cần cập nhật!", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                int maTK = Convert.ToInt32(DataGridView_DachSachTaiKhoan.SelectedRows[0].Cells["MaTK"].Value);
                string tenMoi = TextBox_TenTaiKhoan.Text.Trim();
                if (string.IsNullOrWhiteSpace(tenMoi) || tenMoi.Length < 4 || tenMoi.Length > 30)
                {
                    MessageBox.Show("Tên tài khoản phải từ 4-30 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var tkHienTai = thuVien.LayTK(maTK);
                if (tkHienTai.TenTaiKhoan != tenMoi && thuVien.KiemTraTrungTen(tenMoi))
                {
                    MessageBox.Show("Tên tài khoản này đã được sử dụng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string mkMoi = TextBox_MatKhau.Text.Trim();
                if (!string.IsNullOrEmpty(mkMoi))
                {
                    if (!Regex.IsMatch(mkMoi, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"))
                    {
                        MessageBox.Show("Mật khẩu mới phải ≥8 ký tự, có chữ, số và ký tự đặc biệt!", "Yêu cầu",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                var taikhoan = thuVien.LayTK(maTK);
                if (thuVien.KiemTraCoNhanVien(maTK))
                {
                    MessageBox.Show("Tài khoản này đã gắn với nhân viên nên KHÔNG THỂ thay đổi mật khẩu!",
                                    "Hạn chế", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    mkMoi = "";
                    return;
                }
                if (MessageBox.Show("Bạn có chắc chắn muốn cập nhật tài khoản này?", "Xác nhận cập nhật",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                TaiKhoan tk = new TaiKhoan
                {
                    MaTaiKhoan = maTK,
                    TenTaiKhoan = tenMoi,
                    MatKhauHash = string.IsNullOrEmpty(mkMoi) ? null : mkMoi, 
                    TrangThai = radioButton_Online.Checked ? "Hoạt động" : "Không hoạt động",
                    MaVaiTro = Convert.ToInt32(ComboBox_VaiTro.SelectedValue)
                };
                if (thuVien.CapNhat(tk))
                {
                    MessageBox.Show("Cập nhật tài khoản thành công!", "Thành công",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearThongTin();
                    Loaddata();
                    GroupBox_NhapThongTinTaiKhoan.Visible = false;
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }




        private void Button_Huy_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinTaiKhoan.Visible = false;
            ClearThongTin();

        }

        private void Button_Xoa_Click(object sender, EventArgs e)
        {
            if (DataGridView_DachSachTaiKhoan.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một tài khoản để xóa!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Lấy dòng được chọn
            int maTK = Convert.ToInt32(DataGridView_DachSachTaiKhoan
                            .SelectedRows[0].Cells["MaTK"].Value);

            bool ketQua = thuVien.XoaTaiKhoan(maTK);

            // Luôn reload dữ liệu dù xóa thành công hay thất bại
            // Vì DAL đã tự reset context nên không sợ lỗi cũ
            Loaddata();

            // Chỉ hiện thông báo thành công (thất bại thì DAL/BLL đã tự hiện rồi)
            if (ketQua)
            {
                MessageBox.Show("Xóa tài khoản thành công!", "Thành công",
                                         MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void Button_TimKiem_Click(object sender, EventArgs e)
        {
            string keyword = TextBox_TimKiem.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            List<QLTaiKhoan_DTO> ketQua = thuVien.TimkiemTK(keyword);
            if (ketQua.Any())
            {
                DataGridView_DachSachTaiKhoan.DataSource = ketQua;
            }
            else
            {
                MessageBox.Show("Không tìm thấy taì khoản với từ khóa đã nhập!", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void TextBox_TimKiem_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox_TimKiem_DoubleClick(object sender, EventArgs e)
        {
            TextBox_TimKiem.Clear();
            Loaddata();
        }

        private void Button_CapNhap_Click(object sender, EventArgs e)
        {
            if (DataGridView_DachSachTaiKhoan.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần sửa!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            isValid = false;                                  // QUAN TRỌNG NHẤT
            GroupBox_NhapThongTinTaiKhoan.Visible = true;

            // Đưa dữ liệu từ dòng được chọn lên form
            DataGridViewRow row = DataGridView_DachSachTaiKhoan.SelectedRows[0];

            TextBox_TenTaiKhoan.Text = row.Cells["TenTK"].Value.ToString();
            TextBox_MatKhau.Text = "";
            TextBox_MatKhau.PlaceholderText = "";

            ComboBox_VaiTro.SelectedValue = row.Cells["VaiTro"].Value;

            string trangthai = row.Cells["TrangThai"].Value.ToString();
            if (trangthai == "Hoạt động" || trangthai == "Online")
                radioButton_Online.Checked = true;
            else
                radioButton_Offline.Checked = true;
        }

        private void DataGridView_DachSachTaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DataGridView_DachSachTaiKhoan.Rows[e.RowIndex];
                TextBox_TenTaiKhoan.Text = row.Cells["TenTK"].Value.ToString();
                TextBox_MatKhau.Text = row.Cells["MaTKhau"].Value.ToString();
                string trangthai = row.Cells["TrangThai"].Value.ToString();
                if (trangthai == "Hoạt động")
                {
                    radioButton_Online.Checked = true;
                    radioButton_Offline.Checked = false;
                }
                else if (trangthai == "Không hoạt động")
                {
                    radioButton_Online.Checked = false;
                    radioButton_Offline.Checked = true;
                }
                string vaitro = row.Cells["VaiTro"].Value.ToString();
                ComboBox_VaiTro.Text = vaitro;
            }
        }
    }
}
