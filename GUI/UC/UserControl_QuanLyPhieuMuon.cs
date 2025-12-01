using ClosedXML.Excel;
using QuanLyThuVienNhom3.BLL;
using QuanLyThuVienNhom3.DTO;
using QuanLyThuVienNhom3.Models;
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
    public partial class UserControl_QuanLyPhieuMuon : UserControl
    {
        private QuanLyPhieuTra_BLL _PhieuTraBLL = new QuanLyPhieuTra_BLL();
        private QuanLyPhieuMuon_BLL _PhieuMuonBLL = new QuanLyPhieuMuon_BLL();
        private QuanLyChiTietPhieuMuon_BLL _PhieuMuonCTBLL = new QuanLyChiTietPhieuMuon_BLL();
        public UserControl_QuanLyPhieuMuon()
        {
            InitializeComponent();
            LoadData();
            LoadComBoBox();
        }
        public void LoadData()
        {
            DataGridView_PhieuTra.DataSource = _PhieuTraBLL.GetListPhieuTra();
            DataGridView_PhieuMuon.DataSource = _PhieuMuonBLL.GetListPM();
            DataGridView_PhieuMonCT.DataSource = _PhieuMuonCTBLL.GetListPMCT();
        }
        public void LoadComBoBox()
        {
            var danhSachNV = _PhieuMuonBLL.GetNhanVien()?.ToList();

            if (danhSachNV == null || !danhSachNV.Any())
            {
                MessageBox.Show("Không có nhân viên nào!");
                return;
            }

            danhSachNV.Insert(0, new NhanVien { MaNhanVien = 0, TenNhanVien = "--Chọn nhân viên--" });
            ComboBox_NhanVien.DataSource = danhSachNV;
            ComboBox_NhanVien.DisplayMember = "TenNhanVien";
            ComboBox_NhanVien.ValueMember = "MaNhaNVien";



            var danhSachdg = _PhieuMuonBLL.GetDocGia()?.ToList();

            if (danhSachdg == null || !danhSachdg.Any())
            {
                MessageBox.Show("Không có độc giả nào!");
                return;
            }
            danhSachdg.Insert(0, new DocGium { MaDocGia = 0, TenDocGia = "--Chọn độc giả--" });
            ComboBox_DocGia.DataSource = danhSachdg;
            ComboBox_DocGia.DisplayMember = "TenDocGia";
            ComboBox_DocGia.ValueMember = "MaDocGia";



            var danhSach = _PhieuMuonCTBLL.GetSaches()?.ToList();

            if (danhSach == null || !danhSach.Any())
            {
                MessageBox.Show("Không có sách nào!");
                return;
            }
            danhSach.Insert(0, new Sach { MaSach = 0, TenSach = "--Chọn sách--" });
            ComboBox_Sach.DataSource = danhSach;
            ComboBox_Sach.DisplayMember = "TenSach";
            ComboBox_Sach.ValueMember = "MaSach";




            var danhSachPM = _PhieuMuonCTBLL.GetPhieuMuons()?.ToList();

            if (danhSachPM == null || !danhSachPM.Any())
            {
                MessageBox.Show("Không có phiếu mượn nào chưa trả!");
                return;
            }
            danhSachPM.Insert(0, new PhieuMuon { MaPhieuMuon = 0 });
            ComboBox_PhieuMuon.DataSource = danhSachPM;
            ComboBox_PhieuMuon.ValueMember = "MaPhieuMuon";
        }
        bool isaddofPM = false;
        bool isaddofCTPM = false;
        public void ClearFormPhieuMuon()
        {
            TextBox_MaPhieuMuon.Clear();
            TextBox_SoLuong.Clear();
            ComboBox_NhanVien.SelectedIndex = 0;
            ComboBox_DocGia.SelectedIndex = 0;
            radioButton_ChuaTra.Checked = false;
            radioButton_DaTra.Checked = false;
            DateTimePicker_ThoiGianMuon.Value = DateTime.Now;
            DateTimePicker_ThoiHanTra.Value = DateTime.Now;

        }
        public void ClearFormPMCT()
        {
            TextBox_MaCTPM.Clear();
            TextBox_TinhTrangSach.Clear();
            TextBox_SoLuongMuon.Clear();
            ComboBox_Sach.SelectedIndex = 0;
            ComboBox_PhieuMuon.SelectedIndex = 0;
        }
        private void Button_Them_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinPhieuMuon.Visible = true;
            isaddofPM = true;
            ClearFormPhieuMuon();
        }

        private void Button_CapNhap_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinPhieuMuon.Visible = true;
            isaddofPM = false;
            if (string.IsNullOrWhiteSpace(TextBox_MaPhieuMuon.Text))
            {
                MessageBox.Show(
                    "Vui lòng chọn phiếu mượn cần cập nhật!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }
        }

        private void Button_Huy_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinPhieuMuon.Visible = false;
            ClearFormPhieuMuon();
        }

        private void Button_ThemCTPM_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinChiTietPhieuMuon.Visible = true;
            isaddofCTPM = true;
        }

        private void Button_CapNhapCTPM_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinChiTietPhieuMuon.Visible = true;
            isaddofCTPM = false;
            if (DataGridView_PhieuMonCT.CurrentRow == null)
            {
                MessageBox.Show(
                    "Vui lòng chọn một Chi tiết Phiếu Mượn để thực hiện thao tác!",
                    "Chưa chọn dòng",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinChiTietPhieuMuon.Visible = false;
            isaddofCTPM = false;
            ClearFormPMCT();
        }

        private void Button_Luu_Click(object sender, EventArgs e)
        {
            if (isaddofPM)
            {
                DialogResult dialogResult = MessageBox.Show(
                "Bạn có muốn thêm Phiếu Mượn không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }

                if (string.IsNullOrEmpty(TextBox_SoLuong.Text)
                || (!radioButton_ChuaTra.Checked && !radioButton_DaTra.Checked)
                || ComboBox_NhanVien.SelectedIndex == 0
                || ComboBox_DocGia.SelectedIndex == 0)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin bắt buộc!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (DateTimePicker_ThoiGianMuon.Value.Date >= DateTimePicker_ThoiHanTra.Value.Date)
                {
                    MessageBox.Show("Ngày mượn phải nhỏ hơn ngày trả!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int soLuong;
                if (!int.TryParse(TextBox_SoLuong.Text, out soLuong))
                {
                    MessageBox.Show("Số lượng chỉ được nhập số!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (soLuong <= 0)
                {
                    MessageBox.Show("Số lượng phải lớn hơn 0!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                if (radioButton_DaTra.Checked)
                {
                    DateTime ngayMuon = DateTimePicker_ThoiGianMuon.Value.Date;
                    DateTime ngayHienTai = DateTime.Now.Date;

                    if (ngayMuon == ngayHienTai)
                    {
                        MessageBox.Show("Không thể chọn 'Đã trả' khi ngày mượn là hôm nay!",
                                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        radioButton_DaTra.Checked = false;
                        return;
                    }
                }

                PhieuMuon phieuMuon = new PhieuMuon
                {
                    ThoiGianMuon = DateOnly.FromDateTime(DateTimePicker_ThoiGianMuon.Value),
                    ThoihanTra = DateOnly.FromDateTime(DateTimePicker_ThoiHanTra.Value),
                    SoLuong = soLuong,
                    SoLuongBanDau = soLuong,
                    TrangThai = radioButton_ChuaTra.Checked ? "Chưa trả" : "Đã trả",
                    MaNhanVien = (int)ComboBox_NhanVien.SelectedValue,
                    MaDocGia = (int)ComboBox_DocGia.SelectedValue
                };

                if (_PhieuMuonBLL.ThemPM(phieuMuon))
                {
                    LoadData();
                    ClearFormPhieuMuon();
                    LoadComBoBox();
                    GroupBox_NhapThongTinPhieuMuon.Visible = false;
                }
                else
                {
                    MessageBox.Show("Thêm phiếu mượn thất bại!");
                    return;
                }
            }
            else
            {
                if (DataGridView_PhieuMuon.CurrentRow != null)
                {
                    if (!int.TryParse(DataGridView_PhieuMuon.CurrentRow.Cells["maPhieuMuonPM"].Value.ToString(), out int maPhieuMuon))
                    {
                        MessageBox.Show("Không tìm thấy Mã Phiếu Mượn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    DateTimePicker_ThoiGianMuon.ValueChanged -= DateTimePicker_ThoiGianMuon_ValueChanged;
                    DialogResult dialogResult = MessageBox.Show(
                    "Bạn có muốn CẬP NHẬT Phiếu Mượn này không?",
                    "Xác nhận Cập nhật",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.No)
                    {
                        DateTimePicker_ThoiGianMuon.ValueChanged += DateTimePicker_ThoiGianMuon_ValueChanged;
                        return;
                    }

                    if (string.IsNullOrEmpty(TextBox_SoLuong.Text)
                    || (!radioButton_ChuaTra.Checked && !radioButton_DaTra.Checked)
                    || ComboBox_NhanVien.SelectedIndex == 0
                    || ComboBox_DocGia.SelectedIndex == 0)
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin bắt buộc!",
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (DateTimePicker_ThoiGianMuon.Value.Date >= DateTimePicker_ThoiHanTra.Value.Date)
                    {
                        MessageBox.Show("Ngày mượn phải nhỏ hơn ngày trả!",
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int soLuongMoi;
                    if (!int.TryParse(TextBox_SoLuong.Text, out soLuongMoi) || soLuongMoi <= 0)
                    {
                        MessageBox.Show(
                            "Số lượng phải là một số nguyên dương! Vui lòng nhập lại.",
                            "Lỗi nhập liệu",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }
                    string trangThai = radioButton_ChuaTra.Checked ? "Chưa trả" : "Đã trả";
                    if (trangThai == "Đã trả")
                    {
                        MessageBox.Show(
                            "Chỉ đặt trạng thái 'Đã trả' khi đã xử lý trả sách trong CTPM!",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                        return;
                    }

                    PhieuMuon phieuMuon = new PhieuMuon
                    {
                        MaPhieuMuon = maPhieuMuon,
                        ThoiGianMuon = DateOnly.FromDateTime(DateTimePicker_ThoiGianMuon.Value),
                        ThoihanTra = DateOnly.FromDateTime(DateTimePicker_ThoiHanTra.Value),
                        SoLuong = soLuongMoi,
                        TrangThai = trangThai,
                        MaNhanVien = (int)ComboBox_NhanVien.SelectedValue,
                        MaDocGia = (int)ComboBox_DocGia.SelectedValue
                    };

                    if (_PhieuMuonBLL.CapNhapPhieuMuon(phieuMuon))
                    {
                        LoadData();
                        ClearFormPhieuMuon();
                        LoadComBoBox();
                        GroupBox_NhapThongTinPhieuMuon.Visible = false;
                    }
                    DateTimePicker_ThoiGianMuon.ValueChanged += DateTimePicker_ThoiGianMuon_ValueChanged;
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn Phiếu Mượn cần cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void DateTimePicker_ThoiGianMuon_ValueChanged(object sender, EventArgs e)
        {
            if (DateTimePicker_ThoiGianMuon.Value.Date != DateTime.Today)
            {
                MessageBox.Show("Ngày mượn chỉ được chọn là hôm nay!",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DateTimePicker_ThoiGianMuon.Value = DateTime.Today;
            }

        }

        private void Button_LuuCTPM_Click(object sender, EventArgs e)
        {
            if (isaddofCTPM)
            {
                DialogResult dialogResult = MessageBox.Show(
                "Bạn có muốn thêm Chi tiết Phiếu Mượn không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }

                if (string.IsNullOrEmpty(TextBox_TinhTrangSach.Text)
                    || string.IsNullOrEmpty(TextBox_SoLuongMuon.Text)
                    || ComboBox_Sach.SelectedIndex == 0
                    || ComboBox_PhieuMuon.SelectedIndex == 0)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin bắt buộc!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string tinhTrangSach = TextBox_TinhTrangSach.Text;
                if (tinhTrangSach.Length < 3)
                {
                    MessageBox.Show("Tình trạng sách phải có ít nhất 3 ký tự!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int soLuongMuon;
                if (!int.TryParse(TextBox_SoLuongMuon.Text, out soLuongMuon))
                {
                    MessageBox.Show("Số lượng mượn chỉ được nhập số!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (soLuongMuon <= 0)
                {
                    MessageBox.Show("Số lượng mượn phải lớn hơn 0!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                ChiTietPhieuMuon chiTietPhieuMuon = new ChiTietPhieuMuon()
                {
                    SoLuongMuon = int.Parse(TextBox_SoLuongMuon.Text),
                    TinhTrangSach = TextBox_TinhTrangSach.Text,
                    MaSach = (int)ComboBox_Sach.SelectedValue,
                    MaPhieuMuon = (int)ComboBox_PhieuMuon.SelectedValue
                };
                if (_PhieuMuonCTBLL.themCTPM(chiTietPhieuMuon))
                {
                    LoadData();
                    ClearFormPMCT();
                    LoadComBoBox();
                    GroupBox_NhapThongTinChiTietPhieuMuon.Visible = false;
                }
                else
                {
                    MessageBox.Show("Thêm chi tiết phiếu mượn thất bại!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                if (DataGridView_PhieuMonCT.CurrentRow != null)
                {
                    DialogResult dialogResult = MessageBox.Show(
                    "Bạn có muốn cập nhập Chi tiết Phiếu Mượn này không?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.No)
                    {
                        return;
                    }

                    if (!int.TryParse(DataGridView_PhieuMonCT.CurrentRow.Cells["maChiTietPhieuMuonPMCT"].Value.ToString(), out int maCTPM))
                    {
                        MessageBox.Show("Không tìm thấy Mã Chi tiết Phiếu Mượn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (string.IsNullOrEmpty(TextBox_SoLuongMuon.Text) || ComboBox_Sach.SelectedIndex == 0)
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin bắt buộc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    int soLuongMoi;
                    if (!int.TryParse(TextBox_SoLuongMuon.Text, out soLuongMoi) || soLuongMoi <= 0)
                    {
                        MessageBox.Show("Số lượng mượn phải là số nguyên dương!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    ChiTietPhieuMuon ctpmCapNhat = new ChiTietPhieuMuon
                    {
                        MaChiTietPhieuMuon = maCTPM,
                        SoLuongMuon = soLuongMoi,
                        MaSach = (int)ComboBox_Sach.SelectedValue,
                        MaPhieuMuon = (int)DataGridView_PhieuMonCT.CurrentRow.Cells["maPhieuMuonPMCT"].Value
                    };
                    if (_PhieuMuonCTBLL.capNhapCTPM(ctpmCapNhat))
                    {
                        LoadData();
                        ClearFormPMCT();
                        GroupBox_NhapThongTinChiTietPhieuMuon.Visible = false;
                        LoadComBoBox();
                    }
                }
            }
        }

        private void Button_DaTraSach_Click(object sender, EventArgs e)
        {

            if (DataGridView_PhieuMonCT.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một chi tiết phiếu mượn để trả!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialogResult = MessageBox.Show(
                "Bạn có muốn trả sách đã chọn này không?",
                "Xác nhận Trả Sách",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialogResult == DialogResult.No)
            {
                return;
            }
            DateTime NgayTra = DateTime.Now;
            int maCTPM = Convert.ToInt32(DataGridView_PhieuMonCT.CurrentRow.Cells["maChiTietPhieuMuonPMCT"].Value);
            string tinhTrangSachMoi = TextBox_TInhTrangSachLucTra.Text.Trim();

            if (string.IsNullOrEmpty(tinhTrangSachMoi))
            {
                MessageBox.Show("Vui lòng nhập hoặc chọn Tình Trạng Sách khi trả!",
                                "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            bool daHoanTatPhieuMuon = _PhieuMuonCTBLL.TraSach(maCTPM, tinhTrangSachMoi,NgayTra);
            if (!string.IsNullOrEmpty(_PhieuMuonCTBLL.LastError))
            {
                MessageBox.Show(_PhieuMuonCTBLL.LastError, "Lỗi Xử lý Trả Sách", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (daHoanTatPhieuMuon)
                {
                    MessageBox.Show("Đã trả sách thành công! Phiếu mượn đã được hoàn tất ('Đã trả').",
                                    "Hoàn tất Phiếu Mượn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Đã trả sách thành công. Phiếu mượn vẫn còn sách chưa trả.",
                                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                LoadData();
                ClearFormPMCT();
                LoadComBoBox();
            }

        }

        private void DataGridView_PhieuMonCT_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                TextBox_MaCTPM.Text = DataGridView_PhieuMonCT.Rows[e.RowIndex].Cells["maChiTietPhieuMuonPMCT"].Value.ToString().Trim();
                TextBox_SoLuongMuon.Text = DataGridView_PhieuMonCT.Rows[e.RowIndex].Cells["soLuongMuonPMCT"].Value.ToString().Trim();
                TextBox_TinhTrangSach.Text = DataGridView_PhieuMonCT.Rows[e.RowIndex].Cells["tinhTrangSachPMCT"].Value.ToString();
                ComboBox_Sach.Text = DataGridView_PhieuMonCT.Rows[e.RowIndex].Cells["sachPMCT"].Value.ToString().Trim();
                ComboBox_PhieuMuon.Text = DataGridView_PhieuMonCT.Rows[e.RowIndex].Cells["maPhieuMuonPMCT"].Value.ToString().Trim();
            }
        }

        private void Button_Xoa_Click(object sender, EventArgs e)
        {
            if (DataGridView_PhieuMuon.CurrentRow != null)
            {
                DialogResult dialogResult = MessageBox.Show(
                    "Bạn có chắc chắn muốn XÓA/HỦY phiếu mượn này không?",
                    "Xác nhận Xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
                int maPM = Convert.ToInt32(DataGridView_PhieuMuon.CurrentRow.Cells["maPhieuMuonPM"].Value);

                if (_PhieuMuonBLL.XoaPhieuMuon(maPM))
                {
                    LoadData();
                    ClearFormPhieuMuon();
                    LoadComBoBox();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một phiếu mượn để xóa!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DataGridView_PhieuMuon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DateTimePicker_ThoiGianMuon.ValueChanged -= DateTimePicker_ThoiGianMuon_ValueChanged;
                var selectedRow = DataGridView_PhieuMuon.Rows[e.RowIndex];

                TextBox_MaPhieuMuon.Text = DataGridView_PhieuMuon.Rows[e.RowIndex].Cells["maPhieuMuonPM"].Value.ToString().Trim();
                string thoiGianMuonStr = selectedRow.Cells["thoiGianMuonPM"].Value?.ToString().Trim();
                string thoiHanTraStr = selectedRow.Cells["thoiHanTraPM"].Value?.ToString().Trim();
                if (DateTime.TryParse(thoiGianMuonStr, out DateTime thoiGianMuon))
                {
                    DateTimePicker_ThoiGianMuon.Value = thoiGianMuon;
                }
                if (DateTime.TryParse(thoiHanTraStr, out DateTime thoiHanTra))
                {
                    DateTimePicker_ThoiHanTra.Value = thoiHanTra;
                }
                TextBox_SoLuong.Text = DataGridView_PhieuMuon.Rows[e.RowIndex].Cells["soLuongPM"].Value.ToString().Trim();
                var cellValue = DataGridView_PhieuMuon.Rows[e.RowIndex].Cells["trangThaiPM"].Value;
                string trangThai = cellValue == null ? "" : cellValue.ToString().Trim();
                if (trangThai.ToString() == "Đã trả")
                {
                    radioButton_DaTra.Checked = true;
                }
                else
                {
                    radioButton_ChuaTra.Checked = true;
                }
                ComboBox_NhanVien.Text = DataGridView_PhieuMuon.Rows[e.RowIndex].Cells["nhanVienPM"].Value.ToString().Trim();
                ComboBox_DocGia.Text = DataGridView_PhieuMuon.Rows[e.RowIndex].Cells["docGiaPM"].Value.ToString().Trim();


                DateTimePicker_ThoiGianMuon.ValueChanged += DateTimePicker_ThoiGianMuon_ValueChanged;
            }
        }

        private void Button_XuatFile_Click(object sender, EventArgs e)
        {
            var qlpm = _PhieuMuonBLL.GetListPM();
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Workbook|*.xlsx";
                sfd.Title = "Chọn nơi lưu file";
                sfd.FileName = "DanhSachPhieuMuon.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Danh sách phiếu mượn");
                        worksheet.Cell(1, 1).Value = "STT";
                        worksheet.Cell(1, 2).Value = "Mã phiếu mượn";
                        worksheet.Cell(1, 3).Value = "Thời gian mượn";
                        worksheet.Cell(1, 4).Value = "Thời hạn trả";
                        worksheet.Cell(1, 5).Value = "Số lượng";
                        worksheet.Cell(1, 6).Value = "Trạng thái";
                        worksheet.Cell(1, 7).Value = "nhân viên";
                        worksheet.Cell(1, 8).Value = "Độc giả";

                        int row = 2;
                        foreach (var nv in qlpm)
                        {
                            worksheet.Cell(row, 1).Value = nv.STT;
                            worksheet.Cell(row, 2).Value = nv.MaPhieuMuon;
                            worksheet.Cell(row, 3).Value = nv.ThoiGianMuon.HasValue
                            ? nv.ThoiGianMuon.Value.ToString("dd/MM/yyyy")
                            : "";
                            worksheet.Cell(row, 4).Value = nv.ThoihanTra.HasValue
                            ? nv.ThoihanTra.Value.ToString("dd/MM/yyyy")
                            : "";
                            worksheet.Cell(row, 5).Value = nv.SoLuong;
                            worksheet.Cell(row, 6).Value = nv.TrangThai;
                            worksheet.Cell(row, 7).Value = nv.NhanVien;
                            worksheet.Cell(row, 8).Value = nv.DocGia;
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
                return;
            }
            List<QuanLyPhieuMuon_DTO> ketQua = _PhieuMuonBLL.TimKiemPM(keyword);
            if (ketQua.Any())
            {
                DataGridView_PhieuMuon.DataSource = ketQua;
                LoadData();
            }
            else
            {
                MessageBox.Show("Không tìm thấy phiếu mượn với từ khóa đã nhập!", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void Button_XoaPMCT_Click(object sender, EventArgs e)
        {
            if (DataGridView_PhieuMonCT.CurrentRow == null)
            {
                MessageBox.Show(
                    "Vui lòng chọn một chi tiết phiếu mượn cần xóa!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }
            if (!int.TryParse(DataGridView_PhieuMonCT.CurrentRow.Cells["maChiTietPhieuMuonPMCT"].Value?.ToString(), out int maCTPM))
            {
                MessageBox.Show(
                    "Không tìm thấy Mã Chi tiết Phiếu Mượn!",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            var confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa chi tiết phiếu mượn này? \nThao tác này sẽ cộng lại số lượng sách vào tồn kho và trừ số lượng nợ của Phiếu Mượn cha.",
                "Xác nhận Xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.No)
                return;
            bool isSuccess = _PhieuMuonCTBLL.XoaCTPM(maCTPM);
            if (!string.IsNullOrEmpty(_PhieuMuonCTBLL.LastError))
            {
                MessageBox.Show(
                    _PhieuMuonCTBLL.LastError,
                    "Lỗi Xóa Chi Tiết",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            else if (isSuccess)
            {
                LoadData();
                LoadComBoBox();

                MessageBox.Show(
                    "Xóa chi tiết phiếu mượn thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                MessageBox.Show(
                    "Xóa chi tiết phiếu mượn thất bại do lỗi không xác định.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void Button_TimKiemPMCT_Click(object sender, EventArgs e)
        {
            string tuKhoa = TextBox_TimKiemPMCT.Text.Trim();
            if (string.IsNullOrEmpty(tuKhoa))
            {
                MessageBox.Show(
                    "Vui lòng nhập từ khóa để tìm kiếm Chi tiết Phiếu Mượn!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                LoadData();

                return;
            }
            List<QuanLyPhieuMuonChiTiet_DTO> danhSachKetQua = _PhieuMuonCTBLL.TimKiemPMCT(tuKhoa);
            if (danhSachKetQua.Any())
            {
                DataGridView_PhieuMonCT.DataSource = danhSachKetQua;
            }
            else
            {
                MessageBox.Show(
                    "Không tìm thấy Chi tiết Phiếu Mượn nào với từ khóa đã nhập!",
                    "Kết quả tìm kiếm",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }
        }

        private void Button_XuatFilePhieuTRa_Click(object sender, EventArgs e)
        {
            var qlpt = _PhieuTraBLL.GetListPhieuTra();
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Workbook|*.xlsx";
                sfd.Title = "Chọn nơi lưu file";
                sfd.FileName = "DanhSachPhieuTra.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Danh sách phiếu trả");
                        worksheet.Cell(1, 1).Value = "STT";
                        worksheet.Cell(1, 2).Value = "Mã Phiếu Mượn";
                        worksheet.Cell(1, 3).Value = "Thời Gian Mượn";
                        worksheet.Cell(1, 4).Value = "Thời Hạn Trả";
                        worksheet.Cell(1, 5).Value = "Số Lượng";
                        worksheet.Cell(1, 6).Value = "Trạng Thái";
                        worksheet.Cell(1, 7).Value = "Sách";
                        worksheet.Cell(1, 8).Value = "Số Lượng Mượn";
                        worksheet.Cell(1, 9).Value = "Tình Trạng Sách";
                        worksheet.Cell(1, 10).Value = "Nhân Viên";
                        worksheet.Cell(1, 11).Value = "Độc Giả";

                        int row = 2;
                        foreach (var pt in qlpt)
                        {
                            worksheet.Cell(row, 1).Value = pt.STT;
                            worksheet.Cell(row, 2).Value = pt.MaPhieuMuon;
                            worksheet.Cell(row, 3).Value = pt.ThoiGianMuon.HasValue
                                ? pt.ThoiGianMuon.Value.ToString("dd/MM/yyyy")
                                : "";
                            worksheet.Cell(row, 4).Value = pt.ThoihanTra.HasValue
                                ? pt.ThoihanTra.Value.ToString("dd/MM/yyyy")
                                : "";
                            worksheet.Cell(row, 5).Value = pt.SoLuong;
                            worksheet.Cell(row, 6).Value = pt.TrangThai ?? "";
                            worksheet.Cell(row, 7).Value = pt.Sach ?? "";
                            worksheet.Cell(row, 8).Value = pt.SoLuongMuon;
                            worksheet.Cell(row, 9).Value = pt.TinhTrangSach ?? "";
                            worksheet.Cell(row, 10).Value = pt.NhanVien ?? "";
                            worksheet.Cell(row, 11).Value = pt.DocGia ?? "";
                            row++;
                        }
                        workbook.SaveAs(sfd.FileName);
                        MessageBox.Show("Xuất file thành công ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void Button_TimKiemPhieuTra_Click(object sender, EventArgs e)
        {
            string tuKhoa = TextBox_TimKiemPhieuTra.Text.Trim();
            if (string.IsNullOrEmpty(tuKhoa))
            {
                MessageBox.Show(
                    "Vui lòng nhập từ khóa để tìm kiếm Phiếu Trả!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadData();
                return;
            }
            List<QuanLyPhieuTra_DTO> danhSachKetQua = _PhieuTraBLL.TimKiemPT(tuKhoa);
            if (danhSachKetQua.Any())
            {
                DataGridView_PhieuTra.DataSource = danhSachKetQua;
            }
            else
            {
                MessageBox.Show(
                    "Không tìm thấy Phiếu Trả nào với từ khóa đã nhập!",
                    "Kết quả tìm kiếm",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
        }

    }
}
