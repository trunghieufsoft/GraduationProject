namespace Models.DataAccess
{
    public class RatingRequestDto
    {
        public string RatID { get; set; }
        
        public int ProdID { get; set; }
        
        public string Content { get; set; }
        
        public byte Level { get; set; }
        
        public string UserID { get; set; }
    }
}
