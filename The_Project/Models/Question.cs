using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YourProject.Models
{
    public class Question
    {
        [DisplayName("題目ID")] //主鍵、自動遞增
        [Key]
        public int QuestionID { get; set; }

        [DisplayName("帳號")] //主鍵兼外來鍵，關聯至Member的主鍵
        [Required]
        [MaxLength(30)]
        public string Account { get; set; } = string.Empty;

        [DisplayName("科目")] //1 2 3 4 5 國英數自社
        [Required]
        public int Type { get; set; }

        [DisplayName("題號")]
        [Required]
        public int QuestionNum { get; set; }

        [DisplayName("題目內容")]
        [Required]
        public string Content { get; set; } = string.Empty;

        [DisplayName("題目圖片")] //路徑
        public string Image { get; set; } = string.Empty;

        [DisplayName("題目正解")] //A B C D 其中之一
        [Required]
        [MaxLength(1)]
        public string Answer { get; set; } = string.Empty;

        [DisplayName("題目解析")]
        public string Solution { get; set; } = string.Empty;

        [DisplayName("屬於哪年")]
        [Required]
        public int Year { get; set; }

        [DisplayName("新增時間")]
        [Required]
        public DateTime CreateTime { get; set; }

        [DisplayName("修改時間")]
        public DateTime? EditTime { get; set; }

        [DisplayName("是否被隱藏")] //預設0
        [Required]
        public bool IsDeleted { get; set; }
    }
}
