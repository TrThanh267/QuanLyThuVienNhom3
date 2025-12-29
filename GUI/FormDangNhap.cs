using QuanLyThuVienNhom3.BLL;
using QuanLyThuVienNhom3.GUI;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace QuanLyThuVienNhom3
{
    public partial class FormDangNhap : Form
    {
        private Login_BLL _BLL = new Login_BLL();
        string _emailTam = "";
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

        private void linkLabel_QuenmatKhau_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Panel_QuenMatKhau.Visible = true;
        }
        private bool ThucHienGuiEmail(string recipient, string otp)
        {
            try
            {
                var senderEmail = new MailAddress("sherwin2267@gmail.com", "Hệ Thống Thư Viện");
                var receiverEmail = new MailAddress(recipient);
                string appPassword = "pyvy aotp zocz pswd";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, appPassword)
                };

                using (var message = new MailMessage(senderEmail, receiverEmail))
                {
                    message.Subject = "Mã xác thực đổi mật khẩu";
                    message.Body = $"Mã OTP của bạn là: {otp}\nLưu ý: Mã có hiệu lực trong 5 phút.";
                    smtp.Send(message);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gửi Email thất bại: " + ex.Message);
                return false;
            }
        }

        private void Button_GuiOpt_Click(object sender, EventArgs e)
        {
            string emailInput = TextBox_EmailQuenMatKhau.Text.Trim();

            if (string.IsNullOrEmpty(emailInput))
            {
                MessageBox.Show("Vui lòng nhập Email!");
                return;
            }
            var taiKhoan = _BLL.GetTaiKhoanByEmail(emailInput);

            if (taiKhoan == null)
            {
                MessageBox.Show("Email này không liên kết với bất kỳ tài khoản nào!");
                return;
            }
            _emailTam = emailInput;
            string otpCode = _BLL.GenerateAndSaveOtp(_emailTam);

            if (ThucHienGuiEmail(_emailTam, otpCode))
            {
                MessageBox.Show("Mã OTP đã được gửi! Vui lòng kiểm tra hộp thư.");
                Panel_NhapMatKhauMoi.Visible = true;
                Panel_QuenMatKhau.Visible = false;
            }
        }

        private void Button_XacNhanMatKhauMoi_Click(object sender, EventArgs e)
        {
            string otpNhap = TextBox_NhapMaOTP.Text.Trim();
            string mkMoi = TextBox_MatKhauMoi.Text;
            string xacNhanMk = TextBox_XacNhanMK.Text;
            if (!Regex.IsMatch(mkMoi, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"))
            {
                MessageBox.Show("Mật khẩu phải ≥8 ký tự, có chữ, số và ký tự đặc biệt!", "Yêu cầu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(otpNhap) || string.IsNullOrEmpty(mkMoi) || string.IsNullOrEmpty(xacNhanMk))
            {
                MessageBox.Show("Không được để trống dữ liệu");
                return;
            }
            if (mkMoi != xacNhanMk)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!");
                return;
            }

            bool thanhCong = _BLL.VerifyOtpAndResetPassword(_emailTam, otpNhap, mkMoi);

            if (thanhCong)
            {
                MessageBox.Show("Đổi mật khẩu thành công!");
                this.Close();
                Panel_QuenMatKhau.Visible = false;
            }
            else
            {
                MessageBox.Show("Mã OTP không đúng hoặc đã hết hạn (5 phút)!");
            }
        }

        private void Button_Huy_Click(object sender, EventArgs e)
        {
            Panel_QuenMatKhau.Visible = false;
        }

        private void Button_QuayLai_Click(object sender, EventArgs e)
        {
            Panel_NhapMatKhauMoi.Visible = false;
        }
    }
}
