using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Models
{
    public class Commentary
    {
        public int? id { get; set; }
        [Required]
        public string author { get; set; }
        public string comment { get; set; }
        public DateTime time { get; set; }
        public bool show { get; set; }

        public int? promotionId { get; set; }

    }
}
