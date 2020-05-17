using PDR.PatientBooking.Service.Enums;
using System;
using System.Collections.Generic;

namespace PDR.PatientBooking.Service.PatientServices.Responses
{
    public class GetAllPatientsResponse
    {
        public List<Patient> Patients { get; set; }

        public class Patient
        {
            public long Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime DateOfBirth { get; set; }
            public Gender Gender { get; set; }
            public string Email { get; set; }
            public Clinic Clinic { get; set; }
        }

        public class Clinic
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }
    }
}
