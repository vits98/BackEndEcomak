using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Data.Entities
{
    public class ProductEntity
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }
        public string Fabric { get; set; }
        public string Design { get; set; }
        public string Quantity { get; set; }
        public string Size { get; set; }
        public string Handle { get; set; }
        public string Photo { get; set; }
        [ForeignKey("CategoryId")]
        public virtual CategoryEntity Category { get; set; }
        public virtual IEnumerable<QuoteEntity> Quotes { get; set; }
    }   
}