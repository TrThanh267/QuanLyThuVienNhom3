using QuanLyThuVienNhom3.DAL;
using QuanLyThuVienNhom3.DTO;
using QuanLyThuVienNhom3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVienNhom3.BLL
{
    internal class QLTaiKhoan_BLL
    {
        private readonly QLTaiKhoan_DAL thuVien = new QLTaiKhoan_DAL();
        private readonly ThuVienNhom3Context _context = new ThuVienNhom3Context();
        public List<QLTaiKhoan_DTO> GetLTaiKhoan()
        {
            return thuVien.GetListTaiKhoan().ToList();

        }

        public bool ThemTaiKhoan(TaiKhoan taiKhoan)
        {
            try
            {

                if (thuVien.KiemTraTenDaTonTai(taiKhoan.TenTaiKhoan.Trim()))
                {
                    MessageBox.Show("Tên tài khoản đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // 2. Băm mật khẩu trước
                string hashPass = BCrypt.Net.BCrypt.HashPassword(taiKhoan.MatKhauHash.Trim());



                // 4. Tạo tài khoản mới
                var tkMoi = new TaiKhoan
                {
                    TenTaiKhoan = taiKhoan.TenTaiKhoan.Trim(),
                    MatKhauHash = hashPass,
                    MaVaiTro = taiKhoan.MaVaiTro,
                    TrangThai = taiKhoan.TrangThai ?? "Hoạt động"
                };

                return thuVien.ThemTk(tkMoi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
                return false;
            }
        }

        // BLL
        public bool XoaTaiKhoan(int maTK, int? maTKDangDangNhap = null)
        {
            try
            {
                var tk = thuVien.LayTaiKhoanTheoMa(maTK);
                if (tk == null) return false;

                // Không cho xóa tài khoản đang đăng nhập
                if (maTKDangDangNhap.HasValue && maTK == maTKDangDangNhap.Value)
                {
                    MessageBox.Show("Không thể xóa tài khoản đang đăng nhập!", "Cảnh báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Không cho xóa tài khoản admin cuối cùng (tuỳ dự án)
                if (tk.MaVaiTroNavigation.TenVaiTro == "Quản lý" && _context.TaiKhoans.Count(x => x.MaVaiTroNavigation.TenVaiTro == "Quản lý") == 1)
                {
                    MessageBox.Show("Phải có ít nhất 1 tài khoản Quản lý!", "Cảnh báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }

                if (MessageBox.Show($"Bạn có chắc chắn muốn xóa tài khoản \"{tk.TenTaiKhoan}\"?",
                                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.No)
                {
                    return false;
                }

                return thuVien.XoaTK(maTK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không xác định: " + ex.Message);
                return false;
            }
        }

        public List<QLTaiKhoan_DTO> TimkiemTK(string keyword)
        {
            return thuVien.TimKiemTaiKhoan(keyword);
        }

        public List<QLTaiKhoan_DTO> LocTKtheoVT(int maVT)
        {
            if (maVT == 1)
            {
                return thuVien.LocAddmin();
            }
            else if (maVT == 2)
            {
                return thuVien.LocNhanVien();
            }
            else
            {
                return thuVien.GetListTaiKhoan();
            }
        }
        public TaiKhoan LayTK(int matk)
        {
            return thuVien.LayTaiKhoanTheoMa(matk);
        }
        public bool KiemTraTrungTen(string name)
        {
            return thuVien.KiemTraTenDaTonTai(name);
        }

        public bool CapNhat(TaiKhoan taiKhoan)
        {
            try
            {
                if (taiKhoan == null || taiKhoan.MaTaiKhoan <= 0)
                    return false;

                // Lấy tài khoản hiện tại từ DB để kiểm tra tên trùng
                var tkHienTai = thuVien.LayTaiKhoanTheoMa(taiKhoan.MaTaiKhoan);
                if (tkHienTai == null) return false;

                // Kiểm tra tên trùng (trừ chính nó)
                if (!string.IsNullOrWhiteSpace(taiKhoan.TenTaiKhoan))
                {
                    string tenMoi = taiKhoan.TenTaiKhoan.Trim();
                    if (tkHienTai.TenTaiKhoan != tenMoi && thuVien.KiemTraTenDaTonTai(tenMoi))
                    {
                        MessageBox.Show("Tên tài khoản đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    taiKhoan.TenTaiKhoan = tenMoi;
                }

                // === QUAN TRỌNG NHẤT: XỬ LÝ MẬT KHẨU ===
                if (!string.IsNullOrWhiteSpace(taiKhoan.MatKhauHash)) // người dùng NHẬP MẬT KHẨU MỚI
                {
                    // Băm mật khẩu mới
                    taiKhoan.MatKhauHash = BCrypt.Net.BCrypt.HashPassword(taiKhoan.MatKhauHash.Trim());
                }
                else
                {
                    // Không nhập → giữ nguyên hash cũ
                    taiKhoan.MatKhauHash = tkHienTai.MatKhauHash;
                }

                // Gọi DAL
                return thuVien.CapNhatTK(taiKhoan);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi BLL: " + ex.Message);
                return false;
            }
        }
    }
}
