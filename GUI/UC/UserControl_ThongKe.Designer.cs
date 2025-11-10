namespace QuanLyThuVienNhom3.GUI.UC
{
    partial class UserControl_ThongKe
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Panel_ThongTinSach = new Guna.UI2.WinForms.Guna2GradientPanel();
            label_ThongTinSach = new Label();
            Panel_ThongKe = new Guna.UI2.WinForms.Guna2GradientPanel();
            ComboBox_LocTheoThang = new Guna.UI2.WinForms.Guna2ComboBox();
            label1 = new Label();
            formsPlot_ThongKeLanMuonMoiThang = new ScottPlot.WinForms.FormsPlot();
            Panel_ThongTinSach.SuspendLayout();
            Panel_ThongKe.SuspendLayout();
            SuspendLayout();
            // 
            // Panel_ThongTinSach
            // 
            Panel_ThongTinSach.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Panel_ThongTinSach.BackColor = Color.Transparent;
            Panel_ThongTinSach.BorderColor = Color.Red;
            Panel_ThongTinSach.BorderRadius = 10;
            Panel_ThongTinSach.Controls.Add(label_ThongTinSach);
            Panel_ThongTinSach.CustomBorderColor = Color.FromArgb(192, 0, 192);
            Panel_ThongTinSach.CustomizableEdges = customizableEdges1;
            Panel_ThongTinSach.FillColor = Color.Teal;
            Panel_ThongTinSach.FillColor2 = Color.Transparent;
            Panel_ThongTinSach.Location = new Point(23, 15);
            Panel_ThongTinSach.Name = "Panel_ThongTinSach";
            Panel_ThongTinSach.ShadowDecoration.CustomizableEdges = customizableEdges2;
            Panel_ThongTinSach.Size = new Size(1045, 44);
            Panel_ThongTinSach.TabIndex = 2;
            // 
            // label_ThongTinSach
            // 
            label_ThongTinSach.AutoSize = true;
            label_ThongTinSach.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label_ThongTinSach.ForeColor = Color.White;
            label_ThongTinSach.Location = new Point(22, 11);
            label_ThongTinSach.Name = "label_ThongTinSach";
            label_ThongTinSach.Size = new Size(82, 23);
            label_ThongTinSach.TabIndex = 0;
            label_ThongTinSach.Text = "Thống kê";
            // 
            // Panel_ThongKe
            // 
            Panel_ThongKe.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Panel_ThongKe.BackColor = Color.Teal;
            Panel_ThongKe.BorderColor = Color.Red;
            Panel_ThongKe.BorderRadius = 5;
            Panel_ThongKe.BorderStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            Panel_ThongKe.Controls.Add(ComboBox_LocTheoThang);
            Panel_ThongKe.Controls.Add(label1);
            Panel_ThongKe.Controls.Add(formsPlot_ThongKeLanMuonMoiThang);
            Panel_ThongKe.CustomizableEdges = customizableEdges5;
            Panel_ThongKe.FillColor2 = Color.Gray;
            Panel_ThongKe.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            Panel_ThongKe.Location = new Point(23, 81);
            Panel_ThongKe.Name = "Panel_ThongKe";
            Panel_ThongKe.ShadowDecoration.CustomizableEdges = customizableEdges6;
            Panel_ThongKe.Size = new Size(1045, 862);
            Panel_ThongKe.TabIndex = 6;
            // 
            // ComboBox_LocTheoThang
            // 
            ComboBox_LocTheoThang.BackColor = Color.Transparent;
            ComboBox_LocTheoThang.CustomizableEdges = customizableEdges3;
            ComboBox_LocTheoThang.DrawMode = DrawMode.OwnerDrawFixed;
            ComboBox_LocTheoThang.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBox_LocTheoThang.FocusedColor = Color.FromArgb(94, 148, 255);
            ComboBox_LocTheoThang.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            ComboBox_LocTheoThang.Font = new Font("Segoe UI", 10F);
            ComboBox_LocTheoThang.ForeColor = Color.FromArgb(68, 88, 112);
            ComboBox_LocTheoThang.ItemHeight = 30;
            ComboBox_LocTheoThang.Location = new Point(117, 703);
            ComboBox_LocTheoThang.Name = "ComboBox_LocTheoThang";
            ComboBox_LocTheoThang.ShadowDecoration.CustomizableEdges = customizableEdges4;
            ComboBox_LocTheoThang.Size = new Size(175, 36);
            ComboBox_LocTheoThang.TabIndex = 2;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label1.ForeColor = Color.White;
            label1.Location = new Point(244, 71);
            label1.Name = "label1";
            label1.Size = new Size(544, 33);
            label1.TabIndex = 1;
            label1.Text = "Thống kê số lần mượn sách theo tháng";
            // 
            // formsPlot_ThongKeLanMuonMoiThang
            // 
            formsPlot_ThongKeLanMuonMoiThang.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            formsPlot_ThongKeLanMuonMoiThang.BackColor = Color.Transparent;
            formsPlot_ThongKeLanMuonMoiThang.DisplayScale = 1.25F;
            formsPlot_ThongKeLanMuonMoiThang.Enabled = false;
            formsPlot_ThongKeLanMuonMoiThang.ForeColor = Color.White;
            formsPlot_ThongKeLanMuonMoiThang.Location = new Point(108, 186);
            formsPlot_ThongKeLanMuonMoiThang.Name = "formsPlot_ThongKeLanMuonMoiThang";
            formsPlot_ThongKeLanMuonMoiThang.Size = new Size(799, 448);
            formsPlot_ThongKeLanMuonMoiThang.TabIndex = 0;
            // 
            // UserControl_ThongKe
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(0, 64, 64);
            Controls.Add(Panel_ThongKe);
            Controls.Add(Panel_ThongTinSach);
            Name = "UserControl_ThongKe";
            Size = new Size(1090, 960);
            Panel_ThongTinSach.ResumeLayout(false);
            Panel_ThongTinSach.PerformLayout();
            Panel_ThongKe.ResumeLayout(false);
            Panel_ThongKe.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2GradientPanel Panel_ThongTinSach;
        private Label label_ThongTinSach;
        private Guna.UI2.WinForms.Guna2GradientPanel Panel_ThongKe;
        private ScottPlot.WinForms.FormsPlot formsPlot_ThongKeLanMuonMoiThang;
        private Label label1;
        private Guna.UI2.WinForms.Guna2ComboBox ComboBox_LocTheoThang;
    }
}
