namespace ApiTask.DTOs
{
    public class OrderDTO
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string Carts { get; set; }
        public decimal Amount { get; set; }
        public bool Status { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}
