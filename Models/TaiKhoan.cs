using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
    [NotMapped]
    public string? OtpCode { get; set; }

    [NotMapped]
    public DateTime? OtpExpiry { get; set; }
}
