using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Model
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public string quantity { get; set; }
        public decimal price { get; set; }
        public Guid subcategoryid { get; set; }
        public SubCategory subcategory { get; set; }
        public Guid brandid { get; set; }
        public Brand brand { get; set; }

    }
}
