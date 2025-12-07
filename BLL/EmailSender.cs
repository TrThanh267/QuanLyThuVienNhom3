using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVienNhom3.BLL
{
    internal class EmailSender
    {
            /// <summary>
            /// Gửi email bất đồng bộ
            /// </summary>
            /// <param name="fromAddress">Email người gửi (Gmail)</param>
            /// <param name="toAddress">Email người nhận</param>
            /// <param name="subject">Tiêu đề email</param>
            /// <param name="body">Nội dung email (HTML được hỗ trợ)</param>
            /// <param name="smtpHost">SMTP server (ví dụ: smtp.gmail.com)</param>
            /// <param name="smtpPort">Port SMTP (ví dụ: 587)</param>
            /// <param name="username">Tên đăng nhập SMTP (email người gửi)</param>
            /// <param name="password">App Password Gmail 16 ký tự</param>
            public static async Task SendEmailAsync(
                string fromAddress,
                string toAddress,
                string subject,
                string body,
                string smtpHost,
                int smtpPort,
                string username,
                string password)
            {
                // Tạo MailMessage
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(fromAddress);
                    mail.To.Add(toAddress);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true; // Nếu nội dung có HTML

                    // Tạo SMTP client
                    using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort))
                    {
                        smtpClient.EnableSsl = true; // Bật SSL/TLS
                        smtpClient.UseDefaultCredentials = false; // Không dùng Windows Credentials
                        smtpClient.Credentials = new NetworkCredential(username, password);

                        // Gửi email bất đồng bộ
                        await smtpClient.SendMailAsync(mail);
                    }
                }
            }
        }
    }
