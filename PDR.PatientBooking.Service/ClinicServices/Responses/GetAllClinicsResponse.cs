using PDR.PatientBooking.Data.Models;
using System.Collections.Generic;

namespace PDR.PatientBooking.Service.ClinicServices.Responses
{
    public class GetAllClinicsResponse
    {
        public List<Clinic> Clinics { get; set; }

        public class Clinic
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public SurgeryType SurgeryType { get; set; }
            public virtual IEnumerable<Patient> Patients { get; set; }
        }

        public class Patient
        {
            public long Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}
