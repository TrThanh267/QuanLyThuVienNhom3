using System;
using System.Collections.Generic;

namespace QuanLyThuVienNhom3.Models;

public partial class Sach
{
    public int MaSach { get; set; }

    public string? TenSach { get; set; }

    public int? SoLuong { get; set; }

    public string? TacGia { get; set; }

    public string? TrangThai { get; set; }

    public int? MaNhaSanXuat { get; set; }

    public int? MaLoaiSach { get; set; }

    public virtual ICollection<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; } = new List<ChiTietPhieuMuon>();

    public virtual LoaiSach? MaLoaiSachNavigation { get; set; }

    public virtual NhaSanXuat? MaNhaSanXuatNavigation { get; set; }
}
