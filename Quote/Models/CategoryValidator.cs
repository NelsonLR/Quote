using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quote.Models
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("The name field is required");
            RuleFor(x => x.Name).Must(UniqueName).WithMessage("Category Exists!");
        }

        private bool UniqueName(string name)
        {
            QuotationContext cdb = new QuotationContext();
            if (cdb.Categories.SingleOrDefault(x => x.Name == name) == null)
                return true;
            else
                return false;
        }
    }
}