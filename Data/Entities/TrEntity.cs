using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Data.Entities
{
    public class TrEntity
    {
        [Key]
        [Required]
        public int IdTr { get; set; }
        [Required]
        public string TypeTr { get; set; }
        public string FabricTr { get; set; }
        public string DesignTr { get; set; }
        public string QuantityTr { get; set; }
        public string SizeTr { get; set; }
        public string HandleTr { get; set; }
        public string PhotoTr { get; set; }

        [ForeignKey("CategoryId")]
        public virtual CategoryEntity Category { get; set; }
        public virtual IEnumerable<QuoteEntity> Quotes { get; set; }

    }
}
