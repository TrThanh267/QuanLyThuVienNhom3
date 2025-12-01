using System;
using System.Collections.Generic;

namespace QuanLyThuVienNhom3.Models;

public partial class ChamCong
{
    public int MaChamCong { get; set; }

    public int? NgayLam { get; set; }

    public int? ThangLam { get; set; }

    public int? NamLam { get; set; }

    public int? GioVao { get; set; }

    public int? PhutVao { get; set; }

    public int? GioRa { get; set; }

    public int? PhutRa { get; set; }

    public int? MaLoaiCong { get; set; }

    public int? MaCaLam { get; set; }

    public int? MaNhanVien { get; set; }

    public virtual CaLam? MaCaLamNavigation { get; set; }

    public virtual LoaiCong? MaLoaiCongNavigation { get; set; }

    public virtual NhanVien? MaNhanVienNavigation { get; set; }
}
