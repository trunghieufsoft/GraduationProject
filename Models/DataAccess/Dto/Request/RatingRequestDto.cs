namespace Models.DataAccess
{
    public class RatingRequestDto : Auditable
    {
        public string RatID { get; set; }
        
        public int ProdID { get; set; }
        
        public string Content { get; set; }
        
        public double Level { get; set; }
        
        public string UserID { get; set; }
    }
}
