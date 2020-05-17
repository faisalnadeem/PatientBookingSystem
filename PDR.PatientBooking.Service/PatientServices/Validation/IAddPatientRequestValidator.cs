using PDR.PatientBooking.Service.Common.Validation;
using PDR.PatientBooking.Service.PatientServices.Requests;

namespace PDR.PatientBooking.Service.PatientServices.Validation
{
    public interface IAddPatientRequestValidator
    {
        PdrValidationResult ValidateRequest(AddPatientRequest request);
    }
}