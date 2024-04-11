using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using The_Project.Data;
using System.Text;
using The_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace The_Project.Services
{
    public class MembersDBService
    {
        private readonly DataContext _context;

        #region 密碼
        public string HashPassword(string Password)
        {
            string saltkey = "1q2w3e4r5t6y7u8ui9o0po7tyy";
            string saltAndPassword = string.Concat(Password, saltkey);
            SHA256 sha256Hasher = SHA256.Create();
            byte[] PasswordData = Encoding.Default.GetBytes(saltAndPassword);
            byte[] HashDate = sha256Hasher.ComputeHash(PasswordData);
            string Hashresult = Convert.ToBase64String(HashDate);
            return Hashresult;
        }
        #endregion

        // public string EmailValidate(string Account, string AuthCode)
        // {
        //     // 根據 Account 從資料庫中獲取使用者資訊
        //     var user = _context.Members.FirstOrDefault(m => m.Account == Account);

        //     // 驗證使用者是否存在
        //     if (user == null)
        //     {
        //         return "使用者不存在";
        //     }

        //     // 驗證 AuthCode 是否正確
        //     if (user.AuthCode != AuthCode)
        //     {
        //         return "驗證碼錯誤";
        //     }

        //     // 更新資料庫中的使用者資訊，清空 AuthCode
        //     user.AuthCode = null; // 或者設定為空字串，視您的資料庫設計而定
        //     _context.SaveChanges();

        //     return "success";
        // }

        #region 生成密碼
        public string GenerateRandomPassword()
        {
            string[] Code = {"A", "B", "C", "D", "E", "F", "G", "H", "I","J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
        "a", "b", "c", "d", "e", "f", "g", "h", "i","j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" ,
        "1", "2", "3", "4", "5", "6", "7", "8", "9"};

            string password = string.Empty;
            Random rd = new Random();
            for (int i = 0; i < 10; i++)
            {
                password += Code[rd.Next(Code.Length)];
            }
            return password;
        }
        #endregion

        #region 比對新舊密碼
        public bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            // 對用戶輸入的密碼進行哈希處理
            string hashedInputPassword = HashPassword(inputPassword);

            // 比較兩個哈希值是否相等
            return hashedInputPassword.Equals(hashedPassword);
        }
        #endregion

        #region 登入確認
        public string LoginCheck(string account, string password)
        {
            var user = _context.Members.FirstOrDefault(m => m.Account == account);

            if (user != null)
            {
                if (string.IsNullOrWhiteSpace(user.AuthCode))
                {
                    if (PasswordCheck(user, password))
                    {
                        return "";
                    }
                    else
                    {
                        return "密碼輸入錯誤";
                    }
                }
                else
                {
                    return "此帳號未經過Email驗證，請去收信";
                }
            }
            else
            {
                return "無此會員帳號，請去註冊";
            }
        }
        #endregion

        #region 密碼確認
        private bool PasswordCheck(Member checkMember, string password)
        {
            bool result = checkMember.Password.Equals(HashPassword(password));
            return result;
        }
        #endregion

        #region 取得角色
        // 取得會員的權限角色資料
        public string GetRole(string account)
        {
            // 宣告初始角色字串
            string Role = "User";
            // 取得傳入帳號的會員資料
            var LoginMember = _context.Members.FirstOrDefault(m => m.Account == account);

            if(LoginMember == null)
            {
                return "";
            }

            if (LoginMember.Role == 2)
            {
                Role = "Member";
            }
            else if(LoginMember.Role == 3)
            {
                Role = "Admin";
            }
            
            // 回傳最後結果
            return Role;
        }
        #endregion
    }
}