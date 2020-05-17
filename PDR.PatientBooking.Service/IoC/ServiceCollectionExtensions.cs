using Microsoft.Extensions.DependencyInjection;
using PDR.PatientBooking.Service.ClinicServices;
using PDR.PatientBooking.Service.ClinicServices.Validation;
using PDR.PatientBooking.Service.DoctorServices;
using PDR.PatientBooking.Service.DoctorServices.Validation;
using PDR.PatientBooking.Service.PatientServices;
using PDR.PatientBooking.Service.PatientServices.Validation;

namespace PDR.PatientBooking.Service.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterPatientBookingServices(this IServiceCollection collection)
        {
            collection.AddScoped<IPatientService, PatientService>();
            collection.AddScoped<IAddPatientRequestValidator, AddPatientRequestValidator>();

            collection.AddScoped<IDoctorService, DoctorService>();
            collection.AddScoped<IAddDoctorRequestValidator, AddDoctorRequestValidator>();

            collection.AddScoped<IClinicService, ClinicService>();
            collection.AddScoped<IAddClinicRequestValidator, AddClinicRequestValidator>();
        }
    }
}
