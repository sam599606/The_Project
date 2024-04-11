using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YourProject.Models
{
    public class Class
    {
        [DisplayName("課程ID")] //主鍵、自動遞增
        [Key]
        public int ClassID { get; set; }

        [DisplayName("帳號")] //主鍵兼外來鍵，關聯至Member的主鍵
        [Required]
        [StringLength(30)]
        public string Account { get; set; } = string.Empty;

        [DisplayName("科目")] //1 2 3 4 5 國英數自社
        [Required]
        public int Type { get; set; }

        [DisplayName("課程價錢")]
        [Required]
        public int Price { get; set; }

        [DisplayName("課程介紹")]
        [Required]
        public string Content { get; set; } = string.Empty;

        [DisplayName("課程影片")]
        [Required]
        public string Video { get; set; } = string.Empty;

        [DisplayName("課程年分")]
        [Required]
        public int Year { get; set; }

        [DisplayName("新增時間")]
        [Required]
        public DateTime CreateTime { get; set; }

        [DisplayName("修改時間")]
        public DateTime? EditTime { get; set; }

        [DisplayName("隱藏時間")]
        public DateTime? DeleteTime { get; set; }
    }
}
