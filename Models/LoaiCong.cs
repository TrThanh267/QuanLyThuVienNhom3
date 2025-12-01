using System;
using System.Collections.Generic;

namespace QuanLyThuVienNhom3.Models;

public partial class LoaiCong
{
    public int MaLoaiCong { get; set; }

    public string? TenloaiCong { get; set; }

    public virtual ICollection<ChamCong> ChamCongs { get; set; } = new List<ChamCong>();
}
