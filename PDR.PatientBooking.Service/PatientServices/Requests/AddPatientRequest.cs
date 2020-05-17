using PDR.PatientBooking.Service.Enums;
using System;

namespace PDR.PatientBooking.Service.PatientServices.Requests
{
    public class AddPatientRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public long ClinicId { get; set; }
    }
}
