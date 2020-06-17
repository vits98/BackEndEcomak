using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Data.Entities
{
    public class CommentaryEntity
    {
        [Key]
        [Required]
        public int? id { get; set; }
        [Required]
        public string author { get; set; }
        public string comment { get; set; }
        public DateTime time { get; set; }
        public bool show { get; set; }

        [ForeignKey("promotionId")]
        public virtual PromotionEntity Promotion { get; set; }

    }
}
