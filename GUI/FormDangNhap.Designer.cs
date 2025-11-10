namespace QuanLyThuVienNhom3
{
    partial class FormDangNhap
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDangNhap));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Button_DangNhap = new Guna.UI2.WinForms.Guna2Button();
            Text_Login = new Guna.UI2.WinForms.Guna2HtmlLabel();
            label_TaiKhoan = new Label();
            label_MatKhau = new Label();
            pictureBox1 = new PictureBox();
            TextBox_TaiKhoan = new Guna.UI2.WinForms.Guna2TextBox();
            TextBox_MatKhau = new Guna.UI2.WinForms.Guna2TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // Button_DangNhap
            // 
            Button_DangNhap.Anchor = AnchorStyles.None;
            Button_DangNhap.Animated = true;
            Button_DangNhap.AnimatedGIF = true;
            Button_DangNhap.BorderColor = Color.Teal;
            Button_DangNhap.BorderRadius = 10;
            Button_DangNhap.BorderThickness = 2;
            Button_DangNhap.CustomizableEdges = customizableEdges7;
            Button_DangNhap.DisabledState.BorderColor = Color.DarkGray;
            Button_DangNhap.DisabledState.CustomBorderColor = Color.DarkGray;
            Button_DangNhap.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            Button_DangNhap.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            Button_DangNhap.FillColor = Color.FromArgb(0, 64, 64);
            Button_DangNhap.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 163);
            Button_DangNhap.ForeColor = SystemColors.ButtonFace;
            Button_DangNhap.HoverState.BorderColor = Color.Gray;
            Button_DangNhap.HoverState.FillColor = Color.Teal;
            Button_DangNhap.HoverState.ForeColor = Color.Gray;
            Button_DangNhap.Location = new Point(575, 470);
            Button_DangNhap.Name = "Button_DangNhap";
            Button_DangNhap.ShadowDecoration.CustomizableEdges = customizableEdges8;
            Button_DangNhap.Size = new Size(285, 69);
            Button_DangNhap.TabIndex = 0;
            Button_DangNhap.Text = "Đăng nhập";
            Button_DangNhap.TextTransform = Guna.UI2.WinForms.Enums.TextTransform.UpperCase;
            // 
            // Text_Login
            // 
            Text_Login.Anchor = AnchorStyles.None;
            Text_Login.BackColor = Color.Transparent;
            Text_Login.Font = new Font("Tahoma", 24F, FontStyle.Bold, GraphicsUnit.Point, 163);
            Text_Login.ForeColor = Color.White;
            Text_Login.Location = new Point(619, 74);
            Text_Login.Name = "Text_Login";
            Text_Login.Size = new Size(222, 50);
            Text_Login.TabIndex = 1;
            Text_Login.Text = "Đăng nhập";
            // 
            // label_TaiKhoan
            // 
            label_TaiKhoan.Anchor = AnchorStyles.None;
            label_TaiKhoan.AutoSize = true;
            label_TaiKhoan.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_TaiKhoan.ForeColor = Color.White;
            label_TaiKhoan.Location = new Point(409, 263);
            label_TaiKhoan.Name = "label_TaiKhoan";
            label_TaiKhoan.Size = new Size(116, 25);
            label_TaiKhoan.TabIndex = 2;
            label_TaiKhoan.Text = "Tên tài khoản";
            // 
            // label_MatKhau
            // 
            label_MatKhau.Anchor = AnchorStyles.None;
            label_MatKhau.AutoSize = true;
            label_MatKhau.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_MatKhau.ForeColor = Color.White;
            label_MatKhau.Location = new Point(439, 353);
            label_MatKhau.Name = "label_MatKhau";
            label_MatKhau.Size = new Size(86, 25);
            label_MatKhau.TabIndex = 3;
            label_MatKhau.Text = "Mật khẩu";
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.None;
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Enabled = false;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(439, 31);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(147, 140);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // TextBox_TaiKhoan
            // 
            TextBox_TaiKhoan.Anchor = AnchorStyles.None;
            TextBox_TaiKhoan.Animated = true;
            TextBox_TaiKhoan.BackColor = Color.Transparent;
            TextBox_TaiKhoan.BorderColor = Color.Gray;
            TextBox_TaiKhoan.BorderRadius = 10;
            TextBox_TaiKhoan.BorderThickness = 2;
            TextBox_TaiKhoan.CustomizableEdges = customizableEdges9;
            TextBox_TaiKhoan.DefaultText = "";
            TextBox_TaiKhoan.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            TextBox_TaiKhoan.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            TextBox_TaiKhoan.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            TextBox_TaiKhoan.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            TextBox_TaiKhoan.FocusedState.BorderColor = Color.White;
            TextBox_TaiKhoan.FocusedState.FillColor = Color.FromArgb(224, 224, 224);
            TextBox_TaiKhoan.FocusedState.ForeColor = Color.FromArgb(0, 64, 64);
            TextBox_TaiKhoan.FocusedState.PlaceholderForeColor = Color.Transparent;
            TextBox_TaiKhoan.Font = new Font("Segoe UI", 9F);
            TextBox_TaiKhoan.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            TextBox_TaiKhoan.Location = new Point(548, 254);
            TextBox_TaiKhoan.Margin = new Padding(3, 4, 3, 4);
            TextBox_TaiKhoan.Name = "TextBox_TaiKhoan";
            TextBox_TaiKhoan.PlaceholderText = "";
            TextBox_TaiKhoan.SelectedText = "";
            TextBox_TaiKhoan.ShadowDecoration.CustomizableEdges = customizableEdges10;
            TextBox_TaiKhoan.Size = new Size(349, 43);
            TextBox_TaiKhoan.TabIndex = 5;
            // 
            // TextBox_MatKhau
            // 
            TextBox_MatKhau.Anchor = AnchorStyles.None;
            TextBox_MatKhau.Animated = true;
            TextBox_MatKhau.BackColor = Color.Transparent;
            TextBox_MatKhau.BorderColor = Color.Gray;
            TextBox_MatKhau.BorderRadius = 10;
            TextBox_MatKhau.BorderThickness = 2;
            TextBox_MatKhau.CustomizableEdges = customizableEdges11;
            TextBox_MatKhau.DefaultText = "";
            TextBox_MatKhau.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            TextBox_MatKhau.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            TextBox_MatKhau.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            TextBox_MatKhau.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            TextBox_MatKhau.FocusedState.BorderColor = Color.White;
            TextBox_MatKhau.FocusedState.FillColor = Color.FromArgb(224, 224, 224);
            TextBox_MatKhau.FocusedState.ForeColor = Color.FromArgb(0, 64, 64);
            TextBox_MatKhau.FocusedState.PlaceholderForeColor = Color.Transparent;
            TextBox_MatKhau.Font = new Font("Segoe UI", 9F);
            TextBox_MatKhau.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            TextBox_MatKhau.Location = new Point(548, 345);
            TextBox_MatKhau.Margin = new Padding(3, 4, 3, 4);
            TextBox_MatKhau.Name = "TextBox_MatKhau";
            TextBox_MatKhau.PlaceholderText = "";
            TextBox_MatKhau.SelectedText = "";
            TextBox_MatKhau.ShadowDecoration.CustomizableEdges = customizableEdges12;
            TextBox_MatKhau.Size = new Size(349, 43);
            TextBox_MatKhau.TabIndex = 6;
            // 
            // FormDangNhap
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.Black;
            ClientSize = new Size(1432, 551);
            Controls.Add(TextBox_MatKhau);
            Controls.Add(TextBox_TaiKhoan);
            Controls.Add(pictureBox1);
            Controls.Add(label_MatKhau);
            Controls.Add(label_TaiKhoan);
            Controls.Add(Text_Login);
            Controls.Add(Button_DangNhap);
            Name = "FormDangNhap";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button Button_DangNhap;
        private Guna.UI2.WinForms.Guna2HtmlLabel Text_Login;
        private Label label_TaiKhoan;
        private Label label_MatKhau;
        private PictureBox pictureBox1;
        private Guna.UI2.WinForms.Guna2TextBox TextBox_TaiKhoan;
        private Guna.UI2.WinForms.Guna2TextBox TextBox_MatKhau;
    }
}
