namespace Models.DataAccess
{
    public class RepliesRequestDto : Auditable
    {
        public string ComID { get; set; }
        
        public int RepNo { get; set; }
        
        public string Content { get; set; }
        
        public string UserID { get; set; }
    }
}
