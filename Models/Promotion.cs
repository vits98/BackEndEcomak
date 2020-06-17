using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ecomak.Models
{
    public class Promotion
    {
        public int? id { get; set; }
        [Required]
        public string tittle { get; set; }
        public string description { get; set; }
        public DateTime iniDate { get; set; }
        public DateTime endDate { get; set; }
        public string image { get; set; }

        public IEnumerable<Commentary> Comments { get; set; }
    }
}
