using System;
using System.Collections.Generic;

namespace QuanLyThuVienNhom3.Models;

public partial class DocGium
{
    public int MaDocGia { get; set; }

    public string? TenDocGia { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? GioiTinh { get; set; }

    public byte[]? HinhAnh { get; set; }

    public string? DiaChi { get; set; }

    public string? SoDienThoai { get; set; }

    public string? Email { get; set; }

    public string? TrangThai { get; set; }

    public virtual ICollection<PhieuMuon> PhieuMuons { get; set; } = new List<PhieuMuon>();
}
