using AForge.Video;
using AForge.Video.DirectShow;
using ClosedXML.Excel;
using QRCoder;
using QuanLyThuVienNhom3.BLL;
using QuanLyThuVienNhom3.BLL;
using QuanLyThuVienNhom3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace QuanLyThuVienNhom3.GUI.UC
{
    public partial class UserControl_QuanLyDocGia : UserControl
    {
        private readonly QuanLyDocGia_BLL _quanLyDocGia_BLL = new QuanLyDocGia_BLL();
        public UserControl_QuanLyDocGia()
        {
            InitializeComponent();
            KhoiTaoComboBoxLocDocGia();
            LoadDaTa();
        }
        private void KhoiTaoComboBoxLocDocGia()
        {
            ComBox_LocNhanVienTheoChuCai.Items.Clear();
            ComBox_LocNhanVienTheoChuCai.Items.Add("Tất cả");
            ComBox_LocNhanVienTheoChuCai.Items.Add("Hoạt Động");
            ComBox_LocNhanVienTheoChuCai.Items.Add("Đã Nghỉ");
            ComBox_LocNhanVienTheoChuCai.SelectedIndex = 0;
        }
        public void ClearFormThongTin()
        {
            TextBox_ThongTinMaDocGia.Clear();
            TextBox_ThongTinDiaChi.Clear();
            TextBox_ThongTinEmail.Clear();
            TextBox_ThongTinNgaySinh.Clear();
            TextBox_ThongTinSDT.Clear();
            TextBox_ThongTinTenDocGia.Clear();
            radioButton_ThongTinGioiTinhNam.Checked = false;
            radioButton_ThongTinGioiTinhNu.Checked = false;
            pictureBox_ThongTinAnhDG.Image = null;
        }
        public void ClearGroupBox()
        {
            TextBox_MaDocGia.Clear();
            TextBox_Ten.Clear();
            TextBox_SoDienThoai.Clear();
            TextBox_Email.Clear();
            TextBox_DiaChi.Clear();
            radioButton_Nam.Checked = false;
            radioButton_Nu.Checked = false;
            checkBox_HoatDong.Checked = false;
            checkBox_DaNghi.Checked = false;
            pictureBox_HinhAnhDocGia.Image = null;
            HinhAnhDG = null;
            pictureBox_MaQR.Image = null;
        }
        public void GenerateQRCode(string qrText)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            pictureBox_MaQR.Image = qrCodeImage;
        }
        public void LoadDaTa()
        {
            DataGridView_DachSachDocGia.DataSource = _quanLyDocGia_BLL.GetListDocGia();

        }
        bool isAdding = false;
        private byte[] HinhAnhDG = null;
        FilterInfoCollection videoDevices;
        VideoCaptureDevice videoSource;
        private void Button_Them_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinDocGia.Visible = true;
            isAdding = true;
            checkBox_DaNghi.Enabled = false;
            ClearGroupBox();
        }

        private void Button_CapNhap_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_MaDocGia.Text))
            {
                MessageBox.Show("Vui lòng chọn độc giả cần cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            checkBox_DaNghi.Enabled = true;
            GroupBox_NhapThongTinDocGia.Visible = true;
            isAdding = false;
        }

        private void Button_Huy_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinDocGia.Visible = false;
        }

        private void Button_QuetQRThongTinDocGia_Click(object sender, EventArgs e)
        {
            GroupBox_ThongTinChiTietDocGia.Visible = true;
        }
        private void Button_HuyThongTin_Click(object sender, EventArgs e)
        {
            GroupBox_ThongTinChiTietDocGia.Visible = false;
            ClearFormThongTin();
        }




        private void Button_ChonAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox_HinhAnhDocGia.Image = Image.FromFile(ofd.FileName);
                pictureBox_HinhAnhDocGia.SizeMode = PictureBoxSizeMode.StretchImage;

                using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        HinhAnhDG = br.ReadBytes((int)fs.Length);
                    }
                }
            }
        }


        private void Button_LuuMa_Click(object sender, EventArgs e)
        {
            if (pictureBox_MaQR.Image != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PNG Image|*.png|JPEG Image|*.jpg";
                sfd.FileName = "DocGia.png";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox_MaQR.Image.Save(sfd.FileName);
                    MessageBox.Show("Đã lưu QR thành công!");
                }
            }
            else
            {
                MessageBox.Show("Chưa có QR để lưu!");
            }
        }
        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();
            pictureBox_Camera.Image = frame;
        }
        private void Button_QuetMaQR_Click(object sender, EventArgs e)
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame;
                videoSource.Start();
                timer_QuetQR.Start();
            }
            else
            {
                MessageBox.Show("Không tìm thấy thiết bị camera!");
            }
        }

        private void timer_QuetQR_Tick(object sender, EventArgs e)
        {
            if (pictureBox_Camera.Image != null)
            {

                var reader = new BarcodeReader();
                var result = reader.Decode(new Bitmap(pictureBox_Camera.Image));

                if (result != null)
                {
                    GroupBox_ThongTinChiTietDocGia.Visible = true;
                    string qrText = result.Text;

                    string[] parts = qrText.Split('|');
                    if (parts.Length >= 8)
                    {
                        string mDGia = parts[0];

                        var docGia = _quanLyDocGia_BLL.GetDocGiaByMa(mDGia);

                        if (docGia == null)
                        {
                            MessageBox.Show("Không tìm thấy độc giả trong hệ thống!",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        string trangThaiTuDB = _quanLyDocGia_BLL.GetTrangThaiDocGia(mDGia);
                        if (!string.IsNullOrEmpty(trangThaiTuDB))
                        {
                            if (trangThaiTuDB == "Hoạt Động")
                            {
                                TextBox_ThongTinMaDocGia.Text = parts[0];
                                var DocGiaDG = _quanLyDocGia_BLL.GetDocGiaByMa(mDGia);
                                if (!DateOnly.TryParseExact(
                                parts[2],
                                "dd/MM/yyyy",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out DateOnly ngaySinhQR))
                                {
                                    MessageBox.Show(
                                        "Ngày sinh trong QR không hợp lệ!",
                                        "Lỗi",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                                    return;
                                }
                                if (DocGiaDG.NgaySinh != ngaySinhQR)
                                {
                                    MessageBox.Show(
                                        "Ngày sinh trong QR không khớp với dữ liệu hệ thống!",
                                        "Cảnh báo",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                    return;
                                }
                                bool khongKhop =
                                !string.Equals(DocGiaDG.TenDocGia?.Trim(), parts[1]?.Trim(), StringComparison.OrdinalIgnoreCase)
                                || !string.Equals(DocGiaDG.GioiTinh?.Trim(), parts[3]?.Trim(), StringComparison.OrdinalIgnoreCase)
                                || !string.Equals(DocGiaDG.DiaChi?.Trim(), parts[4]?.Trim(), StringComparison.OrdinalIgnoreCase)
                                || !string.Equals(DocGiaDG.SoDienThoai?.Trim(), parts[5]?.Trim(), StringComparison.OrdinalIgnoreCase)
                                || !string.Equals(DocGiaDG.Email?.Trim(), parts[6]?.Trim(), StringComparison.OrdinalIgnoreCase);

                                if (khongKhop)
                                {
                                    MessageBox.Show(
                                        "Thông tin trong QR không trùng khớp với dữ liệu hệ thống!",
                                        "Cảnh báo",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                    return;
                                }
                                TextBox_ThongTinTenDocGia.Text = parts[1];
                                TextBox_ThongTinNgaySinh.Text = parts[2];
                                if (parts[3] == "Nữ")
                                {
                                    radioButton_ThongTinGioiTinhNu.Checked = true;
                                }
                                else
                                {
                                    radioButton_ThongTinGioiTinhNam.Checked = true;
                                }
                                TextBox_ThongTinDiaChi.Text = parts[4];
                                TextBox_ThongTinSDT.Text = parts[5];
                                TextBox_ThongTinEmail.Text = parts[6];

                                byte[] hinhAnhBytes = _quanLyDocGia_BLL.GetHinhAnhDocGia(mDGia);
                                HienThiHinhAnhDocGia(hinhAnhBytes);
                            }
                            else
                            {
                                MessageBox.Show("Độc giả đã ngừng hoạt động!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy độc giả có mã này trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("QR không đúng định dạng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
        private void HienThiHinhAnhDocGia(byte[] hinhAnhTuDB)
        {
            if (hinhAnhTuDB != null && hinhAnhTuDB.Length > 0)
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream(hinhAnhTuDB))
                    {
                        Image hinhAnh = Image.FromStream(ms);
                        pictureBox_ThongTinAnhDG.Image = hinhAnh;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi hiển thị hình ảnh: " + ex.Message);
                    pictureBox_ThongTinAnhDG.Image = null;
                }
            }
            else
            {
                pictureBox_ThongTinAnhDG.Image = null;
            }
        }

        private void DataGridView_DachSachDocGia_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                int MaDG = (int)DataGridView_DachSachDocGia.Rows[e.RowIndex].Cells["ID"].Value;

                using (var _db = new ThuVienNhom3Context())
                {
                    var nv = _db.DocGia.FirstOrDefault(n => n.MaDocGia == MaDG);

                    if (nv.HinhAnh != null && nv.HinhAnh.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(nv.HinhAnh))
                        {
                            pictureBox_HinhAnhDocGia.Image = Image.FromStream(ms);
                            pictureBox_HinhAnhDocGia.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                    }
                    else
                    {
                        pictureBox_HinhAnhDocGia.Image = null;
                    }
                }
                TextBox_MaDocGia.Text = DataGridView_DachSachDocGia.Rows[e.RowIndex].Cells["ID"].Value.ToString().Trim();
                TextBox_Ten.Text = DataGridView_DachSachDocGia.Rows[e.RowIndex].Cells["Ten"].Value.ToString().Trim();
                DateTimePicker_NgaySinh.Text = DataGridView_DachSachDocGia.Rows[e.RowIndex].Cells["NgayS"].Value.ToString();
                string gioiTinh = DataGridView_DachSachDocGia.Rows[e.RowIndex].Cells["GioiT"].Value.ToString().Trim();
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
                else
                {
                    radioButton_Nam.Checked = false;
                    radioButton_Nu.Checked = false;
                }
                TextBox_DiaChi.Text = DataGridView_DachSachDocGia.Rows[e.RowIndex].Cells["DiaC"].Value.ToString().Trim();
                TextBox_SoDienThoai.Text = DataGridView_DachSachDocGia.Rows[e.RowIndex].Cells["Sdt"].Value.ToString().Trim();
                TextBox_Email.Text = DataGridView_DachSachDocGia.Rows[e.RowIndex].Cells["email"].Value.ToString().Trim();
                string TrangT = DataGridView_DachSachDocGia.Rows[e.RowIndex].Cells["TrangT"].Value.ToString().Trim();
                if (TrangT == "Hoạt Động")
                {
                    checkBox_HoatDong.Checked = true;
                    checkBox_DaNghi.Checked = false;
                }
                else if (TrangT == "Đã Nghỉ")
                {
                    checkBox_HoatDong.Checked = false;
                    checkBox_DaNghi.Checked = true;
                }
                else
                {
                    checkBox_HoatDong.Checked = false;
                    checkBox_DaNghi.Checked = false;
                }
            }
        }

        private void Button_Luu_Click(object sender, EventArgs e)
        {

            if (isAdding == true)
            {
                if (string.IsNullOrEmpty(TextBox_Ten.Text)
                    || string.IsNullOrEmpty(TextBox_SoDienThoai.Text)
                    || string.IsNullOrEmpty(TextBox_Email.Text)
                    || !radioButton_Nam.Checked && !radioButton_Nu.Checked
                    || string.IsNullOrEmpty(TextBox_DiaChi.Text)
                    || !checkBox_HoatDong.Checked && !checkBox_DaNghi.Checked)
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin bắt buộc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string TranThai = (checkBox_HoatDong.Checked) ? "Hoạt động" : "Không hoạt động";
                string GioiTinh = (radioButton_Nam.Checked) ? "Nam" : "Nữ";
                string ten = TextBox_Ten.Text.Trim();
                string DiaChi = TextBox_DiaChi.Text.Trim();
                string SDT = TextBox_SoDienThoai.Text.Trim();
                string email = TextBox_Email.Text.Trim();
                string ThongTinNgaySinh = DateTimePicker_NgaySinh.Value.ToString("dd/MM/yyyy");
                if (ten.Length < 3 || ten.Length > 50)
                {
                    MessageBox.Show("Tên độc giả phải từ 3 đến 50 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (pictureBox_HinhAnhDocGia.Image == null)
                {
                    MessageBox.Show("Vui lòng chọn hình ảnh độc giả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (SDT.Length != 10 || !SDT.StartsWith("0") || !SDT.All(char.IsDigit))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ! Vui lòng nhập lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!email.EndsWith("@gmail.com") || !email.Contains("@") || email.StartsWith("@"))
                {
                    MessageBox.Show("Email không hợp lệ! Vui lòng nhập lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                DateTime NgaySinh = DateTimePicker_NgaySinh.Value;
                int tuoi = DateTime.Now.Year - NgaySinh.Year;
                if (NgaySinh > DateTime.Now.AddYears(-tuoi)) tuoi--;

                if (tuoi < 10)
                {
                    MessageBox.Show("Bạn chưa đủ tuổi mượn sách !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (DiaChi.Length < 3 || DiaChi.Length > 50)
                {
                    MessageBox.Show("Địa chỉ phải từ 3 đến 50 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (checkBox_HoatDong.Checked == true && checkBox_DaNghi.Checked == true)
                {
                    MessageBox.Show("Trạng thái không hợp lệ! Vui lòng chọn lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn thêm độc giả này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No)
                    return;
                if (dialogResult == DialogResult.Yes)
                {
                    DocGium docGia = new DocGium()
                    {
                        TenDocGia = TextBox_Ten.Text.Trim(),
                        NgaySinh = DateOnly.FromDateTime(DateTimePicker_NgaySinh.Value),
                        SoDienThoai = TextBox_SoDienThoai.Text.Trim(),
                        Email = TextBox_Email.Text.Trim(),
                        DiaChi = TextBox_DiaChi.Text.Trim(),
                        GioiTinh = radioButton_Nam.Checked ? "Nam" : "Nữ",
                        TrangThai = checkBox_HoatDong.Checked ? "Hoạt Động" : "Đã Nghỉ",
                        HinhAnh = HinhAnhDG
                    };
                    if (_quanLyDocGia_BLL.ThemDocGia(docGia))
                    {
                        string maDocGia = _quanLyDocGia_BLL.ThemMaDoGia().ToString();
                        LoadDaTa();
                        ClearGroupBox();
                        string chuoiThongTin =
                        $"{maDocGia}|{ten}|{ThongTinNgaySinh}|{GioiTinh}|{DiaChi}|{SDT}|{email}|{TranThai}";
                        if (!string.IsNullOrEmpty(chuoiThongTin))
                        {
                            GenerateQRCode(chuoiThongTin);
                        }
                        else
                        {
                            MessageBox.Show("Vui lòng nhập nội dung để tạo QR!");
                        }
                        if (pictureBox_MaQR.Image != null)
                        {
                            SaveFileDialog sfd = new SaveFileDialog();
                            sfd.Filter = "PNG Image|*.png|JPEG Image|*.jpg";
                            sfd.FileName = $"{maDocGia},{ten}.png";

                            if (sfd.ShowDialog() == DialogResult.OK)
                            {
                                pictureBox_MaQR.Image.Save(sfd.FileName);
                                MessageBox.Show("Đã lưu QR thành công!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Chưa có QR để lưu!");
                        }
                    }
                }
            }
            else
            {
                if (TextBox_MaDocGia.Text == null)
                {
                    MessageBox.Show("Vui lòng chọn độc giả cần cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (string.IsNullOrEmpty(TextBox_Ten.Text)
                    || string.IsNullOrEmpty(TextBox_SoDienThoai.Text)
                    || string.IsNullOrEmpty(TextBox_Email.Text)
                    || !radioButton_Nam.Checked && !radioButton_Nu.Checked
                    || string.IsNullOrEmpty(TextBox_DiaChi.Text)
                    || !checkBox_HoatDong.Checked && !checkBox_DaNghi.Checked)
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin bắt buộc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string TranThai = (checkBox_HoatDong.Checked) ? "Hoạt động" : "Không hoạt động";
                string GioiTinh = (radioButton_Nam.Checked) ? "Nam" : "Nữ";
                string ten = TextBox_Ten.Text.Trim();
                string DiaChi = TextBox_DiaChi.Text.Trim();
                string SDT = TextBox_SoDienThoai.Text.Trim();
                string email = TextBox_Email.Text.Trim();
                string ThongTinNgaySinh = DateTimePicker_NgaySinh.Value.ToString("dd/MM/yyyy");
                if (ten.Length < 3 || ten.Length > 50)
                {
                    MessageBox.Show("Tên độc giả phải từ 3 đến 50 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (pictureBox_HinhAnhDocGia.Image == null)
                {
                    MessageBox.Show("Vui lòng chọn hình ảnh độc giả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (SDT.Length != 10 || !SDT.StartsWith("0") || !SDT.All(char.IsDigit))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ! Vui lòng nhập lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!email.EndsWith("@gmail.com") || !email.Contains("@") || email.StartsWith("@"))
                {
                    MessageBox.Show("Email không hợp lệ! Vui lòng nhập lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                DateTime NgaySinh = DateTimePicker_NgaySinh.Value;
                int tuoi = DateTime.Now.Year - NgaySinh.Year;
                if (NgaySinh > DateTime.Now.AddYears(-tuoi)) tuoi--;

                if (tuoi < 10)
                {
                    MessageBox.Show("Bạn chưa đủ tuổi mượn sách !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (DiaChi.Length < 3 || DiaChi.Length > 50)
                {
                    MessageBox.Show("Địa chỉ phải từ 3 đến 50 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (checkBox_HoatDong.Checked == true && checkBox_DaNghi.Checked == true)
                {
                    MessageBox.Show("Trạng thái không hợp lệ! Vui lòng chọn lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn cập nhật độc giả này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No)
                    return;
                if (dialogResult == DialogResult.Yes)
                {
                    DocGium docGia = new DocGium()
                    {
                        MaDocGia = int.Parse(TextBox_MaDocGia.Text.Trim()),
                        TenDocGia = TextBox_Ten.Text.Trim(),
                        NgaySinh = DateOnly.FromDateTime(DateTimePicker_NgaySinh.Value),
                        SoDienThoai = TextBox_SoDienThoai.Text.Trim(),
                        Email = TextBox_Email.Text.Trim(),
                        DiaChi = TextBox_DiaChi.Text.Trim(),
                        GioiTinh = radioButton_Nam.Checked ? "Nam" : "Nữ",
                        TrangThai = checkBox_HoatDong.Checked ? "Hoạt Động" : "Đã Nghỉ",
                        HinhAnh = HinhAnhDG
                    };
                    _quanLyDocGia_BLL.SuaDocGia(docGia);
                    LoadDaTa();
                    ClearGroupBox();
                    GroupBox_NhapThongTinDocGia.Visible = false;
                }

            }
        }

        private void Button_ChonAnh_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox_HinhAnhDocGia.Image = Image.FromFile(ofd.FileName);
                pictureBox_HinhAnhDocGia.SizeMode = PictureBoxSizeMode.StretchImage;

                using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        HinhAnhDG = br.ReadBytes((int)fs.Length);
                    }
                }
            }
        }

        private void Button_QuetMaQR_Click_1(object sender, EventArgs e)
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame;
                videoSource.Start();
                timer_QuetQR.Start();
            }
            else
            {
                MessageBox.Show("Không tìm thấy thiết bị camera!");
            }
        }

        private void DataGridView_DachSachDocGia_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Button_Xoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBox_MaDocGia.Text))
            {
                MessageBox.Show(
                    "Vui lòng chọn độc giả trước khi xóa!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }
            int maDocGia = Convert.ToInt32(TextBox_MaDocGia.Text);
            DialogResult confirmResult = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa độc giả này không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmResult == DialogResult.Yes)
            {
                int ketQua = _quanLyDocGia_BLL.XoaDocGia(maDocGia);

                if (ketQua == -1)
                {
                    MessageBox.Show(
                        "Độc giả này vẫn còn phiếu mượn chưa trả, không thể xóa!",
                        "Cảnh báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
                else if (ketQua == 0)
                {
                    MessageBox.Show(
                        "Xóa thất bại do lỗi cơ sở dữ liệu!",
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
                else
                {
                    MessageBox.Show(
                        "Xóa thành công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    LoadDaTa();
                }
            }
        }

        private void ComBox_LocNhanVienTheoChuCai_SelectedIndexChanged(object sender, EventArgs e)
        {
            var allDocGia = _quanLyDocGia_BLL.GetListDocGia();
            string selectedTrangThai = ComBox_LocNhanVienTheoChuCai.SelectedItem.ToString();

            if (selectedTrangThai == "Tất cả")
            {
                DataGridView_DachSachDocGia.DataSource = allDocGia;
            }
            else
            {
                DataGridView_DachSachDocGia.DataSource = allDocGia
                    .Where(dg => dg.TrangThai == selectedTrangThai)
                    .ToList();
            }
        }
        private void Button_TimKiem_Click(object sender, EventArgs e)
        {
            string keyword = TextBox_TimKiem.Text.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                MessageBox.Show(
                    "Vui lòng nhập từ khóa để tìm kiếm!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            var allDocGia = _quanLyDocGia_BLL.GetListDocGia();

            var filtered = allDocGia.Where(dg => (!string.IsNullOrEmpty(dg.TenDocGia) &&
                                                 dg.TenDocGia.ToLower().Contains(keyword)) ||
                                                (!string.IsNullOrEmpty(dg.SoDienThoai) &&
                                                 dg.SoDienThoai.ToLower().Contains(keyword)) ||
                                                (!string.IsNullOrEmpty(dg.Email) &&
                                                 dg.Email.ToLower().Contains(keyword))
                                                ).ToList();

            DataGridView_DachSachDocGia.DataSource = filtered;
        }

        private void TextBox_TimKiem_TextChanged(object sender, EventArgs e)
        {
            Button_TimKiem_Click(sender, e);
        }

        private void Button_XuatFile_Click(object sender, EventArgs e)
        {
            var qldg = _quanLyDocGia_BLL.GetListDocGia();
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Workbook|*.xlsx";
                sfd.Title = "Chọn nơi lưu file";
                sfd.FileName = "DanhSachDocGia.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Danh sách độc giả");
                        worksheet.Cell(1, 1).Value = "STT";
                        worksheet.Cell(1, 2).Value = "Mã độc giả";
                        worksheet.Cell(1, 3).Value = "Tên độc giả";
                        worksheet.Cell(1, 4).Value = "Ngày sinh";
                        worksheet.Cell(1, 5).Value = "Giới tính";
                        worksheet.Cell(1, 6).Value = "Địa chỉ";
                        worksheet.Cell(1, 7).Value = "Số điện thoại";
                        worksheet.Cell(1, 8).Value = "Email";
                        worksheet.Cell(1, 9).Value = "Trạng thái";

                        int row = 2;
                        foreach (var dg in qldg)
                        {
                            worksheet.Cell(row, 1).Value = dg.STT;
                            worksheet.Cell(row, 2).Value = dg.MaDocGia;
                            worksheet.Cell(row, 3).Value = dg.TenDocGia;
                            worksheet.Cell(row, 4).Value = dg.NgaySinh.HasValue
                            ? dg.NgaySinh.Value.ToString("dd/MM/yyyy")
                            : "";
                            worksheet.Cell(row, 5).Value = dg.GioiTinh;
                            worksheet.Cell(row, 6).Value = dg.DiaChi;
                            worksheet.Cell(row, 7).Value = dg.SoDienThoai;
                            worksheet.Cell(row, 8).Value = dg.Email;
                            worksheet.Cell(row, 9).Value = dg.TrangThai;
                            row++;
                        }
                        workbook.SaveAs(sfd.FileName);
                        MessageBox.Show("Xuất file thành công ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void Button_LuuMa_Click_1(object sender, EventArgs e)
        {
            if (!isAdding)
            {
                string TranThai = (checkBox_HoatDong.Checked) ? "Hoạt động" : "Không hoạt động";
                string GioiTinh = (radioButton_Nam.Checked) ? "Nam" : "Nữ";
                string ten = TextBox_Ten.Text.Trim();
                string DiaChi = TextBox_DiaChi.Text.Trim();
                string SDT = TextBox_SoDienThoai.Text.Trim();
                string email = TextBox_Email.Text.Trim();
                string ThongTinNgaySinh = DateTimePicker_NgaySinh.Value.ToString("dd/MM/yyyy");
                int maDocGia = int.Parse(TextBox_MaDocGia.Text.Trim());
                string chuoiThongTin =
                    $"{maDocGia}|{ten}|{ThongTinNgaySinh}|{GioiTinh}|{DiaChi}|{SDT}|{email}|{TranThai}";
                if (!string.IsNullOrEmpty(chuoiThongTin))
                {
                    GenerateQRCode(chuoiThongTin);
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập nội dung để tạo QR!");
                }
                if (pictureBox_MaQR.Image != null)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "PNG Image|*.png|JPEG Image|*.jpg";
                    sfd.FileName = $"{maDocGia},{ten}.png";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        pictureBox_MaQR.Image.Save(sfd.FileName);
                        MessageBox.Show("Đã lưu QR thành công!");
                    }
                }
                else
                {
                    MessageBox.Show("Chưa có QR để lưu!");
                }
            }
        }
    }
}