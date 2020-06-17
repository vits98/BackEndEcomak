using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Data.Entities
{
    public class QuoteEntity
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string QuoteColor { get; set; }
        public int Quantity { get; set; }
        public string nameSub { get; set; }
        public string emailSub { get; set; }
        public string phonoSub { get; set; }
        public string enterpriseSub { get; set; }
        public string messageSub { get; set; }
        public string imagePath { get; set; }
        
        [ForeignKey("ProductId")]
        public virtual ProductEntity Product { get; set; }
        [ForeignKey("TRId")]
        public virtual TrEntity Tr { get; set; }
    }
}
