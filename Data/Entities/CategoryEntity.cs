using Ecomak.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Data.Entities
{
    public class CategoryEntity
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual IEnumerable<ProductEntity> Products { get; set; }
        public virtual IEnumerable<TrEntity> Trs { get; set; }
    }
}
