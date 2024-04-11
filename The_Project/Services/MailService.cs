using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace The_Project.Services
{
    public class MailService
    {
        private string gmail_account = "sam599606@gmail.com";
        private string gmail_password = "qfnw fjvn ujre undh";
        private string gmail_mail = "sam599606@gmail.com";

        public string GetValidateCode()
        {
            string[] Code ={ "A", "B", "C", "D", "E", "F", "G", "H", "I",
                "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U",
                "V", "W", "X", "Y", "Z", "1", "2", "3", "4", "5", "6",
                "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h",
                "i", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t",
                "u", "v", "w", "x", "y", "z" };
            string ValidateCode = string.Empty;
            Random rd = new Random();
            for (int i = 0; i < 10; i++)
            {
                ValidateCode += Code[rd.Next(Code.Count())];
            }
            return ValidateCode;
        }

        #region 註冊會員郵件範本
        public string GetRegisterMailBody(string TempString, string UserName, string ValidateUrl)
        {
            TempString = TempString.Replace("{{UserName}}", UserName);
            TempString = TempString.Replace("{{ValidateUrl}}", ValidateUrl);
            return TempString;
        }
        #endregion

        #region 寄會員驗證信
        public void SendMail(string MailBody, string ToEmail, string mailSubject)
        {
            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            smtpServer.Port = 587;
            smtpServer.Credentials = new System.Net.NetworkCredential(gmail_account, gmail_password);
            smtpServer.EnableSsl = true;
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(gmail_mail);
            mail.To.Add(ToEmail);
            mail.Subject = mailSubject;
            mail.Body = MailBody;
            mail.IsBodyHtml = true;
            smtpServer.Send(mail);
        }
        #endregion
    }
}