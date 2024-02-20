using Microsoft.AspNetCore.Identity;
using Platform.Model.Auth;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Platform.Model
{
   
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public string UserId { get; set; }
        public int Quantity { get; set; }

    }
}
