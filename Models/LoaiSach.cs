using System;
using System.Collections.Generic;

namespace QuanLyThuVienNhom3.Models;

public partial class LoaiSach
{
    public int MaLoaiSach { get; set; }

    public string? TenLoaiSach { get; set; }

    public virtual ICollection<Sach> Saches { get; set; } = new List<Sach>();
}
