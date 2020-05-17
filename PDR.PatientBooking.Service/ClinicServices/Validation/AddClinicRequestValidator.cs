using PDR.PatientBooking.Data;
using PDR.PatientBooking.Service.ClinicServices.Requests;
using PDR.PatientBooking.Service.Validation;
using System.Collections.Generic;
using System.Linq;

namespace PDR.PatientBooking.Service.ClinicServices.Validation
{
    public class AddClinicRequestValidator : IAddClinicRequestValidator
    {
        private readonly PatientBookingContext _context;

        public AddClinicRequestValidator(PatientBookingContext context)
        {
            _context = context;
        }

        public PdrValidationResult ValidateRequest(AddClinicRequest request)
        {
            var result = new PdrValidationResult(true);

            if (MissingRequiredFields(request, ref result))
                return result;

            if (ClinicAlreadyInDb(request, ref result))
                return result;

            return result;
        }

        public bool MissingRequiredFields(AddClinicRequest request, ref PdrValidationResult result)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(request.Name))
                errors.Add("Name must be populated");

            if (errors.Any())
            {
                result.PassedValidation = false;
                result.Errors.AddRange(errors);
                return true;
            }

            return false;
        }

        private bool ClinicAlreadyInDb(AddClinicRequest request, ref PdrValidationResult result)
        {
            if (_context.Clinic.Any(x => x.Name == request.Name))
            {
                result.PassedValidation = false;
                result.Errors.Add("A clinic with that name already exists");
                return true;
            }

            return false;
        }
    }
}
