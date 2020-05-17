using PDR.PatientBooking.Service.ClinicServices.Requests;
using PDR.PatientBooking.Service.Common.Validation;

namespace PDR.PatientBooking.Service.ClinicServices.Validation
{
    public interface IAddClinicRequestValidator
    {
        PdrValidationResult ValidateRequest(AddClinicRequest request);
    }
}