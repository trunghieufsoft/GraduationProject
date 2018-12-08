using System;
using System.ComponentModel.DataAnnotations;

namespace Models.DataAccess
{
    public class Auditable
    {
        [Display(Name ="Ngày tạo")]
        public string CreatedAt { get; set; }

        [Display(Name = "Ngày sửa")]
        public string UpdatedAt { get; set; }
    }
}