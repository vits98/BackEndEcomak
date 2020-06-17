using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Models
{
    public class Tr
    {
        public int? IdTr { get; set; }
        [Required]
        public string TypeTr { get; set; }
        public string FabricTr { get; set; }
        public string DesignTr { get; set; }
        public string QuantityTr { get; set; }
        public string SizeTr { get; set; }
        public string HandleTr { get; set; }
        public string PhotoTr { get; set; }
        public int CategoryId{ get; set; }
        public IEnumerable<Quote> quotes { get; set; }
    }
}
