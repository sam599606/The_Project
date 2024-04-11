using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YourProject.Models
{
    public class UserAnswer
    {
        [DisplayName("使用者答案ID")] //主鍵、自動遞增
        [Key]
        public int UserAnsID { get; set; }

        [DisplayName("題目ID")] //外來鍵，關聯至Question資料表的主鍵
        [Required]
        public int QuestionID { get; set; }

        [DisplayName("帳號")] //外來鍵，關聯至Member資料表的主鍵
        [Required]
        [MaxLength(30)]
        public string Account { get; set; } = string.Empty;

        [DisplayName("使用者答案")]
        [Required]
        [MaxLength(1)]
        public string Useranswer { get; set; } = string.Empty;
    }
}
