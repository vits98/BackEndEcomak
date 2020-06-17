using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Data.Entities
{
    public class PromotionEntity
    {
        [Key]
        [Required]
        public int id { get; set; }
        [Required]
        public string tittle { get; set; }
        public string description { get; set; }
        public DateTime iniDate { get; set; }
        public DateTime endDate { get; set; }
        public string image { get; set; }

        public virtual ICollection<CommentaryEntity> Comments { get; set; }
    }
}
