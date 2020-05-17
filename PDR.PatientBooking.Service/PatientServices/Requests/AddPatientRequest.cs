using PDR.PatientBooking.Service.Common.Requests;

namespace PDR.PatientBooking.Service.PatientServices.Requests
{
    public class AddPatientRequest : BasePersonRequest
    {
        public long ClinicId { get; set; }
    }
}
