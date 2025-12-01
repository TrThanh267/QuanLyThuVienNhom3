using System;
using System.Collections.Generic;

namespace QuanLyThuVienNhom3.Models;

public partial class PhieuMuon
{
    public int MaPhieuMuon { get; set; }

    public DateOnly? ThoiGianMuon { get; set; }

    public DateOnly? ThoihanTra { get; set; }

    public int? SoLuong { get; set; }

    public string? TrangThai { get; set; }

    public int? MaNhanVien { get; set; }

    public int? MaDocGia { get; set; }

    public int? SoLuongBanDau { get; set; }

    public virtual ICollection<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; } = new List<ChiTietPhieuMuon>();

    public virtual DocGium? MaDocGiaNavigation { get; set; }

    public virtual NhanVien? MaNhanVienNavigation { get; set; }
}
