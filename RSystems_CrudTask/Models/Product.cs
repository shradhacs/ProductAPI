using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace RSystems_CrudTask.Models
{
    public class Product
    {
        [Key]
        [JsonProperty]
        public int Id { get; set; }
        [Required]
        [JsonProperty][StringLength(50)]
        public string Name { get; set; }
        [Range(1, 1000)]
        [JsonProperty]
        public double Price { get; set; }
        [Required]
        [JsonProperty]
        public int Quantity { get; set; }
    }
}
