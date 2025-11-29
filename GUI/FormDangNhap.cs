namespace QuanLyThuVienNhom3
{
    public partial class FormDangNhap : Form
    {
        public FormDangNhap()
        {
            InitializeComponent();
        }
        public void Dang()
        {
            FormDangNhap frmMain = new FormDangNhap();
            this.Hide();
            frmMain.ShowDialog();
            this.Show();
        }
    }
}
