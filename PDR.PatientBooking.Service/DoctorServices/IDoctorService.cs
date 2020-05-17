using PDR.PatientBooking.Service.DoctorServices.Requests;
using PDR.PatientBooking.Service.DoctorServices.Responses;

namespace PDR.PatientBooking.Service.DoctorServices
{
    public interface IDoctorService
    {
        void AddDoctor(AddDoctorRequest request);
        GetAllDoctorsResponse GetAllDoctors();
    }
}