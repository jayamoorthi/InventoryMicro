namespace kafka.ProductApi.Models
{
    public class InventoryStockRequest
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
