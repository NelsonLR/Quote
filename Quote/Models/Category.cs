using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentValidation.Mvc;

namespace Quote.Models
{
    [FluentValidation.Attributes.Validator(typeof(CategoryValidator))]
    public class Category
    {
        public int CategoryID { get; set; }
        
        //[Required]
        public String Name { get; set; }

        public virtual List<Quotation> Quotations { get; set; }
    }
}