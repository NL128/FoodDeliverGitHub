using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class AddressInfo
    {
        [Required]
      public string opstina { get; set; }
        [Required]
        public  int postCode { get; set; }
        
        public  string restoran { get; set; }
        public  string kuhinje { get; set; }
        public int skip { get; set; }
    }
}
