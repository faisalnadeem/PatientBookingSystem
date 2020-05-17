using PDR.PatientBooking.Service.DoctorServices.Requests;
using PDR.PatientBooking.Service.Validation;

namespace PDR.PatientBooking.Service.DoctorServices.Validation
{
    public interface IAddDoctorRequestValidator
    {
        PdrValidationResult ValidateRequest(AddDoctorRequest request);
    }
}