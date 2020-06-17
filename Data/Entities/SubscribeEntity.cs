using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Data.Entities
{
    public class SubscribeEntity
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Company { get; set; }
        [Required]
        public string Email { get; set; }
        public int Phone { get; set; }
        public string Message { get; set; }

    }
}
