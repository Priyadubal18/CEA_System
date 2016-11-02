using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CEA_System.Models
{
    public class SchoolCompare
        
    {
        public int SchoolId { get; set; }
        public string AddressSchool { get; set; }
        public string City{ get; set; }
        public string SchoolCode { get; set; }
        public string WebSite { get; set; }
        public string SchoolType { get; set; }
        public string TutionFee { get; set; }
        public string GraduateRate { get; set; }
        public string RetentionRate { get; set; }
        public string BestProgram{ get; set; }
        public string ReviewLink { get; set; }
        public string AcceptanceRate{ get; set; }
        public string Comment { get; set; }
        public string EmailAddress { get; set; }
        public string CourseAvailable { get; set; }
        public string SchoolName { get; set; }
    }
}