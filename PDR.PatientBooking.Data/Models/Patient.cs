using System;
using System.Collections.Generic;

namespace PDR.PatientBooking.Data.Models
{
    public class Patient
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
        public string Email { get; set; }
        public DateTime Created { get; set; }
        public virtual long ClinicId { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual Clinic Clinic { get; set; }
    }
}
