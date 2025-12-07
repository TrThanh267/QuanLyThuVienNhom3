using ClosedXML.Excel;
using Nhom3ThuVienBanNhap.DTO;
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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyThuVienNhom3.GUI.UC
{
    public partial class UserControl_QuanLyChamCong : UserControl
    {
        private QuanLyChamCong_BLL _BLL = new QuanLyChamCong_BLL();
        public UserControl_QuanLyChamCong()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }
            LocChamCong();
            loadComboBoxNhanVien();
            LoadComboBoxLoaiCong();
            LoadComBoxCaLam();
            LoadData();
            PhanQuyen();
        }
        public void PhanQuyen()
        {
            if (UserSession.TaiKhoanHienTai != null)
            {
                if (UserSession.TaiKhoanHienTai.MaVaiTro == 2)
                {
                    Button_XoaChamCong.Enabled = false;
                }
            }
        }
        public void Loaddulieu()
        {
            LocChamCong();
            loadComboBoxNhanVien();
            LoadComboBoxLoaiCong();
            LoadComBoxCaLam();
            LoadData();
        }
        public void LoadData()
        {
            DataGridView_DanhsachCC.DataSource = _BLL.GetListChamCong();
        }
        public void loadComboBoxNhanVien()
        {
            var danhSachNV = _BLL.GetNhanViens()?.ToList();

            if (danhSachNV == null || !danhSachNV.Any())
            {
                MessageBox.Show("Không có nhân viên nào!");
                return;
            }

            danhSachNV.Insert(0, new NhanVien { MaNhanVien = 0, TenNhanVien = "--Chọn nhân viên--" });

            ComboBox_NhanVien.DataSource = danhSachNV;
            ComboBox_NhanVien.DisplayMember = "TenNhanVien";
            ComboBox_NhanVien.ValueMember = "MaNhanVien";

            ComboBox_NhanVien.SelectedIndexChanged += ComboBox_NhanVien_SelectedIndexChanged;

        }
        public void LoadComBoxCaLam()
        {
            var danhsachCaLam = _BLL.GetCaLams()?.ToList();

            if (danhsachCaLam == null || !danhsachCaLam.Any())
            {
                MessageBox.Show("Không có ca làm nào!");
                return;
            }

            danhsachCaLam.Insert(0, new CaLam { MaCaLam = 0, TenCaLam = "--Chọn Ca làm--" });

            ComboBox_CaLam.DataSource = danhsachCaLam;
            ComboBox_CaLam.DisplayMember = "TenCaLam";
            ComboBox_CaLam.ValueMember = "MaCaLam";
        }
        public void LoadComboBoxLoaiCong()
        {
            var danhsachLoaiCong = _BLL.GetLoaiCong()?.ToList();

            if (danhsachLoaiCong == null || !danhsachLoaiCong.Any())
            {
                MessageBox.Show("Không có ca làm nào!");
                return;
            }

            danhsachLoaiCong.Insert(0, new LoaiCong { MaLoaiCong = 0, TenloaiCong = "--Chọn Ca làm--" });

            ComboBox_LoaiCong.DataSource = danhsachLoaiCong;
            ComboBox_LoaiCong.DisplayMember = "TenLoaiCong";
            ComboBox_LoaiCong.ValueMember = "MaLoaiCong";
        }

        public void LocChamCong()
        {
            int ngay = DateTimePicker_BoLocChamCong.Value.Day;
            int thang = DateTimePicker_BoLocChamCong.Value.Month;
            int nam = DateTimePicker_BoLocChamCong.Value.Year;

            List<QuanLyChamCong_DTO> kq = _BLL.LocChamCong(ngay, thang, nam);

            if (kq.Any())
            {
                DataGridView_DachSachChamCong.DataSource = kq;
            }
            else
            {
                DataGridView_DachSachChamCong.DataSource = null;
            }
        }
        private void dateTimePicker_TinhLuongThang_ValueChanged(object sender, EventArgs e)
        {
            int thang = dateTimePicker_TinhLuongThang.Value.Month;
            int nam = dateTimePicker_TinhLuongThang.Value.Year;
        }
        public void ThongTinNhanVien(NhanVien nhanVien)
        {
            if (nhanVien != null)
            {
                TextBox_MaNhanVien.Text = nhanVien.MaNhanVien.ToString();
                TextBox_TenNhanVien.Text = nhanVien.TenNhanVien;
                if (nhanVien.NgaySinh.HasValue)
                {
                    DateTimePicker_Ngáyinh.Value = nhanVien.NgaySinh.Value.ToDateTime(TimeOnly.MinValue);
                }
                else
                {
                    DateTimePicker_Ngáyinh.Value = DateTime.Now;
                }

                TextBox_GioiTinh.Text = nhanVien.GioiTinh;
                TextBox_DiaChi.Text = nhanVien.DiaChi;
                TextBox_SoDienThoai.Text = nhanVien.SoDienThoai;
                TextBox_Email.Text = nhanVien.Email;
                if (nhanVien.TrangThai == "Đang làm việc")
                {
                    radioButton_HoatDong.Checked = true;
                }
                else
                {
                    radioButton_DaNghi.Checked = true;
                }
                TextBox_TaiKhoan.Text = nhanVien.MaTaiKhoanNavigation != null
                                        ? nhanVien.MaTaiKhoanNavigation.TenTaiKhoan
                                        : "Không có tài khoản";

            }

        }

        private void Button_ChamCongVao_Click(object sender, EventArgs e)
        {
            if (ComboBox_NhanVien.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên để chấm công");
                return;
            }
            if (ComboBox_CaLam.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng chọn ca làm để chấm công");
                return;
            }
            if (ComboBox_LoaiCong.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng chọn loại công để chấm công");
                return;
            }
            TextBox_GioVao.Clear();
            TextBox_GioRa.Clear();
            TextBox_PhutRa.Clear();
            TextBox_PhutVao.Clear();
            ChamCong chamCong = new ChamCong()
            {
                MaCaLam = (int)ComboBox_CaLam.SelectedValue,
                MaLoaiCong = (int)ComboBox_LoaiCong.SelectedValue,
                NamLam = DateTime.Now.Year,
                ThangLam = DateTime.Now.Month,
                NgayLam = DateTime.Now.Day,
                MaNhanVien = (int)ComboBox_NhanVien.SelectedValue,
                GioVao = DateTime.Now.Hour,
                PhutVao = DateTime.Now.Minute,
                GioRa = 0,
                PhutRa = 0
            };
            _BLL.ThemChamCong(chamCong);
            LocChamCong();
        }

        private void ComboBox_NhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            NhanVien nhanVien = ComboBox_NhanVien.SelectedItem as NhanVien;
            if (!int.TryParse(ComboBox_NhanVien.SelectedValue.ToString(), out int idnv))
            {
                return;
            }
            NhanVien IDNv = _BLL.GetNhanViens().FirstOrDefault(x => x.MaNhanVien == idnv);
            if (IDNv != null)
            {
                ThongTinNhanVien(IDNv);
            }
        }

        private void Button_ChamCongRa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_MaChamCong.Text))
            {
                MessageBox.Show("Vui lòng chọn bản ghi chấm công trước khi cập nhật giờ ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (int.Parse(TextBox_GioRa.Text.ToString()) != 0 || int.Parse(TextBox_PhutRa.Text.ToString()) != 0)
            {
                DialogResult dialogResult = MessageBox.Show("Bạn đã chấm công ra trước đó. Bạn có chắc chắn muốn cập nhật giờ ra mới không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }

            ChamCong chamCong = new ChamCong
            {
                MaChamCong = int.Parse(TextBox_MaChamCong.Text),
                GioRa = DateTime.Now.Hour,
                PhutRa = DateTime.Now.Minute,
            };

            _BLL.CapNhapChamCong(chamCong);
            LocChamCong();
            LoadData();
        }

        private void DataGridView_DachSachChamCong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                TextBox_MaChamCong.Text = DataGridView_DachSachChamCong.Rows[e.RowIndex].Cells["maChamCong"].Value.ToString().Trim();
                ComboBox_CaLam.Text = DataGridView_DachSachChamCong.Rows[e.RowIndex].Cells["caLam"].Value.ToString().Trim();
                ComboBox_LoaiCong.Text = DataGridView_DachSachChamCong.Rows[e.RowIndex].Cells["loaiCong"].Value.ToString().Trim();
                TextBox_NamLam.Text = DataGridView_DachSachChamCong.Rows[e.RowIndex].Cells["namLam"].Value.ToString().Trim();
                TextBox_ThangLam.Text = DataGridView_DachSachChamCong.Rows[e.RowIndex].Cells["thangLam"].Value.ToString().Trim();
                TextBox_NgayLam.Text = DataGridView_DachSachChamCong.Rows[e.RowIndex].Cells["ngayLam"].Value.ToString().Trim();
                TextBox_GioVao.Text = DataGridView_DachSachChamCong.Rows[e.RowIndex].Cells["gioVao"].Value.ToString().Trim();
                TextBox_PhutVao.Text = DataGridView_DachSachChamCong.Rows[e.RowIndex].Cells["phutVao"].Value.ToString().Trim();
                TextBox_GioRa.Text = DataGridView_DachSachChamCong.Rows[e.RowIndex].Cells["gioRa"].Value.ToString().Trim();
                TextBox_PhutRa.Text = DataGridView_DachSachChamCong.Rows[e.RowIndex].Cells["phutRa"].Value.ToString().Trim();
                var maNV = DataGridView_DachSachChamCong.Rows[e.RowIndex].Cells["MaNhanVien"].Value;
                if (maNV != null && int.TryParse(maNV.ToString(), out int idNV))
                {
                    ComboBox_NhanVien.SelectedValue = idNV;
                }


                if (int.TryParse(TextBox_GioVao.Text, out int gioVao) &&
                    int.TryParse(TextBox_PhutVao.Text, out int phutVao) &&
                    int.TryParse(TextBox_GioRa.Text, out int gioRa) &&
                    int.TryParse(TextBox_PhutRa.Text, out int phutRa))
                {

                    if (gioRa == 0 || phutRa == 0)
                    {
                        TextBox_ThoiGianLam.Text = "Chưa chấm công ra";
                    }
                    else
                    {
                        int Gio = gioRa - gioVao;
                        int Phut = phutRa - phutVao;

                        double tongGio = Gio + (Phut / 60.0);
                        TextBox_ThoiGianLam.Text = $"{tongGio:F2} giờ";
                    }

                }
            }
        }

        private void DateTimePicker_BoLocChamCong_ValueChanged(object sender, EventArgs e)
        {
            LocChamCong();
        }

        private void Button_HienThiDanhSach_Click(object sender, EventArgs e)
        {
            GroupBox_DanhSachChamCong.Visible = true;
        }

        private void Button_TatDanhSachCC_Click(object sender, EventArgs e)
        {
            GroupBox_DanhSachChamCong.Visible = false;
        }

        private void Button_TinhLuong_Click(object sender, EventArgs e)
        {
            if (ComboBox_NhanVien.SelectedValue == null)
            {
                MessageBox.Show("Chưa chọn nhân viên!", "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int idnv = (int)ComboBox_NhanVien.SelectedValue;

            int thang = dateTimePicker_TinhLuongThang.Value.Month;
            int nam = dateTimePicker_TinhLuongThang.Value.Year;

            int Thang = dateTimePicker_TinhLuongThang.Value.Month;
            int Nam = dateTimePicker_TinhLuongThang.Value.Year;

            int ngayCongHanhChinh = _BLL.KiemTraLamChinh(thang, nam, idnv);
            int ngayCongNgoaiGio = _BLL.KiemTraLamThem(thang, nam, idnv);
            int ngayCongNgayLe = _BLL.KiemTraLamNgayLe(thang, nam, idnv);

            double donGiaHanhChinh = 100000;
            double donGiaNgoaiGio = 150000;
            double donGiaNgayLe = 200000;

            double tongLuong = (ngayCongHanhChinh * donGiaHanhChinh)
                             + (ngayCongNgoaiGio * donGiaNgoaiGio)
                             + (ngayCongNgayLe * donGiaNgayLe);

            TextBox_SoTienCong.Text = tongLuong.ToString("N0");

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string tk = TextBox_TimKiem.Text.Trim();

            if (string.IsNullOrEmpty(tk))
            {
                MessageBox.Show("Vui lòng nhập thông tin tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<QuanLyChamCong_DTO> ketQua = _BLL.TimKiem(tk);

            if (ketQua.Any())
            {
                DataGridView_DanhsachCC.DataSource = ketQua;
            }
            else
            {
                MessageBox.Show("Không tìm thấy mã nhân viên: " + tk, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TextBox_TimKiem.Clear();
                LoadData();
            }
        }

        private void Button_XuatFile_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xuất danh sách chấm công không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }

            var qlcc = _BLL.GetListChamCong();
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Workbook|*.xlsx";
                sfd.Title = "Chọn nơi lưu file";
                sfd.FileName = "DanhSachNhanVienChamCong.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string filePath = sfd.FileName;

                    if (System.IO.File.Exists(filePath))
                    {
                        MessageBox.Show("File đã tồn tại. Vui lòng chọn tên khác hoặc xóa file cũ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    try
                    {
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Danh sách nhân viên");
                            worksheet.Cell(1, 1).Value = "STT";
                            worksheet.Cell(1, 2).Value = "ID chấm công";
                            worksheet.Cell(1, 3).Value = "Ca làm";
                            worksheet.Cell(1, 4).Value = "Loại Công";
                            worksheet.Cell(1, 5).Value = "Năm ";
                            worksheet.Cell(1, 6).Value = "Tháng";
                            worksheet.Cell(1, 7).Value = "Ngày";
                            worksheet.Cell(1, 8).Value = "Giờ vào";
                            worksheet.Cell(1, 9).Value = "Phút Vào";
                            worksheet.Cell(1, 10).Value = "Giờ ra ";
                            worksheet.Cell(1, 11).Value = "Phút Ra";
                            worksheet.Cell(1, 12).Value = "ID Nhân viên";
                            worksheet.Cell(1, 13).Value = "Tên nhân viên";
                            worksheet.Cell(1, 14).Value = "Giới tính";
                            worksheet.Cell(1, 15).Value = "SĐT";
                            worksheet.Cell(1, 16).Value = "CCCD";
                            worksheet.Cell(1, 17).Value = "Địa chỉ";

                            int row = 2;
                            foreach (var nv in qlcc)
                            {
                                worksheet.Cell(row, 1).Value = nv.STT;
                                worksheet.Cell(row, 2).Value = nv.MaChamCong;
                                worksheet.Cell(row, 3).Value = nv.CaLam;
                                worksheet.Cell(row, 4).Value = nv.LoaiCong;
                                worksheet.Cell(row, 5).Value = nv.NamLam;
                                worksheet.Cell(row, 6).Value = nv.ThangLam;
                                worksheet.Cell(row, 7).Value = nv.NgayLam;
                                worksheet.Cell(row, 8).Value = nv.GioVao;
                                worksheet.Cell(row, 9).Value = nv.PhutVao;
                                worksheet.Cell(row, 10).Value = nv.GioRa;
                                worksheet.Cell(row, 11).Value = nv.PhutRa;
                                worksheet.Cell(row, 12).Value = nv.MaNhanVien;
                                worksheet.Cell(row, 13).Value = nv.NhanVien;
                                row++;
                            }

                            workbook.SaveAs(filePath);
                        }

                        MessageBox.Show($"Xuất file thành công: {Path.GetFileName(filePath)}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lưu file thất bại: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void Button_XoaChamCong_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(TextBox_MaChamCong.Text.Trim(), out int idcc))
            {
                MessageBox.Show("Mã chấm công không hợp lệ!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult dialogResult = MessageBox.Show(
            "Bạn có chắc chắn muốn xóa bản ghi chấm công này không?",
            "Xác nhận xóa",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.No)
            {
                return;
            }

            _BLL.XoaCC(idcc);
            LoadData();

        }

        private void DataGridView_DanhsachCC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                var row = DataGridView_DanhsachCC.Rows[e.RowIndex];

                TextBox_MaChamCong.Text = row.Cells["ID"].Value?.ToString().Trim() ?? "";
                ComboBox_CaLam.Text = row.Cells["Ca"].Value?.ToString().Trim() ?? "";
                ComboBox_LoaiCong.Text = row.Cells["Cong"].Value?.ToString().Trim() ?? "";
                TextBox_NamLam.Text = row.Cells["Year"].Value?.ToString().Trim() ?? "";
                TextBox_ThangLam.Text = row.Cells["Month"].Value?.ToString().Trim() ?? "";
                TextBox_NgayLam.Text = row.Cells["Day"].Value?.ToString().Trim() ?? "";
                TextBox_GioVao.Text = row.Cells["HourIn"].Value?.ToString().Trim() ?? "";
                TextBox_PhutVao.Text = row.Cells["MinuteIn"].Value?.ToString().Trim() ?? "";
                TextBox_GioRa.Text = row.Cells["HourOut"].Value?.ToString().Trim() ?? "";
                TextBox_PhutRa.Text = row.Cells["MinuteOut"].Value?.ToString().Trim() ?? "";
            }

        }
    }
}
