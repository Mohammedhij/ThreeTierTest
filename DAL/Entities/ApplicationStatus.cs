using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class ApplicationStatus
    {
        public Int64 ApplicationStatus_ID { get; set; }
        public string ApplicationStatus_Name { get;set; }
        public DateTime ApplicationStatus_CreationDate { get; set; }
        public DateTime ApplicationStatus_ModifiedDate { get; set; }

        public ICollection<Application> Applications { get; set; }
    }
}
