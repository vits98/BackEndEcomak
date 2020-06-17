using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Models
{
    public class Quote
    {
        public int Id { get; set; }
        public string QuoteColor { get; set; }
        public int Quantity { get; set; }
        public string nameSub { get; set; }
        public string emailSub { get; set; }
        public string phonoSub { get; set; }
        public string enterpriseSub { get; set; }
        public string messageSub { get; set; }
        public string imagePath { get; set; }


        public int ProductId { get; set; }
        public int TRId { get; set; }
    }
}
