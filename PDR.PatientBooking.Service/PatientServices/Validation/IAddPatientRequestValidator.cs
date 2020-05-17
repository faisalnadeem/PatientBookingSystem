using PDR.PatientBooking.Service.PatientServices.Requests;
using PDR.PatientBooking.Service.Validation;

namespace PDR.PatientBooking.Service.PatientServices.Validation
{
    public interface IAddPatientRequestValidator
    {
        PdrValidationResult ValidateRequest(AddPatientRequest request);
    }
}