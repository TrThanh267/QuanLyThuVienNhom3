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
        public UserControl_QuanLyChamCong()
        {
            InitializeComponent();
        }

        private void dateTimePicker_TinhLuongThang_ValueChanged(object sender, EventArgs e)
        {
            int thang = dateTimePicker_TinhLuongThang.Value.Month;
            int nam = dateTimePicker_TinhLuongThang.Value.Year;
        }
    }
}
