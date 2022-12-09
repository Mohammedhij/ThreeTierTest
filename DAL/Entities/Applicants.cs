using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Applicants
    {
        public Int64 Applicant_ID { get; set; }
        public string Applicant_Name { get; set; }
        public string Applicant_Surname { get; set; }

        public DateTime Applicant_BirthDate { get; set; }
        public String Contact_Email { get; set; }
        public String Contact_Number { get; set;}
        public DateTime applicant_creationDate { get; set; }
        public DateTime applicant_ModifiedDate { get;}


        public ICollection<Application> Applications { get; set; }
    }
    }

