using QuanLyThuVienNhom3.BLL;
using QuanLyThuVienNhom3.DTO;
using QuanLyThuVienNhom3.BLL;
using QuanLyThuVienNhom3.DTO;
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
    public partial class UserControl_ThongKe : UserControl
    {
        private ThongKe_BLL _BLL = new ThongKe_BLL();
        public UserControl_ThongKe()
        {
            InitializeComponent();
            // Đặt mặc định ngày cho DatePicker (ví dụ)
            DateTimePicker_DenNgay.Value = DateTime.Now;
            DateTimePicker_Loc.Value = DateTime.Now.AddDays(-30);
            DateTimePicker_Loc.ValueChanged += DateTimePicker_Loc_ValueChanged;
            // Gọi LoadData lần đầu
            LoadData(DateOnly.FromDateTime(DateTimePicker_Loc.Value), DateOnly.FromDateTime(DateTimePicker_DenNgay.Value));
            LoadData();
        }
        public void LoadData()
        {
            // Lấy giá trị từ các DateTimePicker nằm trên UserControl này
            DateOnly tuNgay = DateOnly.FromDateTime(DateTimePicker_Loc.Value);
            DateOnly denNgay = DateOnly.FromDateTime(DateTimePicker_DenNgay.Value);

            // Kiểm tra điều kiện ngày tháng hợp lệ (Nếu cần)
            if (tuNgay > denNgay)
            {
                // Có thể hiển thị MessageBox hoặc xử lý lỗi
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Gọi hàm LoadData cũ của bạn với các tham số đã lấy
            this.LoadData(tuNgay, denNgay); // Gọi hàm LoadData(DateOnly, DateOnly) hiện có
        }
        public void LoadData(DateOnly ngayBatDau, DateOnly ngayKetThuc)
        {
            // 1. LẤY DỮ LIỆU ĐÃ LỌC VÀ CHUẨN BỊ MẢNG GIÁ TRỊ (VALUES & LABELS)
            List<ThongKe_DTO> data = _BLL.ThongKePM(ngayBatDau, ngayKetThuc);
            double[] values = data.Select(d => (double)d.TongSoLuotMuon).ToArray();
            string[] labels = data.Select(d => d.NgayMuon.ToString("dd/MM")).ToArray();

            // ⭐ Tạo mảng vị trí cho các cột (0, 1, 2, 3, ...) để căn chỉnh
            double[] barPositions = Enumerable.Range(0, values.Length).Select(i => (double)i).ToArray();

            // 2. TÍNH TOÁN GIỚI HẠN MAX (MAXIMUM) LINH HOẠT
            int maxLuotMuonThucTe = (int)Math.Ceiling(values.DefaultIfEmpty(0).Max());
            int gioiHanMinCoDinh = _BLL.SoluotMuonLonNhat();
            int gioiHanMaxCanThiet = Math.Max(maxLuotMuonThucTe, gioiHanMinCoDinh);

            // Thêm 20% khoảng đệm trên đỉnh
            int gioiHanMaxAnToan = (int)Math.Ceiling(gioiHanMaxCanThiet * 1.2);
            int maxTickValue = gioiHanMaxAnToan; // Dùng cho việc tạo ticks số nguyên

            // 3. VẼ BIỂU ĐỒ & CĂN CHỈNH VỊ TRÍ CỘT
            formsPlot_ThongKeLanMuonMoiThang.Plot.Clear();

            // Thêm barPositions vào Add.Bars để căn chỉnh cột
            formsPlot_ThongKeLanMuonMoiThang.Plot.Add.Bars(barPositions, values);

            // Ẩn trục Y bên phải
            formsPlot_ThongKeLanMuonMoiThang.Plot.Axes.Right.IsVisible = false;

            // 4. THIẾT LẬP TRỤC X (NGÀY)
            // Cài đặt TickGenerator bằng NumericManual để sử dụng labels
            formsPlot_ThongKeLanMuonMoiThang.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(
                positions: barPositions, // Dùng chung mảng vị trí để căn chỉnh
                labels: labels
            );

            // ⭐ ĐẶT GIỚI HẠN MIN/MAX cho trục X để căn chỉnh cột đầu tiên
            formsPlot_ThongKeLanMuonMoiThang.Plot.Axes.Bottom.Min = -0.5;
            formsPlot_ThongKeLanMuonMoiThang.Plot.Axes.Bottom.Max = values.Length - 0.5;

            // 5. THIẾT LẬP TRỤC Y (LƯỢT)
            formsPlot_ThongKeLanMuonMoiThang.Plot.Axes.Left.Label.Text = "Lượt";
            formsPlot_ThongKeLanMuonMoiThang.Plot.Axes.Left.Min = 0;
            formsPlot_ThongKeLanMuonMoiThang.Plot.Axes.Left.Max = gioiHanMaxAnToan; // Áp dụng Max linh hoạt

            // Buộc hiển thị ticks số nguyên dựa trên giới hạn đã đặt
            formsPlot_ThongKeLanMuonMoiThang.Plot.Axes.Left.TickGenerator =
                new ScottPlot.TickGenerators.NumericManual(
                    positions: Enumerable.Range(0, maxTickValue + 1).Select(i => (double)i).ToArray(),
                    labels: Enumerable.Range(0, maxTickValue + 1).Select(i => i.ToString()).ToArray()
                );

            // 6. HOÀN THIỆN VÀ HIỂN THỊ
            formsPlot_ThongKeLanMuonMoiThang.Plot.Axes.Title.Label.Text = $"Thống kê từ {ngayBatDau:dd/MM/yyyy} đến {ngayKetThuc:dd/MM/yyyy}";

            // Áp dụng giới hạn Min/Max đã đặt cho cả hai trục
            formsPlot_ThongKeLanMuonMoiThang.Plot.Axes.SetLimits();

            formsPlot_ThongKeLanMuonMoiThang.Refresh();
        }

        private void DateTimePicker_Loc_ValueChanged(object sender, EventArgs e)
        {
            // 1. Lấy ngày bắt đầu mới được chọn
            DateTime tuNgay = DateTimePicker_Loc.Value.Date;

            // 2. TÍNH TOÁN VÀ GÁN LUÔN + 30 NGÀY
            DateTime denNgayMacDinh = tuNgay.AddDays(30).Date;

            // Cập nhật giá trị DatePicker_DenNgay mà không cần kiểm tra DateTime.Now
            DateTimePicker_DenNgay.Value = denNgayMacDinh;


            // 3. Gọi LoadData để lọc và vẽ lại biểu đồ ngay lập tức
            DateOnly tuNgayDO = DateOnly.FromDateTime(tuNgay);
            DateOnly denNgayDO = DateOnly.FromDateTime(denNgayMacDinh);
            LoadData(tuNgayDO, denNgayDO);
        }
    }
}
