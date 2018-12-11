using System.ComponentModel.DataAnnotations;

namespace Models.DataAccess
{
    public class CommentRequestDto : Auditable
    {
        [Display(Name = "Mã Bình luận")]
        public string ComID { get; set; }

        [Display(Name = "Nội dung")]
        public string Content { get; set; }

        [Display(Name = "Mã khách hàng")]
        public string UserID { get; set; }

        public int ProdId { get; set; }
    }
}
