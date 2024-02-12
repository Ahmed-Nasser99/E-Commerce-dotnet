using Platform.Model;
using System.ComponentModel.DataAnnotations;

namespace Platform.Dtos
{
    public class AddProductViewModel
    {
        [Required]
        public string title { get; set; }
        public string description { get; set; }
        [Required]
        public IFormFile image { get; set; }
        [Required]
        public string quantity { get; set; }
        [Required]
        public decimal price { get; set; }
        [Required]
        public Guid subcategoryid { get; set; }
        [Required]
        public Guid brandid { get; set; }
    }
}
