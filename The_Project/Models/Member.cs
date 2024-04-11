using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace The_Project.Models
{
    public class Member
    {
        [DisplayName("帳號")] //主鍵
        [Key]
        [Required(ErrorMessage = "帳號是必填的")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "帳號長度必須在6到30個字符之間")]
        public string Account { get; set; } = string.Empty; 

        [DisplayName("密碼")] //因要操作Hash，故為MAX
        [Required(ErrorMessage = "密碼是必填的")]
        public string Password { get; set; } = string.Empty; 

        [DisplayName("電子信箱")]
        [Required(ErrorMessage = "電子郵件是必填的")]
        [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$", ErrorMessage = "請輸入有效的電子郵件地址")]
        public string Email { get; set; } = string.Empty;

        [DisplayName("信箱驗證碼")]
        [Required]
        public string AuthCode { get; set; } = string.Empty;

        [DisplayName("權限")] // 1: User, 2: Member, 3: Admin
        [Required]
        public int Role { get; set; } 

        [DisplayName("是否已刪除")] //預設0
        public bool IsDeleted { get; set; } 
    }
}