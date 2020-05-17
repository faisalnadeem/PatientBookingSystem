using PDR.PatientBooking.Service.Common.Validation;
using PDR.PatientBooking.Service.DoctorServices.Requests;

namespace PDR.PatientBooking.Service.DoctorServices.Validation
{
    public interface IAddDoctorRequestValidator
    {
        PdrValidationResult ValidateRequest(AddDoctorRequest request);
    }
}