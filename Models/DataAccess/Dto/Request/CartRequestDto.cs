namespace Models.DataAccess.Dto.Request
{
    public class CartRequestDto
    {
        public int ProdId { get; set; }

        public string ProdName { get; set; }

        public string Code { get; set; }

        public string ImageUrl { get; set; }

        public int Amount { get; set; }
    }
}