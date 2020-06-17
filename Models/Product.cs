using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Models
{
    public class Product
    {
        public int? Id { get; set; }
        [Required]
        public string Type { get; set; }
        public string Fabric { get; set; }
        public string Design { get; set; }
        public string Quantity { get; set; }
        public string Size { get; set; }
        public string Handle { get; set; }
        public string Photo { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<Quote> quotes { get; set; }

    }
}