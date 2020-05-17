using PDR.PatientBooking.Service.Enums;
using System;

namespace PDR.PatientBooking.Service.DoctorServices.Requests
{
    public class AddDoctorRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
    }
}
