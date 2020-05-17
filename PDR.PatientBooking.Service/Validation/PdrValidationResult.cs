using System.Collections.Generic;

namespace PDR.PatientBooking.Service.Validation
{
    public class PdrValidationResult
    {
        public bool PassedValidation { get; set; }
        public List<string> Errors { get; set; }

        public PdrValidationResult(bool passedValidation)
        {
            PassedValidation = passedValidation;
            Errors = new List<string>();
        }

        public PdrValidationResult(bool passedValidation, string error)
        {
            PassedValidation = passedValidation;
            Errors = new List<string> { error };
        }

        public PdrValidationResult(bool passedValidation, List<string> errors)
        {
            PassedValidation = passedValidation;
            Errors = errors;
        }
    }
}
