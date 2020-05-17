using Microsoft.EntityFrameworkCore;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Data.Models;
using PDR.PatientBooking.Service.ClinicServices.Requests;
using PDR.PatientBooking.Service.ClinicServices.Responses;
using PDR.PatientBooking.Service.ClinicServices.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDR.PatientBooking.Service.ClinicServices
{
    public class ClinicService : IClinicService
    {
        private readonly PatientBookingContext _context;
        private readonly IAddClinicRequestValidator _validator;

        public ClinicService(PatientBookingContext context, IAddClinicRequestValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public void AddClinic(AddClinicRequest request)
        {
            var validationResult = _validator.ValidateRequest(request);

            if (!validationResult.PassedValidation)
            {
                throw new ArgumentException(validationResult.Errors.First());
            }

            _context.Clinic.Add(new Clinic
            {
                Name = request.Name,
                SurgeryType = request.SurgeryType,
                Patients = new List<Patient>()
            });

            _context.SaveChanges();
        }

        public GetAllClinicsResponse GetAllClinics()
        {
            var clinics = _context
                .Clinic
                .Select(x => new GetAllClinicsResponse.Clinic
                {
                    Id = x.Id,
                    Name = x.Name,
                    SurgeryType = x.SurgeryType,
                    Patients = x.Patients.Select(p => new GetAllClinicsResponse.Patient
                    {
                        Id = p.Id,
                        FirstName = p.FirstName,
                        LastName = p.LastName
                    })
                })
                .AsNoTracking()
                .ToList();

            return new GetAllClinicsResponse
            {
                Clinics = clinics
            };
        }
    }
}
