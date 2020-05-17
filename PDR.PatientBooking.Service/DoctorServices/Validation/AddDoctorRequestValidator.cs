using PDR.PatientBooking.Data;
using PDR.PatientBooking.Service.DoctorServices.Requests;
using System.Collections.Generic;
using System.Linq;
using PDR.PatientBooking.Service.Common.Validation;

namespace PDR.PatientBooking.Service.DoctorServices.Validation
{
    public class AddDoctorRequestValidator : IAddDoctorRequestValidator
    {
        private readonly PatientBookingContext _context;

        public AddDoctorRequestValidator(PatientBookingContext context)
        {
            _context = context;
        }

        public PdrValidationResult ValidateRequest(AddDoctorRequest request)
        {
            var result = new PdrValidationResult(true);

            var fieldValidationErrors = request.EnsureRequestFieldValidation();

            if (fieldValidationErrors.Any())
            {
                result.PassedValidation = false;
                result.Errors.AddRange(fieldValidationErrors);
                return result;
            }

            if (DoctorAlreadyInDb(request, ref result))
                return result;

            return result;
        }

        private bool DoctorAlreadyInDb(AddDoctorRequest request, ref PdrValidationResult result)
        {
            if (_context.Doctor.Any(x => x.Email == request.Email))
            {
                result.PassedValidation = false;
                result.Errors.Add("A doctor with that email address already exists");
                return true;
            }

            return false;
        }
    }
}
