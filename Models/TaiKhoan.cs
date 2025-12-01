using System;
using System.Collections.Generic;

namespace QuanLyThuVienNhom3.Models;

public partial class TaiKhoan
{
    public int MaTaiKhoan { get; set; }

    public string? TenTaiKhoan { get; set; }

    public string? MatKhauHash { get; set; }

    public string? TrangThai { get; set; }

    public int? MaVaiTro { get; set; }

    public virtual VaiTro? MaVaiTroNavigation { get; set; }

    public virtual NhanVien? NhanVien { get; set; }
}
