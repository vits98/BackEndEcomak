using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecomak.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CantProducts { get; set; }
        public int CantTrs { get; set; }
        public IEnumerable<Product> products { get; set; }   
        public IEnumerable<Tr> trs { get; set; }

    }
}
