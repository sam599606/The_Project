using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YourProject.Models
{
    public class BookSave
    {
        [DisplayName("訂閱ID")] //主鍵兼外來鍵，關聯至Book的主鍵
        [Key]
        public int BookID { get; set; }

        [DisplayName("帳號")] //主鍵兼外來鍵，關聯至Member的主鍵
        [Required]
        [MaxLength(30)]
        public string Account { get; set; } = string.Empty;
    }
}
