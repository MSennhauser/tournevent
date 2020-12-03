using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public class DoesExistValidation : ValidationAttribute
    {
        private readonly UserContext _dbContext = new UserContext();

        public string Table { get; set; }
        public string Attribute { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int anzEmail = _dbContext.Database.ExecuteSqlCommand("Select COUNT(*) From " + Table + " WHERE " + Attribute + " = '" + value.ToString() + "'");
            if (anzEmail == 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Das " + validationContext.DisplayName +"-Attribut existiert bereits");
            }
        }
    }
}