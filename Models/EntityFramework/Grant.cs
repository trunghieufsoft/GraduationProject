using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.EntityFramework
{
    public class Grant : Auditable
    {


        public Grant()
        {
            Users = new List<User>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte GrantID { get; set; }

        [Required]
        [StringLength(150)]
        public string GrantName { get; set; }

        [Required]
        public bool isActive { get; set; }

        public ICollection<User> Users { get; set; }
    }
}