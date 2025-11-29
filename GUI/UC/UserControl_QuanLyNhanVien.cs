using ClosedXML.Excel;
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
    public partial class UserControl_QuanLyNhanVien : UserControl
    {
        QuanLyNhanVien_BLL _QuanLyNhanVien_BLL;
        public UserControl_QuanLyNhanVien()
        {

            InitializeComponent();
            _QuanLyNhanVien_BLL = new QuanLyNhanVien_BLL();
            LoadData();
            loadComboBoxTk();
            ComboxLocTrangThai();
        }
        public void LoadData()
        {
            DataGridView_DachSachNhanVien.DataSource = _QuanLyNhanVien_BLL.GetNhanViens();
            loadComboBoxTk();
        }
        public void loadComboBoxTk()
        {
            ComboBox_TaiKhoan.DataSource = _QuanLyNhanVien_BLL.TaiKhoanNhanVien();
            ComboBox_TaiKhoan.DisplayMember = "TenTaiKhoan";
            ComboBox_TaiKhoan.ValueMember = "MaTaiKhoan";
        }
        public void ComboxLocTrangThai()
        {
            ComBox_LocNhanVienTheoChuCai.Items.Add("Tất cả");
            ComBox_LocNhanVienTheoChuCai.Items.Add("Đang làm việc");
            ComBox_LocNhanVienTheoChuCai.Items.Add("Đã nghỉ");
        }

        public void ClearThongTin()
        {
            TextBox_MaNhanVien.Clear();
            TextBox_TenNhanVien.Clear();
            DateTimePicker_NgaySinh.Value = DateTime.Now;
            radioButton_Nam.Checked = false;
            radioButton_Nu.Checked = false;
            TextBox_DiaChi.Clear();
            TextBox_SoDienThoai.Clear();
            TextBox_Email.Clear();
            checkBox_HoatDong.Checked = false;
            checkBox_DaNghi.Checked = false;
            ComboBox_TaiKhoan.SelectedIndex = -1;
        }
        bool isadd = false;
        private void Button_ThemNhanVien_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinNhanVien.Visible = true;
            isadd = true;
        }

        private void Button_Huy_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinNhanVien.Visible = false;
            ClearThongTin();
        }

        private void Button_CapNhap_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_MaNhanVien.Text))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            GroupBox_NhapThongTinNhanVien.Visible = true;
            isadd = false;
        }

        private void Button_Luu_Click(object sender, EventArgs e)
        {

            if (isadd)
            {
                if (string.IsNullOrEmpty(TextBox_TenNhanVien.Text)
                    || !radioButton_Nam.Checked && !radioButton_Nu.Checked
                    || string.IsNullOrEmpty(TextBox_DiaChi.Text)
                    || string.IsNullOrEmpty(TextBox_SoDienThoai.Text)
                    || string.IsNullOrEmpty(TextBox_Email.Text)
                    || !checkBox_DaNghi.Checked && !checkBox_HoatDong.Checked
                    || string.IsNullOrEmpty(ComboBox_TaiKhoan.Text))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin bắt buộc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string Ten = TextBox_TenNhanVien.Text.Trim();
                if (Ten.Length <= 9 || Ten.Length >= 50 || Ten.All(char.IsDigit))
                {
                    MessageBox.Show("Tên nhân viên phải từ 10 đến 50 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (radioButton_Nam.Checked == true && radioButton_Nu.Checked == true)
                {
                    MessageBox.Show("Vui lòng chỉ chọn một giới tính!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string DiaChi = TextBox_DiaChi.Text.Trim();
                if (DiaChi.Length < 9 || DiaChi.Length > 50)
                {
                    MessageBox.Show("Địa chỉ phải từ 10 đến 100 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string SoDienThoai = TextBox_SoDienThoai.Text.Trim();
                if (SoDienThoai.Length != 10 || !SoDienThoai.All(char.IsDigit) || !SoDienThoai.StartsWith("0"))
                {
                    MessageBox.Show("Số điện thoại phải bắt đầu bằng số 0 và gồm đúng 10 chữ số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (DateTimePicker_NgaySinh.Value.Date >= DateTime.Now.Date)
                {
                    MessageBox.Show("Ngày sinh không hợp lệ! Vui lòng chọn lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string Email = TextBox_Email.Text.Trim();
                if (Email.Length < 10 || Email.Length > 50)
                {
                    MessageBox.Show("Email phải có độ dài từ 10 đến 50 ký tự.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(Email, emailRegex))
                {
                    MessageBox.Show("Email không hợp lệ! Vui lòng kiểm tra định dạng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (checkBox_DaNghi.Checked == true && checkBox_HoatDong.Checked == true)
                {
                    MessageBox.Show("Vui lòng chỉ chọn một trạng thái!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                int? maTaiKhoan = (int)ComboBox_TaiKhoan.SelectedValue;
                if (maTaiKhoan == null)
                {
                    MessageBox.Show("Vui lòng chọn tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn thêm nhân viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
                NhanVien nhanVien = new NhanVien
                {
                    TenNhanVien = TextBox_TenNhanVien.Text,
                    NgaySinh = DateOnly.FromDateTime(DateTimePicker_NgaySinh.Value),
                    GioiTinh = radioButton_Nam.Checked ? "Nam" : "Nữ",
                    SoDienThoai = TextBox_SoDienThoai.Text,
                    DiaChi = TextBox_DiaChi.Text,
                    Email = TextBox_Email.Text,
                    TrangThai = checkBox_HoatDong.Checked ? "Đang làm việc" : "Đã nghỉ",
                    MaTaiKhoan = (int)ComboBox_TaiKhoan.SelectedValue
                };
                if (_QuanLyNhanVien_BLL.themNhanVien(nhanVien))
                {
                    ClearThongTin();
                    LoadData();
                    GroupBox_NhapThongTinNhanVien.Visible = false;
                }
                else
                {
                    MessageBox.Show("Thêm nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (DataGridView_DachSachNhanVien.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên cần cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (string.IsNullOrEmpty(TextBox_TenNhanVien.Text)
                    || !radioButton_Nam.Checked && !radioButton_Nu.Checked
                    || string.IsNullOrEmpty(TextBox_DiaChi.Text)
                    || string.IsNullOrEmpty(TextBox_SoDienThoai.Text)
                    || string.IsNullOrEmpty(TextBox_Email.Text)
                    || !checkBox_DaNghi.Checked && !checkBox_HoatDong.Checked
                    || string.IsNullOrEmpty(ComboBox_TaiKhoan.Text))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin bắt buộc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string Ten = TextBox_TenNhanVien.Text.Trim();
                if (Ten.Length <= 9 || Ten.Length >= 50 || Ten.All(char.IsDigit))
                {
                    MessageBox.Show("Tên nhân viên phải từ 10 đến 50 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (radioButton_Nam.Checked == true && radioButton_Nu.Checked == true)
                {
                    MessageBox.Show("Vui lòng chỉ chọn một giới tính!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string DiaChi = TextBox_DiaChi.Text.Trim();
                if (DiaChi.Length < 9 || DiaChi.Length > 50)
                {
                    MessageBox.Show("Địa chỉ phải từ 10 đến 100 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string SoDienThoai = TextBox_SoDienThoai.Text.Trim();
                if (SoDienThoai.Length != 10 || !SoDienThoai.All(char.IsDigit) || !SoDienThoai.StartsWith("0"))
                {
                    MessageBox.Show("Số điện thoại phải bắt đầu bằng số 0 và gồm đúng 10 chữ số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (DateTimePicker_NgaySinh.Value.Date >= DateTime.Now.Date)
                {
                    MessageBox.Show("Ngày sinh không hợp lệ! Vui lòng chọn lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string Email = TextBox_Email.Text.Trim();
                if (Email.Length < 10 || Email.Length > 50)
                {
                    MessageBox.Show("Email phải có độ dài từ 10 đến 50 ký tự.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(Email, emailRegex))
                {
                    MessageBox.Show("Email không hợp lệ! Vui lòng kiểm tra định dạng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (checkBox_DaNghi.Checked == true && checkBox_HoatDong.Checked == true)
                {
                    MessageBox.Show("Vui lòng chỉ chọn một trạng thái!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                int? maTaiKhoan = (int)ComboBox_TaiKhoan.SelectedValue;
                if (maTaiKhoan == null)
                {
                    MessageBox.Show("Vui lòng chọn tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn cập nhật nhân viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
                NhanVien nhanVien = new NhanVien
                {
                    MaNhanVien = int.Parse(TextBox_MaNhanVien.Text),
                    TenNhanVien = TextBox_TenNhanVien.Text,
                    NgaySinh = DateOnly.FromDateTime(DateTimePicker_NgaySinh.Value),
                    GioiTinh = radioButton_Nam.Checked ? "Nam" : "Nữ",
                    SoDienThoai = TextBox_SoDienThoai.Text,
                    DiaChi = TextBox_DiaChi.Text,
                    Email = TextBox_Email.Text,
                    TrangThai = checkBox_HoatDong.Checked ? "Đang làm việc" : "Đã nghỉ",
                    MaTaiKhoan = (int)ComboBox_TaiKhoan.SelectedValue
                };
                if (_QuanLyNhanVien_BLL.CapNhapNhanVien(nhanVien))
                {
                    ClearThongTin();
                    LoadData();
                    GroupBox_NhapThongTinNhanVien.Visible = false;
                }
                else
                {
                    MessageBox.Show("Cập nhật nhân viên thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DataGridView_DachSachNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DataGridView_DachSachNhanVien.Rows[e.RowIndex];
                TextBox_MaNhanVien.Text = row.Cells["ID"].Value.ToString();
                TextBox_TenNhanVien.Text = row.Cells["Name"].Value.ToString();
                DateOnly dateOnly = (DateOnly)row.Cells["Date"].Value;
                DateTimePicker_NgaySinh.Value = dateOnly.ToDateTime(new TimeOnly(0, 0));
                string gioiTinh = row.Cells["sex"].Value.ToString();
                if (gioiTinh == "Nam")
                {
                    radioButton_Nam.Checked = true;
                    radioButton_Nu.Checked = false;
                }
                else if (gioiTinh == "Nữ")
                {
                    radioButton_Nam.Checked = false;
                    radioButton_Nu.Checked = true;
                }
                TextBox_DiaChi.Text = row.Cells["address"].Value.ToString();
                TextBox_SoDienThoai.Text = row.Cells["PhoneNumber"].Value.ToString();
                TextBox_Email.Text = row.Cells["mail"].Value.ToString();
                string trangThai = row.Cells["statu"].Value.ToString();
                if (trangThai == "Đang làm việc")
                {
                    checkBox_HoatDong.Checked = true;
                    checkBox_DaNghi.Checked = false;
                }
                else if (trangThai == "Đã nghỉ")
                {
                    checkBox_HoatDong.Checked = false;
                    checkBox_DaNghi.Checked = true;
                }
                string taiKhoan = row.Cells["Account"].Value.ToString();
                ComboBox_TaiKhoan.Text = taiKhoan;
            }
        }

        private void Button_XoaNhanVien_Click(object sender, EventArgs e)
        {
            if (DataGridView_DachSachNhanVien.SelectedRows.Count > 0)
            {

                if (string.IsNullOrEmpty(TextBox_MaNhanVien.Text))
                {
                    MessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
                int maNhanVien = int.Parse(TextBox_MaNhanVien.Text);
                if (_QuanLyNhanVien_BLL.XoaNhanVien(maNhanVien))
                {
                    ClearThongTin();
                    LoadData();
                }

            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void Button_XuatFile_Click(object sender, EventArgs e)
        {
            var qlnv = _QuanLyNhanVien_BLL.GetNhanViens();
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Workbook|*.xlsx";
                sfd.Title = "Chọn nơi lưu file";
                sfd.FileName = "DanhSachNhanVien.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Danh sách nhân viên");
                        worksheet.Cell(1, 1).Value = "STT";
                        worksheet.Cell(1, 2).Value = "ID Nhân Viên";
                        worksheet.Cell(1, 3).Value = "Tên Nhân Viên";
                        worksheet.Cell(1, 4).Value = "Giới Tính";
                        worksheet.Cell(1, 5).Value = "Ngày Sinh";
                        worksheet.Cell(1, 6).Value = "Số Điện Thoại";
                        worksheet.Cell(1, 7).Value = "Email";
                        worksheet.Cell(1, 8).Value = "Địa Chỉ";
                        worksheet.Cell(1, 9).Value = "Trạng Thái";
                        worksheet.Cell(1, 10).Value = "Tài Khoản";

                        int row = 2;
                        foreach (var nv in qlnv)
                        {
                            worksheet.Cell(row, 1).Value = nv.STT;
                            worksheet.Cell(row, 2).Value = nv.MaNhanVien;
                            worksheet.Cell(row, 3).Value = nv.TenNhanVien;
                            worksheet.Cell(row, 4).Value = nv.GioiTinh;
                            worksheet.Cell(row, 5).Value = nv.NgaySinh.HasValue
                            ? nv.NgaySinh.Value.ToString("dd/MM/yyyy")
                            : "";
                            worksheet.Cell(row, 6).Value = nv.SoDienThoai;
                            worksheet.Cell(row, 7).Value = nv.Email;
                            worksheet.Cell(row, 8).Value = nv.DiaChi;
                            worksheet.Cell(row, 9).Value = nv.TrangThai;
                            worksheet.Cell(row, 10).Value = nv.TaiKhoan;
                            row++;
                        }
                        workbook.SaveAs(sfd.FileName);
                        MessageBox.Show("Xuất file thành công ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void Button_TimKiem_Click(object sender, EventArgs e)
        {
            string keyword = TextBox_TimKiem.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                return;
            }
            List<QuanLyNhanVien_DTO> ketQua = _QuanLyNhanVien_BLL.TimKiemNhanVien(keyword);
            if (ketQua.Any())
            {
                DataGridView_DachSachNhanVien.DataSource = ketQua;
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhân viên với từ khóa đã nhập!", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void ComBox_LocNhanVienTheoChuCai_SelectedIndexChanged(object sender, EventArgs e)
        {
            string TrangThai = ComBox_LocNhanVienTheoChuCai.SelectedItem.ToString();
            var DanhSach = _QuanLyNhanVien_BLL.LocNhanVienTheoTrangThai(TrangThai);
            DataGridView_DachSachNhanVien.DataSource = DanhSach;
        }
    }
}
