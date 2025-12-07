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
            DateTimePicker_DenNgay.Value = DateTime.Now;
            DateTimePicker_Loc.Value = DateTime.Now.AddDays(-30);
            DateTimePicker_Loc.ValueChanged += DateTimePicker_Loc_ValueChanged;
            LoadData(DateOnly.FromDateTime(DateTimePicker_Loc.Value), DateOnly.FromDateTime(DateTimePicker_DenNgay.Value));
            LoadData();
        }
        public void LoadData()
        {
            DateOnly tuNgay = DateOnly.FromDateTime(DateTimePicker_Loc.Value);
            DateOnly denNgay = DateOnly.FromDateTime(DateTimePicker_DenNgay.Value);
            if (tuNgay > denNgay)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.LoadData(tuNgay, denNgay); 
        }
        public void LoadData(DateOnly ngayBatDau, DateOnly ngayKetThuc)
        {
            List<ThongKe_DTO> data = _BLL.ThongKePM(ngayBatDau, ngayKetThuc);
            double[] values = data.Select(d => (double)d.TongSoLuotMuon).ToArray();
            string[] labels = data.Select(d => d.NgayMuon.ToString("dd/MM")).ToArray();
            double[] barPositions = Enumerable.Range(0, values.Length).Select(i => (double)i).ToArray();
            int maxLuotMuonThucTe = (int)Math.Ceiling(values.DefaultIfEmpty(0).Max());
            int gioiHanMinCoDinh = _BLL.SoluotMuonLonNhat();
            int gioiHanMaxCanThiet = Math.Max(maxLuotMuonThucTe, gioiHanMinCoDinh);
            int gioiHanMaxAnToan = (int)Math.Ceiling(gioiHanMaxCanThiet * 1.2);
            int maxTickValue = gioiHanMaxAnToan;
            formsPlot_ThongKeLanMuonMoiThang.Plot.Clear();
            formsPlot_ThongKeLanMuonMoiThang.Plot.Add.Bars(barPositions, values);
            formsPlot_ThongKeLanMuonMoiThang.Plot.Axes.Right.IsVisible = false;
            formsPlot_ThongKeLanMuonMoiThang.Plot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(
                positions: barPositions, 
                labels: labels
            );
            formsPlot_ThongKeLanMuonMoiThang.Plot.Axes.Bottom.Min = -0.5;
            formsPlot_ThongKeLanMuonMoiThang.Plot.Axes.Bottom.Max = values.Length - 0.5;
            formsPlot_ThongKeLanMuonMoiThang.Plot.Axes.Left.Label.Text = "Lượt";
            formsPlot_ThongKeLanMuonMoiThang.Plot.Axes.Left.Min = 0;
            formsPlot_ThongKeLanMuonMoiThang.Plot.Axes.Left.Max = gioiHanMaxAnToan; 
            formsPlot_ThongKeLanMuonMoiThang.Plot.Axes.Left.TickGenerator =
                new ScottPlot.TickGenerators.NumericManual(
                    positions: Enumerable.Range(0, maxTickValue + 1).Select(i => (double)i).ToArray(),
                    labels: Enumerable.Range(0, maxTickValue + 1).Select(i => i.ToString()).ToArray()
                );
            formsPlot_ThongKeLanMuonMoiThang.Plot.Axes.Title.Label.Text = $"Thống kê từ {ngayBatDau:dd/MM/yyyy} đến {ngayKetThuc:dd/MM/yyyy}";
            formsPlot_ThongKeLanMuonMoiThang.Plot.Axes.SetLimits();

            formsPlot_ThongKeLanMuonMoiThang.Refresh();
        }

        private void DateTimePicker_Loc_ValueChanged(object sender, EventArgs e)
        {
            DateTime tuNgay = DateTimePicker_Loc.Value.Date;
            DateTime denNgayMacDinh = tuNgay.AddDays(30).Date;
            DateTimePicker_DenNgay.Value = denNgayMacDinh;
            DateOnly tuNgayDO = DateOnly.FromDateTime(tuNgay);
            DateOnly denNgayDO = DateOnly.FromDateTime(denNgayMacDinh);
            LoadData(tuNgayDO, denNgayDO);
        }
    }
}
