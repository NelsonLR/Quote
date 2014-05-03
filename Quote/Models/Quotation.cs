using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quote.Models
{
    public class Quotation
    {
        public int QuotationID { get; set; }
        [Required]
        public String Quote { get; set; }
        [Required]
        public String Author { get; set; }
        public DateTime? Date { get; set; }

        public virtual Category Category { get; set; }
        public int CategoryID { get; set; }
    }
}