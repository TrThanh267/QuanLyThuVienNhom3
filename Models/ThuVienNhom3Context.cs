using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuanLyThuVienNhom3.Models;

public partial class ThuVienNhom3Context : DbContext
{
    public ThuVienNhom3Context()
    {
    }

    public ThuVienNhom3Context(DbContextOptions<ThuVienNhom3Context> options)
        : base(options)
    {
    }

    public virtual DbSet<CaLam> CaLams { get; set; }

    public virtual DbSet<ChamCong> ChamCongs { get; set; }

    public virtual DbSet<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; }

    public virtual DbSet<DocGium> DocGia { get; set; }

    public virtual DbSet<LoaiCong> LoaiCongs { get; set; }

    public virtual DbSet<LoaiSach> LoaiSaches { get; set; }

    public virtual DbSet<NhaSanXuat> NhaSanXuats { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<PhieuMuon> PhieuMuons { get; set; }

    public virtual DbSet<Sach> Saches { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=TROUBLE\\SQLEXPRESS03;Database=ThuVienNhom3;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CaLam>(entity =>
        {
            entity.HasKey(e => e.MaCaLam).HasName("PK__CaLam__662C15A2A2A5DBA1");

            entity.ToTable("CaLam");

            entity.Property(e => e.TenCaLam).HasMaxLength(100);
        });

        modelBuilder.Entity<ChamCong>(entity =>
        {
            entity.HasKey(e => e.MaChamCong).HasName("PK__ChamCong__307331A15384BCE2");

            entity.ToTable("ChamCong");

            entity.HasOne(d => d.MaCaLamNavigation).WithMany(p => p.ChamCongs)
                .HasForeignKey(d => d.MaCaLam)
                .HasConstraintName("FK__ChamCong__MaCaLa__68487DD7");

            entity.HasOne(d => d.MaLoaiCongNavigation).WithMany(p => p.ChamCongs)
                .HasForeignKey(d => d.MaLoaiCong)
                .HasConstraintName("FK__ChamCong__MaLoai__6754599E");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.ChamCongs)
                .HasForeignKey(d => d.MaNhanVien)
                .HasConstraintName("FK__ChamCong__MaNhan__693CA210");
        });

        modelBuilder.Entity<ChiTietPhieuMuon>(entity =>
        {
            entity.HasKey(e => e.MaChiTietPhieuMuon).HasName("PK__ChiTietP__811679095AD22CA9");

            entity.ToTable("ChiTietPhieuMuon");

            entity.Property(e => e.NgayTraSach).HasColumnType("datetime");
            entity.Property(e => e.TinhTrangSach).HasMaxLength(100);

            entity.HasOne(d => d.MaPhieuMuonNavigation).WithMany(p => p.ChiTietPhieuMuons)
                .HasForeignKey(d => d.MaPhieuMuon)
                .HasConstraintName("FK__ChiTietPh__MaPhi__60A75C0F");

            entity.HasOne(d => d.MaSachNavigation).WithMany(p => p.ChiTietPhieuMuons)
                .HasForeignKey(d => d.MaSach)
                .HasConstraintName("FK__ChiTietPh__MaSac__5FB337D6");
        });

        modelBuilder.Entity<DocGium>(entity =>
        {
            entity.HasKey(e => e.MaDocGia).HasName("PK__DocGia__F165F94598F211DC");

            entity.Property(e => e.DiaChi).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.GioiTinh).HasMaxLength(50);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TenDocGia).HasMaxLength(100);
            entity.Property(e => e.TrangThai).HasMaxLength(50);
        });

        modelBuilder.Entity<LoaiCong>(entity =>
        {
            entity.HasKey(e => e.MaLoaiCong).HasName("PK__LoaiCong__7A74B2D7519B99A8");

            entity.ToTable("LoaiCong");

            entity.Property(e => e.TenloaiCong).HasMaxLength(100);
        });

        modelBuilder.Entity<LoaiSach>(entity =>
        {
            entity.HasKey(e => e.MaLoaiSach).HasName("PK__LoaiSach__2F9B373FDCC01592");

            entity.ToTable("LoaiSach");

            entity.Property(e => e.TenLoaiSach).HasMaxLength(50);
        });

        modelBuilder.Entity<NhaSanXuat>(entity =>
        {
            entity.HasKey(e => e.MaNhaSanXuat).HasName("PK__NhaSanXu__838C17A1401A4B80");

            entity.ToTable("NhaSanXuat");

            entity.Property(e => e.TenNhaSanXuat).HasMaxLength(50);
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNhanVien).HasName("PK__NhanVien__77B2CA4732A95C95");

            entity.ToTable("NhanVien");

            entity.HasIndex(e => e.MaTaiKhoan, "UQ__NhanVien__AD7C6528B34EFA15").IsUnique();

            entity.Property(e => e.DiaChi)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.GioiTinh).HasMaxLength(50);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TenNhanVien).HasMaxLength(100);
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithOne(p => p.NhanVien)
                .HasForeignKey<NhanVien>(d => d.MaTaiKhoan)
                .HasConstraintName("FK__NhanVien__MaTaiK__571DF1D5");
        });

        modelBuilder.Entity<PhieuMuon>(entity =>
        {
            entity.HasKey(e => e.MaPhieuMuon).HasName("PK__PhieuMuo__C4C822228B4DDA2B");

            entity.ToTable("PhieuMuon");

            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaDocGiaNavigation).WithMany(p => p.PhieuMuons)
                .HasForeignKey(d => d.MaDocGia)
                .HasConstraintName("FK__PhieuMuon__MaDoc__5CD6CB2B");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.PhieuMuons)
                .HasForeignKey(d => d.MaNhanVien)
                .HasConstraintName("FK__PhieuMuon__MaNha__5BE2A6F2");
        });

        modelBuilder.Entity<Sach>(entity =>
        {
            entity.HasKey(e => e.MaSach).HasName("PK__Sach__B235742DAECDB767");

            entity.ToTable("Sach");

            entity.Property(e => e.TacGia).HasMaxLength(50);
            entity.Property(e => e.TenSach).HasMaxLength(100);
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaLoaiSachNavigation).WithMany(p => p.Saches)
                .HasForeignKey(d => d.MaLoaiSach)
                .HasConstraintName("FK__Sach__MaLoaiSach__4E88ABD4");

            entity.HasOne(d => d.MaNhaSanXuatNavigation).WithMany(p => p.Saches)
                .HasForeignKey(d => d.MaNhaSanXuat)
                .HasConstraintName("FK__Sach__MaNhaSanXu__4D94879B");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTaiKhoan).HasName("PK__TaiKhoan__AD7C65294E9F47D6");

            entity.ToTable("TaiKhoan");

            entity.Property(e => e.MatKhauHash)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TenTaiKhoan)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaVaiTroNavigation).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.MaVaiTro)
                .HasConstraintName("FK__TaiKhoan__MaVaiT__534D60F1");
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.HasKey(e => e.MaVaiTro).HasName("PK__VaiTro__C24C41CFAAAD5179");

            entity.ToTable("VaiTro");

            entity.Property(e => e.TenVaiTro).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
