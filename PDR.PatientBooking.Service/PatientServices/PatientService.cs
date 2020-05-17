using Microsoft.EntityFrameworkCore;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Data.Models;
using PDR.PatientBooking.Service.Enums;
using PDR.PatientBooking.Service.PatientServices.Requests;
using PDR.PatientBooking.Service.PatientServices.Responses;
using PDR.PatientBooking.Service.PatientServices.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDR.PatientBooking.Service.PatientServices
{
    public class PatientService : IPatientService
    {
        private readonly PatientBookingContext _context;
        private readonly IAddPatientRequestValidator _validator;

        public PatientService(PatientBookingContext context, IAddPatientRequestValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public void AddPatient(AddPatientRequest request)
        {
            var validationResult = _validator.ValidateRequest(request);

            if (!validationResult.PassedValidation)
            {
                throw new ArgumentException(validationResult.Errors.First());
            }

            _context.Patient.Add(new Patient
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = (int)request.Gender,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
                Orders = new List<Order>(),
                ClinicId = request.ClinicId,
                Created = DateTime.UtcNow
            });

            _context.SaveChanges();
        }

        public GetAllPatientsResponse GetAllPatients()
        {
            var patients = _context
                .Patient
                .Select(x => new GetAllPatientsResponse.Patient
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    DateOfBirth = x.DateOfBirth,
                    Gender = (Gender)x.Gender,
                    Clinic = new GetAllPatientsResponse.Clinic
                    {
                        Id = x.ClinicId,
                        Name = x.Clinic.Name
                    }
                })
                .ToList();

            return new GetAllPatientsResponse
            {
                Patients = patients
            };
        }
    }
}
