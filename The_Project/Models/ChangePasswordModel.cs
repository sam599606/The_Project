using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace The_Project.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "請輸入舊密碼")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "請輸入新密碼")]
        public string NewPassword { get; set; } = string.Empty;
    }
}