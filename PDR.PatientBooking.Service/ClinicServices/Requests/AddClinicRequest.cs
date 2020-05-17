using PDR.PatientBooking.Data.Models;

namespace PDR.PatientBooking.Service.ClinicServices.Requests
{
    public class AddClinicRequest
    {
        public string Name { get; set; }
        public SurgeryType SurgeryType { get; set; }
    }
}
