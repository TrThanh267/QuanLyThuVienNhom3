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
        public UserControl_ThongKe()
        {
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {
            formsPlot_ThongKeLanMuonMoiThang.Plot.HideGrid();
        }
    }
}
