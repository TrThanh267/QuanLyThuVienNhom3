using System;
using System.Collections.Generic;

namespace QuanLyThuVienNhom3.Models;

public partial class NhaSanXuat
{
    public int MaNhaSanXuat { get; set; }

    public string? TenNhaSanXuat { get; set; }

    public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
}
