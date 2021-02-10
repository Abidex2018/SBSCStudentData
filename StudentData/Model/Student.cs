using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentData.Model
{
    public class Student
    {
        public int StudentId { get; set; }
        
        public string Name { get; set; }
        
        public string FamilyName { get; set; }
        
        public string Address { get; set; }
        public string CountryOfOrigin { get; set; }

        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        public int Age { get; set; }
        public bool Approved { get; set; }
        public bool IsValidEmail
        {
            get
            {
                return new EmailAddressAttribute().IsValid(this.EmailAddress);
            }
        }
    }
}
