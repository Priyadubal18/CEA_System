using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CEA_System.Models
{
    public class StudentProfile
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string SSN { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CountryofBirth { get; set; }
           
        public string AptStreet { get; set; }
       
        
    }
}