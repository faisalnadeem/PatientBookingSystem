using PDR.PatientBooking.Service.ClinicServices.Requests;
using PDR.PatientBooking.Service.ClinicServices.Responses;

namespace PDR.PatientBooking.Service.ClinicServices
{
    public interface IClinicService
    {
        void AddClinic(AddClinicRequest request);
        GetAllClinicsResponse GetAllClinics();
    }
}