using QuanLyThuVienNhom3.BLL;
using QuanLyThuVienNhom3.GUI;

namespace QuanLyThuVienNhom3
{
    public partial class FormDangNhap : Form
    {
        private Login_BLL _BLL = new Login_BLL();
        public FormDangNhap()
        {
            InitializeComponent();
        }

        private void Button_DangNhap_Click(object sender, EventArgs e)
        {
            string user = TextBox_TaiKhoan.Text;
            string pass = TextBox_MatKhau.Text;
            string result = _BLL.Login(user, pass);
            if (result == "")
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FormTrangChu f = new FormTrangChu();
                this.Hide();
                f.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show(result, "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TextBox_MatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Button_DangNhap.PerformClick();
            }
        }

        private void CheckBox_HienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            bool show = CheckBox_HienMatKhau.Checked;
            TextBox_MatKhau.UseSystemPasswordChar = !show;
        }
    }
}
