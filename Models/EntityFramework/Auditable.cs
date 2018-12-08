using System;

namespace Models.EntityFramework
{
    public class Auditable
    {
        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}