using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Data.Entities
{
    public class ImageEntity
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string Type { get; set; }
        public string Origin { get; set; }
        public string Name { get; set; }
    }
}
