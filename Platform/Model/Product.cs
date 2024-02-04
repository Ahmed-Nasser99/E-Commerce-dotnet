namespace Platform.Model
{
    public class Product
    {
        public Guid id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public string quantity { get; set; }
        public decimal price { get; set; }
        public string subcategoryId { get; set; }
        public string brandId { get; set; }

    }
}
