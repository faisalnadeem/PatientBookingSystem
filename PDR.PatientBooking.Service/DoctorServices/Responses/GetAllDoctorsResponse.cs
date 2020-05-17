using PDR.PatientBooking.Service.Enums;
using System;
using System.Collections.Generic;

namespace PDR.PatientBooking.Service.DoctorServices.Responses
{
    public class GetAllDoctorsResponse
    {
        public List<Doctor> Doctors { get; set; }

        public class Doctor
        {
            public long Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime DateOfBirth { get; set; }
            public Gender Gender { get; set; }
            public string Email { get; set; }
        }
    }
}
