using AForge.Video;
using AForge.Video.DirectShow;
using QRCoder;
using QuanLyThuVienNhom3.BLL;
using QuanLyThuVienNhom3.BLL;
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
            ClearGroupBox();
        }

        private void Button_CapNhap_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_MaDocGia.Text))
            {
                MessageBox.Show("Vui lòng chọn độc giả cần cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
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
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice); // ✅ đúng chỗ gán
            if (videoDevices.Count > 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame;
                videoSource.Start();
                timer_QuetQR.Start(); // Timer dùng để quét QR định kỳ
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
                    if (parts.Length >= 6)
                    {

                        if (parts[7] == "Hoạt động")
                        {
                            TextBox_ThongTinMaDocGia.Text = parts[0];
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
                        }
                        else
                        {
                            MessageBox.Show("Độc giả đã ngừng hoạt động!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("QR không đúng định dạng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
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
                string maDocGia = _quanLyDocGia_BLL.ThemMaDoGia().ToString();
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
                if (NgaySinh > DateTime.Now.AddYears(-tuoi)) tuoi--; // Điều chỉnh nếu chưa đến ngày sinh trong năm nay

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
                string maDocGia = _quanLyDocGia_BLL.ThemMaDoGia().ToString();
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
                if (NgaySinh > DateTime.Now.AddYears(-tuoi)) tuoi--; // Điều chỉnh nếu chưa đến ngày sinh trong năm nay

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
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice); // ✅ đúng chỗ gán
            if (videoDevices.Count > 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame;
                videoSource.Start();
                timer_QuetQR.Start(); // Timer dùng để quét QR định kỳ
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
            // Lấy mã độc giả từ TextBox
            int maDocGia = Convert.ToInt32(TextBox_MaDocGia.Text);

            // Hiển thị hộp thoại xác nhận
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
                    LoadDaTa(); // Reload DataGridView
                }
            }
            // Nếu người dùng chọn No thì không làm gì cả
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
            var allDocGia = _quanLyDocGia_BLL.GetListDocGia();

            var filtered = allDocGia
                .Where(dg => !string.IsNullOrEmpty(dg.TenDocGia) && dg.TenDocGia.ToLower().Contains(keyword))
                .ToList();

            DataGridView_DachSachDocGia.DataSource = filtered;
        }

        private void TextBox_TimKiem_TextChanged(object sender, EventArgs e)
        {
            Button_TimKiem_Click(sender, e);
        }
        private void ExportDataGridViewToCSV(DataGridView dgv, string filePath)
        {
            var sb = new StringBuilder();

            // Write column headers
            var headers = dgv.Columns.Cast<DataGridViewColumn>();
            sb.AppendLine(string.Join(",", headers.Select(column => $"\"{column.HeaderText}\"")));

            // Write rows
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (!row.IsNewRow)
                {
                    var cells = row.Cells.Cast<DataGridViewCell>();
                    sb.AppendLine(string.Join(",", cells.Select(cell => $"\"{cell.Value?.ToString()?.Replace("\"", "\"\"")}\"")));
                }
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        private void Button_XuatFile_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV files (*.csv)|*.csv";
                sfd.FileName = "DocGia.csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ExportDataGridViewToCSV(DataGridView_DachSachDocGia, sfd.FileName);
                    MessageBox.Show("Xuất file thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

    }
}