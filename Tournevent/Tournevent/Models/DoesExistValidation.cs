using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tournevent.Models
{
    public class DoesExistValidation : ValidationAttribute
    {
        private readonly UserContext _dbContext = new UserContext();

        public override bool IsValid(object value)
        {
            string email = value.ToString();
            var userWithEmail = _dbContext.Users.Where(u => u.Email == email).ToList();
            return userWithEmail.Count() == 0;
        }
    }
}