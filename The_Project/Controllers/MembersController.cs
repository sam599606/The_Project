using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using The_Project.Data;
using The_Project.Models;
using The_Project.Services;

namespace The_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly DataContext _context;
        public MembersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("GetMember/{account}")]
        public async Task<IActionResult> GetMember([FromRoute] string account)
        {
            var user = await _context.Members.FirstOrDefaultAsync(m => m.Account == account);

            if (user == null)
            {
                return NotFound("找不到該帳號的會員資訊");
            }

            return Ok(user);
        }


        #region 註冊
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(Member member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var membersDBService = new MembersDBService();
            var mailService = new MailService();

            // 檢查帳號是否重複
            var existingMember = await _context.Members.FirstOrDefaultAsync(m => m.Account == member.Account);
            if (existingMember != null)
            {
                return BadRequest("此帳號已被註冊");
            }

            // 將密碼進行哈希處理
            string hashedPassword = membersDBService.HashPassword(member.Password);

            // 從 MailService 中獲取驗證碼
            var validationCode = mailService.GetValidateCode();

            // 使用從 MailService 中獲取的驗證碼和哈希後的密碼
            Member insert = new Member
            {
                Account = member.Account,
                Password = hashedPassword,
                Email = member.Email,
                AuthCode = validationCode,
                Role = 1,
                IsDeleted = false
            };

            // 組合驗證連結的URL，這裡使用 Url.Action 來生成 URL
            string verifyUrl = $"{Request.Scheme}://{Request.Host}/api/Members/EmailValidate?Account={insert.Account}&AuthCode={insert.AuthCode}";

            // 讀取郵件模板
            string tempMail = await System.IO.File.ReadAllTextAsync("MailBody/RegisterEmailTemplate.html");

            // 將驗證碼插入郵件模板中
            string mailBody = mailService.GetRegisterMailBody(tempMail, member.Account, verifyUrl);

            // 設定郵件標題
            string mailSubject = "會員註冊確認信";

            // 寄送郵件
            mailService.SendMail(mailBody, member.Email, mailSubject);

            _context.Members.Add(insert);
            await _context.SaveChangesAsync();
            return Ok("會員註冊成功，請至Email收信");
        }
        #endregion

        #region Email驗證
        [HttpGet]
        [Route("EmailValidate")]
        public IActionResult EmailValidate(string Account, string AuthCode)
        {
            // 根據 Account 從資料庫中獲取使用者資訊
            var user = _context.Members.FirstOrDefault(m => m.Account == Account);

            // 驗證使用者是否存在
            if (user == null)
            {
                return NotFound("使用者不存在");
            }

            // 驗證 AuthCode 是否正確
            if (user.AuthCode != AuthCode)
            {
                return BadRequest("驗證碼錯誤");
            }

            // 更新資料庫中的使用者資訊，重設 AuthCode
            user.AuthCode = "YES"; // 或者設定為空字串，視您的資料庫設計而定
            _context.SaveChanges();

            return Ok("驗證成功");
        }
        #endregion

        #region 忘記密碼
        [HttpPut("forgotpassword")]
        public async Task<IActionResult> ForgotPassword(string account)
        {
            var membersDBService = new MembersDBService();
            var mailService = new MailService();

            // 呼叫 GetMember 方法以獲取使用者資訊
            var user = await _context.Members.FirstOrDefaultAsync(m => m.Account == account);

            // 如果用戶不存在，返回 BadRequest
            if (user == null)
            {
                return NotFound("帳號不存在");
            }

            if (user.Email == null)
            {
                return BadRequest("未綁定郵箱");
            }

            // 生成新密碼
            var newPassword = membersDBService.GenerateRandomPassword();

            // 更新用戶密碼
            user.Password = membersDBService.HashPassword(newPassword);
            await _context.SaveChangesAsync();

            // 讀取郵件模板
            string emailTemplatePath = "MailBody/ForgotPassword.html";
            string emailTemplate = await System.IO.File.ReadAllTextAsync(emailTemplatePath);

            // 替換模板中的標記
            emailTemplate = emailTemplate.Replace("{{UserName}}", user.Account);
            emailTemplate = emailTemplate.Replace("{{NewPassword}}", newPassword);

            string mailSubject = "會員密碼";

            // 寄送郵件
            mailService.SendMail(emailTemplate, user.Email, mailSubject);

            return Ok("已將新密碼發送至郵箱");
        }
        #endregion


        #region 修改密碼
        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePassword(string account, string OldPassword, string NewPassword)
        {
            // 驗證用戶身份
            var user = await _context.Members.FirstOrDefaultAsync(m => m.Account == account);
            if (user == null)
            {
                return NotFound("帳號不存在");
            }

            var membersDBService = new MembersDBService();
            if (!membersDBService.VerifyPassword(OldPassword, user.Password))
            {
                return BadRequest("舊密碼錯誤");
            }

            // 更新密碼
            user.Password = membersDBService.HashPassword(NewPassword);
            await _context.SaveChangesAsync();

            return Ok("密碼已成功修改");
        }
        #endregion

        #region 刪除帳號
        [HttpPut("deleteaccount/{account}")]
        public async Task<IActionResult> DeleteAccount(string account)
        {
            // 查找要刪除的用戶
            var user = await _context.Members.FirstOrDefaultAsync(m => m.Account == account);
            if (user == null)
            {
                return NotFound("找不到該帳號的資訊");
            }

            // 將 IsDeleted 設置為 true
            user.IsDeleted = true;

            // 保存到數據庫
            await _context.SaveChangesAsync();

            return Ok($"帳號 {account} 已成功刪除");
        }

        #endregion

        // #region 登入
        // public ActionResult Login()
        // {
        //     if (User.Identity.IsAuthenticated)
        //         return RedirectToAction("Index", "Guestbooks");
        //     return View();
        // }
        // [HttpPost]
        // public ActionResult Login(MembersLoginViewModel LoginMember)
        // {
        //     string ValidateStr = membersService.LoginCheck(LoginMember.Account, LoginMember.Password);
        //     if (string.IsNullOrEmpty(ValidateStr))
        //     {
        //         string RoleData = membersService.GetRole(LoginMember.Account);
        //         JwtService jwtService = new JwtService();
        //         string cookieName = WebConfigurationManager.AppSettings["CookieName"].ToString();
        //         string Token = jwtService.GenerateToken(LoginMember.Account, RoleData);
        //         HttpCookie cookie = new HttpCookie(cookieName);
        //         cookie.Value = Server.UrlEncode(Token);
        //         Response.Cookies.Add(cookie);
        //         Response.Cookies[cookieName].Expires = DateTime.Now.AddMinutes(Convert.ToInt32(WebConfigurationManager.AppSettings["ExpireMinutes"]));
        //         return RedirectToAction("Index", "Guestbooks");
        //     }
        //     else
        //     {
        //         ModelState.AddModelError("", ValidateStr);
        //         return View(LoginMember);
        //     }
        // }
        // #endregion

    }
}