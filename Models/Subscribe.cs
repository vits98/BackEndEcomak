using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Models
{
    public class Subscribe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public string Message { get; set; }
        public IEnumerable<Quote> quotes { get; set; }
    }
}
