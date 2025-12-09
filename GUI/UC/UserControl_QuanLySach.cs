using Microsoft.EntityFrameworkCore;
using Nhom3ThuVienBanNhap.DTO;
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
    public partial class UserControl_QuanLySach : UserControl
    {
        private bool isUpdating = false;

        // Biến lưu được chọn để cập nhật
        private int selectedSachId = -1;

        private object DanhSachSachGoc;
        public UserControl_QuanLySach()
        {
            InitializeComponent();
            LoadDataToDataGridView();
            LoadComboBoxes();
            LoadLoaiSachToComboBox();
            PhanQuyen();
        }
        public void PhanQuyen()
        {
            if (UserSession.TaiKhoanHienTai.MaVaiTro == 2)
            {
                Button_Xoa.Enabled = false;
                Button_CapNhap.Enabled = false;
            }
        }
        //load
        public void LoadDataToDataGridView()
        {
            using (var dbContext = new ThuVienNhom3Context())
            {
                try
                {
                    var query = dbContext.Saches
                                 .Include(s => s.MaLoaiSachNavigation)
                                 .Include(s => s.MaNhaSanXuatNavigation);
                    var danhSachProjection = query.Select(s => new// Tạo một đối tượng ẩn danh
                    {
                        // Giữ lại ID ẩn để phục vụ chức năng Xóa/Cập nhật
                        MaSach = s.MaSach,

                        // Các cột hiển thị thông tin sách
                        TenSach = s.TenSach,
                        SoLuong = s.SoLuong,
                        TacGia = s.TacGia,
                        TrangThai = s.TrangThai,

                        // >> Lấy TÊN từ thuộc tính Điều hướng <<
                        TenLoaiSach = s.MaLoaiSachNavigation.TenLoaiSach, // Lấy Tên
                        TenNhaSanXuat = s.MaNhaSanXuatNavigation.TenNhaSanXuat,   // Lấy Tên

                        // Giữ lại Mã nếu cần cho mục đích khác (tùy chọn)
                        MaLoaiSach = s.MaLoaiSach,
                        MaNhaSanXuat = s.MaNhaSanXuat

                    }).ToList(); // Thực thi truy vấn
                                 // Gán danh sách đối tượng Sách cho DataSource của DataGridView
                                 // DataGridView sẽ tự động tạo các cột dựa trên thuộc tính của lớp Sach
                                 //DataGridView_DachSachSach.DataSource = danhSachHienThi;
                    DanhSachSachGoc = danhSachProjection;
                    DataGridView_DachSachSach.DataSource = DanhSachSachGoc;
                    // Tùy chỉnh hiển thị cột nếu cần (ví dụ: ẩn ID, đổi tên tiêu đề cột)
                    CustomizeDataGridViewColumns();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void CustomizeDataGridViewColumns()
        {
            //Kiểm tra xem đã có dữ liệu chưa
            var dgv = DataGridView_DachSachSach;
            if (dgv.DataSource != null && dgv.Columns.Count > 0)
            {
                // 1. Ẩn các cột Khóa Chính và Khóa Ngoại
                //if (dgv.Columns.Contains("MaSach"))
                //{
                //    dgv.Columns["MaSach"].Visible = false; // Luôn ẩn ID
                }
                // Ẩn các cột Mã vẫn còn trong danh sách (MaLoaiSach, MaNhaSanXuat)
                if (dgv.Columns.Contains("MaLoaiSach"))
                {
                    dgv.Columns["MaLoaiSach"].Visible = false;
                }
                if (dgv.Columns.Contains("MaNhaSanXuat"))
                {
                    dgv.Columns["MaNhaSanXuat"].Visible = false;
                }

                if (dgv.Columns.Contains("TenSach"))
                    dgv.Columns["TenSach"].HeaderText = "TenSach";

                if (dgv.Columns.Contains("SoLuong"))
                    dgv.Columns["SoLuong"].HeaderText = "SoLuong";

                if (dgv.Columns.Contains("TacGia"))
                    dgv.Columns["TacGia"].HeaderText = "TacGia";
                // 2. Chỉnh sửa Header Text cho các cột mới nếu cần
                if (dgv.Columns.Contains("TenLoaiSachHienThi"))
                {
                    dgv.Columns["TenLoaiSachHienThi"].HeaderText = "TheLoaiSach";
                }
                if (dgv.Columns.Contains("TenNSX"))
                {
                    dgv.Columns["TenNSX"].HeaderText = "NhaSanXuat";
                }
                // ... (Các cột khác nếu cần) ...
                //dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }

        
        // Đặt sự kiện này trong Designer để DataGridView luôn tải lại khi form hiện ra (hoặc dùng sự kiện Load)
        private void QuanLySachForm_Load(object sender, EventArgs e)
        {
            LoadDataToDataGridView();
        }

        //Hết load


        private void Button_ThemSach_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinSach.Visible = true;
            isUpdating = false;
            GroupBox_NhapThongTinSach.Text = "Nhập thông tin sách";
            // 4. Thay đổi tiêu đề Form/Button để hiển thị chế độ cập nhật (tùy chọn)
            Button_Luu.Text = "Lưu";
            ResetFormState();
        }

        private void LoadComboBoxes()
        {
            using (var dbContext = new ThuVienNhom3Context())
            {
                // Tải dữ liệu cho ComboBox Loại Sách
                var loaiSachList = dbContext.LoaiSaches.ToList();
                ComboBox1.DataSource = loaiSachList;
                ComboBox1.DisplayMember = "TenLoaiSach"; // Tên hiển thị
                ComboBox1.ValueMember = "MaLoaiSach"; // Giá trị thực tế

                // Tải dữ liệu cho ComboBox Nhà Xuất Bản
                var nxbList = dbContext.NhaSanXuats.ToList();
                ComboBox2.DataSource = nxbList;
                ComboBox2.DisplayMember = "TenNhaSanXuat"; // Tên hiển thị
                ComboBox2.ValueMember = "MaNhaSanXuat"; // Giá trị thực tế
            }
        }
        private void Button_CapNhap_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinSach.Visible = true;
            isUpdating = true;
            GroupBox_NhapThongTinSach.Text = "Cập nhật thông tin sách";
            // 4. Thay đổi tiêu đề Form/Button để hiển thị chế độ cập nhật (tùy chọn)
            Button_Luu.Text = "Cập nhật";
        }

        private void DataGridView_DanhSachSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            //Bỏ qua nếu click vào tiêu đề cột(RowIndex< 0)
            if (e.RowIndex < 0)
            {
                return;
            }

            try
            {
                // 1. Lấy hàng được click
                DataGridViewRow selectedRow = DataGridView_DachSachSach.Rows[e.RowIndex];
                //// 2. Lấy ID Sách từ cột ID (Giả sử cột ID nằm ở vị trí 0, hoặc có tên là 'SachId')
                //// Lưu ý: Tên cột phải khớp với tên thuộc tính trong Projection (ví dụ: 'SachId')
                if (selectedRow.Cells["MaSach"].Value != null)
                {
                    selectedSachId = Convert.ToInt32(selectedRow.Cells["MaSach"].Value);
                    /*isUpdating = true;*/ // Chuyển sang chế độ cập nhật

                    // 3. Đọc và điền dữ liệu vào các controls

                    TextBox_TenSach.Text = selectedRow.Cells["TenSach"].Value.ToString();
                    guna2TextBox1.Text = selectedRow.Cells["SoLuong"].Value.ToString(); // Hoặc tên cột Số Lượng
                    guna2TextBox2.Text = selectedRow.Cells["TacGia"].Value.ToString(); // Hoặc tên cột Tác Giả

                    // Xử lý RadioButton
                    string trangThai = selectedRow.Cells["TrangThai"].Value.ToString();
                    radioButton_ConSach.Checked = (trangThai == "Còn sách");
                    radioButton_HetSach.Checked = (trangThai == "Hết sách");

                    if (selectedRow.Cells["MaLoaiSach"].Value != null)
                    {
                        ComboBox1.SelectedValue = selectedRow.Cells["MaLoaiSach"].Value;
                    }
                    // Gán Khóa ngoại ID (MaNhaSanXuat) vào SelectedValue của ComboBox 2
                    if (selectedRow.Cells["MaNhaSanXuat"].Value != null)
                    {
                        ComboBox2.SelectedValue = selectedRow.Cells["MaNhaSanXuat"].Value;
                    }
                    //        // Xử lý ComboBox (Quan trọng: Phải gán SelectedValue bằng Khóa ngoại ID)
                    //GroupBox_NhapThongTinSach.Visible = true;
                    //GroupBox_NhapThongTinSach.Text = "Cập nhật thông tin sách";
                    //// 4. Thay đổi tiêu đề Form/Button để hiển thị chế độ cập nhật (tùy chọn)
                    //Button_Luu.Text = "Cập nhật";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Button_Huy_Click(object sender, EventArgs e)
        {
            GroupBox_NhapThongTinSach.Visible = false;
        }

        private void Button_Luu_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            // BƯỚC 2: Phân loại hành động
            if (isUpdating)
            {
                UpdateSach(); // Chế độ CẬP NHẬT
            }
            else
            {
                AddSach(); // Chế độ THÊM MỚI
            }

        }
        private void UpdateSach()
        {
            try
            {
                using (var dbContext = new ThuVienNhom3Context())
                {
                    // 1. Tìm sách cần cập nhật trong CSDL bằng ID
                    var sachCanCapNhat = dbContext.Saches.FirstOrDefault(s => s.MaSach == selectedSachId);

                    if (sachCanCapNhat != null)
                    {
                        // 2. Cập nhật các thuộc tính của đối tượng Sách
                        sachCanCapNhat.TenSach = TextBox_TenSach.Text;
                        sachCanCapNhat.SoLuong = int.Parse(guna2TextBox1.Text);
                        sachCanCapNhat.TacGia = guna2TextBox2.Text;
                        sachCanCapNhat.TrangThai = radioButton_ConSach.Checked ? "Còn sách" : "Hết sách";
                        //if (comboBox1.SelectedValue != null)
                        //{
                        //    sachCanCapNhat.MaLoaiSach = Convert.ToInt32(comboBox1.SelectedValue);
                        //}

                        //if (comboBox2.SelectedValue != null)
                        //{
                        //    sachCanCapNhat.MaNhaSanXuat = Convert.ToInt32(comboBox2.SelectedValue);
                        //}

                        //[TÙY CHỌN] Cập nhật các thuộc tính Tên hiển thị(chỉ khi chúng có [NotMapped] trong lớp Sach)
                        if (ComboBox1.SelectedItem is LoaiSach loaiSachDuocChon)
                        {
                            sachCanCapNhat.TenLoaiSach = loaiSachDuocChon.TenLoaiSach;
                        }
                        if (ComboBox2.SelectedItem is NhaSanXuat nhaSanXuatDuocChon)
                        {
                            sachCanCapNhat.TenNhaSanXuat = nhaSanXuatDuocChon.TenNhaSanXuat;
                        }
                        // ... Cập nhật các trường khác ...

                        // 3. Thực hiện lệnh UPDATE vào CSDL
                        // EF Core sẽ tự động nhận ra các thay đổi và tạo lệnh UPDATE
                        dbContext.SaveChanges();

                        MessageBox.Show("Cập nhật sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 4. Reset trạng thái
                        ResetFormState();
                        LoadDataToDataGridView();
                        ClearInputFields();
                        GroupBox_NhapThongTinSach.Visible = false;
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Số Lượng phải là một số nguyên hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật sách: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AddSach()
        {
            // Cần đảm bảo bạn đã gọi ValidateInput() trước khi gọi hàm này
            // hoặc gọi lại ở đây để đảm bảo an toàn
            string tenLoaiSach = "";
            string tenNhaSanXuat = "";

            // 1. Lấy Khóa Ngoại (ID) và Lấy Tên từ SelectedItem

            // --- Xử lý ComboBox Loại Sách (comboBox1) ---
            int maLoaiSach = 0;
            if (ComboBox1.SelectedValue != null)
            {
                maLoaiSach = Convert.ToInt32(ComboBox1.SelectedValue);

                // >> Lấy Tên từ SelectedItem (Ép kiểu sang lớp mô hình LoaiSach) <<
                if (ComboBox1.SelectedItem is LoaiSach loaiSachDuocChon)
                {
                    tenLoaiSach = loaiSachDuocChon.TenLoaiSach;
                    // Ví dụ: MessageBox.Show($"Loại sách được chọn: {tenLoaiSach}"); 
                }
            }
            // else: Xử lý lỗi nếu SelectedValue là null (nên được xử lý trong ValidateInput hoặc ở đây)

            // --- Xử lý ComboBox Nhà Sản Xuất (comboBox2) ---
            int maNhaSanXuat = 0;
            if (ComboBox2.SelectedValue != null)
            {
                maNhaSanXuat = Convert.ToInt32(ComboBox2.SelectedValue);

                // >> Lấy Tên từ SelectedItem (Ép kiểu sang lớp mô hình NhaSanXuat) <<
                if (ComboBox2.SelectedItem is NhaSanXuat nxbDuocChon)
                {
                    tenNhaSanXuat = nxbDuocChon.TenNhaSanXuat;
                }
            }
            try
            {
                // 1. Tạo đối tượng Sách mới từ dữ liệu nhập
                var newSach = new Sach
                {
                    TenSach = TextBox_TenSach.Text,
                    SoLuong = int.Parse(guna2TextBox1.Text),
                    TacGia = guna2TextBox2.Text,
                    TrangThai = radioButton_ConSach.Checked ? "Còn sách" : "Hết sách",
                    MaLoaiSach = maLoaiSach,
                    MaNhaSanXuat = maNhaSanXuat,
                };

                // 2. Sử dụng Entity Framework Core để thêm vào CSDL
                using (var dbContext = new ThuVienNhom3Context())
                {
                    dbContext.Saches.Add(newSach);
                    dbContext.SaveChanges(); // Thực hiện INSERT
                }

                MessageBox.Show("Thêm sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 3. Reset trạng thái và cập nhật DataGridView
                ResetFormState(); // Đặt lại Form về chế độ Thêm mới
                LoadDataToDataGridView();
            }
            catch (FormatException)
            {
                MessageBox.Show("Số Lượng phải là số nguyên hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Bắt lỗi CSDL hoặc lỗi khác
                MessageBox.Show($"Lỗi khi thêm sách: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool ValidateInput()
        {
            // --- 1. KIỂM TRA TEXTBOX RỖNG (STRING INPUTS) ---

            // Kiểm tra Tên Sách
            if (string.IsNullOrWhiteSpace(TextBox_TenSach.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên Sách.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TextBox_TenSach.Focus();
                return false;
            }

            // Kiểm tra Tác Giả
            if (string.IsNullOrWhiteSpace(guna2TextBox2.Text))
            {
                MessageBox.Show("Vui lòng nhập Tác Giả.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                guna2TextBox2.Focus();
                return false;
            }

            // --- 2. KIỂM TRA SỐ LƯỢNG (INTEGER INPUTS) ---

            // Kiểm tra Số Lượng (Sử dụng TryParse để đảm bảo là số nguyên)
            if (string.IsNullOrWhiteSpace(guna2TextBox1.Text))
            {
                MessageBox.Show("Vui lòng nhập Số Lượng.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                guna2TextBox1.Focus();
                return false;
            }

            try
            {
                using (var dbContext = new ThuVienNhom3Context())
                {
                    // Bắt đầu truy vấn kiểm tra trùng lặp
                    var query = dbContext.Saches
                                        .Where(s => s.TenSach.ToLower() == TextBox_TenSach.Text.ToLower());

                    // Nếu đang ở chế độ CẬP NHẬT, loại trừ sách hiện tại ra khỏi kiểm tra
                    if (isUpdating && selectedSachId > 0)
                    {
                        query = query.Where(s => s.MaSach != selectedSachId);
                    }

                    // Thực thi truy vấn: Kiểm tra xem có bất kỳ sách nào trùng tên không
                    if (query.Any())
                    {
                        MessageBox.Show("Tên Sách này đã tồn tại trong thư viện. Vui lòng nhập tên khác.",
                                        "Lỗi Trùng lặp",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        TextBox_TenSach.Focus();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi kiểm tra trùng lặp: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Trả về false nếu có lỗi CSDL
            }

            int soLuong;
            if (!int.TryParse(guna2TextBox1.Text, out soLuong))
            {
                MessageBox.Show("Số Lượng phải là một số nguyên hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                guna2TextBox1.Focus();
                return false;
            }

            if (soLuong <= 0)
            {
                MessageBox.Show("Số Lượng phải lớn hơn 0.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                guna2TextBox1.Focus();
                return false;
            }


            // --- 3. KIỂM TRA COMBOBOX (FOREIGN KEY ID) ---

            // Kiểm tra Loại Sách (comboBox1)
            if (ComboBox1.SelectedValue == null || Convert.ToInt32(ComboBox1.SelectedValue) == 0) // Giả sử 0 là giá trị mặc định "Chọn..."
            {
                MessageBox.Show("Vui lòng chọn Loại Sách.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ComboBox1.Focus();
                return false;
            }

            // Kiểm tra Nhà Sản Xuất (comboBox2)
            if (ComboBox2.SelectedValue == null || Convert.ToInt32(ComboBox2.SelectedValue) == 0)
            {
                MessageBox.Show("Vui lòng chọn Nhà Sản Xuất.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ComboBox2.Focus();
                return false;
            }

            // --- 4. KIỂM TRA RADIO BUTTON (TÙY CHỌN) ---
            // Mặc dù một trong hai thường đã được chọn, vẫn nên kiểm tra
            if (!radioButton_ConSach.Checked && !radioButton_HetSach.Checked)
            {
                MessageBox.Show("Vui lòng chọn Trạng Thái Sách.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (radioButton_HetSach.Checked == true && soLuong > 0)
            {
                MessageBox.Show("Trạng thái hết sách không hợp lệ với số lượng.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Tất cả kiểm tra thành công
            return true;
        }
        private void ResetFormState()
        {
            // 1. Reset biến cờ và ID
            //isUpdating = false;
            selectedSachId = 0;

            // 2. Xóa dữ liệu cũ trên Form nhập
            ClearInputFields();

            // 3. Đặt lại tên nút Lưu
            Button_Luu.Text = "Lưu";

            // 4. Xóa lựa chọn trên DataGridView (tùy chọn)
            DataGridView_DachSachSach.ClearSelection();
        }

        private void ClearInputFields()
        {
            // Xóa dữ liệu trên các trường nhập
            TextBox_TenSach.Clear();
            guna2TextBox1.Clear();
            guna2TextBox2.Clear();
            radioButton_ConSach.Checked = true; // Đặt lại RadioButton
            ComboBox1.SelectedIndex = -1; // Đặt lại ComboBox
            ComboBox2.SelectedIndex = -1; // Đặt lại ComboBox
        }

        private void Button_Xoa_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra xem người dùng đã chọn hàng nào trên DataGridView chưa
            if (DataGridView_DachSachSach.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một cuốn sách từ danh sách để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy SachId của cuốn sách được chọn
            // Giả định SachId nằm trong cột đầu tiên (index 0) hoặc có tên cột là "SachId"
            // Hãy thay thế "SachId" bằng tên cột thực tế nếu khác
            int selectedSachId = (int)DataGridView_DachSachSach.SelectedRows[0].Cells["MaSach"].Value;

            // 2. Xác nhận với người dùng trước khi xóa
            DialogResult confirmResult = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa cuốn sách này không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    // 3. Sử dụng Entity Framework Core để xóa
                    using (var dbContext = new ThuVienNhom3Context())
                    {
                        // **Cách 1: Truy vấn đối tượng từ CSDL trước (Thường dùng)**
                        // Tìm cuốn sách cần xóa theo ID
                        var sachCanXoa = dbContext.Saches.FirstOrDefault(s => s.MaSach == selectedSachId);

                        if (sachCanXoa != null)
                        {
                            // Đánh dấu đối tượng này là "cần xóa"
                            dbContext.Saches.Remove(sachCanXoa);

                            // Thực hiện lệnh DELETE vào CSDL
                            dbContext.SaveChanges();

                            MessageBox.Show("Xóa sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // 4. Cập nhật lại DataGridView
                            LoadDataToDataGridView();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy sách để xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // **Cách 2: Xóa đối tượng không cần truy vấn (Chỉ khi có đủ khóa chính)**
                        /* var dummySach = new Sach { SachId = selectedSachId };
                        dbContext.Attach(dummySach); // Attach vào context
                        dbContext.Saches.Remove(dummySach); // Đánh dấu xóa
                        dbContext.SaveChanges(); 
                        */
                    }
                }
                catch (Exception ex)
                {
                    // Bắt lỗi CSDL, ví dụ lỗi khóa ngoại (Foreign Key Constraint)
                    MessageBox.Show($"Lỗi khi xóa sách: {ex.Message}\nCó thể sách đang có phiếu mượn liên quan.", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Button_TimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = TextBox_TimKiemSach.Text.Trim();


            try
            {
                using (var dbContext = new ThuVienNhom3Context())
                {
                    var query = dbContext.Saches
                                 .Include(s => s.MaLoaiSachNavigation)
                                 .Include(s => s.MaNhaSanXuatNavigation);
                    // Sử dụng LINQ để tạo truy vấn lọc
                    var ketQuaTimKiemQuery = query
                                                .Where(s =>
                                                    // Tìm theo Tên Sách (Contains/IndexOf cho phép tìm kiếm một phần)
                                                    s.TenSach.ToLower().Contains(tuKhoa) ||
                                                    // Hoặc tìm theo Tác Giả
                                                    s.TacGia.ToLower().Contains(tuKhoa) ||
                                                    // Hoặc tìm theo Mã Sách (chuyển sang string để so sánh)
                                                    (s.MaLoaiSachNavigation != null && s.MaLoaiSachNavigation.TenLoaiSach.ToLower().Contains(tuKhoa.ToLower())) ||
                                                    // Hoặc tìm theo Tên NXB
                                                    (s.MaNhaSanXuatNavigation != null && s.MaNhaSanXuatNavigation.TenNhaSanXuat.ToLower().Contains(tuKhoa.ToLower()))
                                                );// Thực thi truy vấn và lấy kết quả
                    var ketQuaTimKiem = ketQuaTimKiemQuery.Select(s => new // Tạo Projection
                    {
                        // Giữ lại ID ẩn
                        MaSach = s.MaSach,

                        // Các cột hiển thị thông tin sách
                        TenSach = s.TenSach,
                        SoLuong = s.SoLuong,
                        TacGia = s.TacGia,
                        TrangThai = s.TrangThai,

                        // >> Lấy TÊN từ thuộc tính Điều hướng <<
                        TenLoaiSach = s.MaLoaiSachNavigation.TenLoaiSach,
                        TenNhaSanXuat = s.MaNhaSanXuatNavigation.TenNhaSanXuat,

                        // Giữ lại Mã nếu cần cho mục đích khác
                        MaLoaiSach = s.MaLoaiSach,
                        MaNhaSanXuat = s.MaNhaSanXuat
                    }).ToList(); // Thực thi truy vấn và lấy kết quả
                                 // Gán kết quả tìm kiếm cho DataGridView
                    DataGridView_DachSachSach.DataSource = ketQuaTimKiem;

                    // Tùy chỉnh cột lại nếu cần (ví dụ: đổi tên tiêu đề)
                    CustomizeDataGridViewColumns();

                    if (ketQuaTimKiem.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy cuốn sách nào khớp với từ khóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm dữ liệu: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLoaiSachToComboBox()
        {
            using (var dbContext = new ThuVienNhom3Context())
            {
                // 1. Tải dữ liệu từ CSDL
                var danhSachLoaiSach = dbContext.LoaiSaches.ToList();

                // 2. Thêm mục mặc định vào đầu danh sách
                // Đảm bảo MaLoaiSach có kiểu dữ liệu khớp với thuộc tính trong lớp mô hình LoaiSach
                danhSachLoaiSach.Insert(0, new LoaiSach { MaLoaiSach = 0, TenLoaiSach = "   __loại sách__" });

                // 3. >> THIẾT LẬP THUỘC TÍNH HIỂN THỊ VÀ GIÁ TRỊ <<
                ComBox_LocLoaiSach.DisplayMember = "TenLoaiSach"; // Tên thuộc tính muốn hiển thị
                ComBox_LocLoaiSach.ValueMember = "MaLoaiSach";   // Tên thuộc tính làm giá trị (ID)

                // 4. Gán nguồn dữ liệu
                ComBox_LocLoaiSach.DataSource = danhSachLoaiSach;

                // Tùy chọn: Chọn mục mặc định đã thêm
                ComBox_LocLoaiSach.SelectedIndex = 0;
            }
        }

        private void ComBox_LocLoaiSach_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComBox_LocLoaiSach.SelectedValue != null && DanhSachSachGoc != null)
            {
                ThucHienLocDuLieu(); // Gọi hàm lọc
            }
        }
        // Logic này đã đúng và nên được giữ nguyên!
        // List rõ ràng
        private void ThucHienLocDuLieu()
        {
            if (DanhSachSachGoc == null) return;

            // >> Lấy Tên Loại Sách đã chọn từ ComboBox.Text <<
            string tenLoaiSachDuocChon = ComBox_LocLoaiSach.Text.Trim();

            // Lấy danh sách gốc (sử dụng IEnumerable<dynamic> để LINQ hoạt động)
            var listGocDynamic = DanhSachSachGoc as IEnumerable<dynamic>;

            if (listGocDynamic == null) return;

            // Kiểm tra nếu người dùng chọn mục mặc định hoặc không chọn gì
            if (tenLoaiSachDuocChon == "__loại sách__" || string.IsNullOrEmpty(tenLoaiSachDuocChon))
            {
                // Hiển thị toàn bộ danh sách gốc
                DataGridView_DachSachSach.DataSource = DanhSachSachGoc;
            }
            else
            {
                // Lọc theo Tên Loại Sách (Tên_Loại_Sách là thuộc tính trong Projection của bạn)
                // Lưu ý: Dùng ToLower() để tìm kiếm không phân biệt chữ hoa/thường
                var danhSachDaLoc = listGocDynamic
                     .Where(s => s.TenLoaiSach.ToLower() == tenLoaiSachDuocChon.ToLower())
                     .ToList();

                DataGridView_DachSachSach.DataSource = danhSachDaLoc;
            }
            CustomizeDataGridViewColumns();
        }

        private void Button_XuatFile_Click(object sender, EventArgs e)
        {

            // 1. Mở hộp thoại Lưu file
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            sfd.FileName = "DanhSachSach_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
            sfd.Title = "Lưu danh sách sách thành file CSV";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // 2. Chuẩn bị nội dung file
                try
                {
                    // Sử dụng StringBuilder để xây dựng nội dung file hiệu quả
                    StringBuilder sb = new StringBuilder();
                    // >> 2a. GHI TIÊU ĐỀ CỘT <<
                    // Duyệt qua các cột hiển thị trong DataGridView
                    string[] columnNames = DataGridView_DachSachSach.Columns
                                                .Cast<DataGridViewColumn>()
                                                .Where(c => c.Visible) // Chỉ lấy các cột đang hiển thị
                                                .Select(column => column.HeaderText) // Lấy tiêu đề cột
                                                .ToArray();

                    // Thêm các tiêu đề cột vào StringBuilder, cách nhau bằng dấu phẩy
                    sb.AppendLine(string.Join(",", columnNames));

                    // >> 2b. GHI DỮ LIỆU CÁC HÀNG <<
                    foreach (DataGridViewRow row in DataGridView_DachSachSach.Rows)
                    {
                        if (!row.IsNewRow) // Bỏ qua hàng trống cuối cùng
                        {
                            // Lấy giá trị của từng ô (cell) trong hàng
                            var cells = row.Cells.Cast<DataGridViewCell>()
                                               .Where(c => c.OwningColumn.Visible) // Chỉ lấy các ô thuộc cột hiển thị
                                               .Select(cell => EscapeCsvString(cell.Value?.ToString() ?? "")); // Xử lý giá trị và null

                            sb.AppendLine(string.Join(",", cells));
                        }
                    }

                    // 3. Ghi nội dung vào file
                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);

                    MessageBox.Show("Xuất file thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private string EscapeCsvString(string value)
        {
            // 1. Kiểm tra null/rỗng
            if (string.IsNullOrEmpty(value)) return "";

            // 2. Xóa các ký tự xuống dòng
            value = value.Replace("\r\n", " ").Replace('\n', ' ').Replace('\r', ' ');

            // 3. Nếu chuỗi chứa dấu phẩy hoặc dấu nháy kép, cần bọc bằng nháy kép
            if (value.Contains(',') || value.Contains('"'))
            {
                // Thay thế mọi dấu nháy kép bên trong bằng hai dấu nháy kép ("")
                // và bọc toàn bộ chuỗi bằng dấu nháy kép.
                return $"\"{value.Replace("\"", "\"\"")}\""; // Dùng interpolation string ($"")
            }

            return value;
        }
    }
}
