using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using PDR.PatientBooking.Service.Common.Enums;

namespace PDR.PatientBooking.Service.Common.Requests
{
    public abstract class BasePersonRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }

        public List<string> EnsureRequestFieldValidation()
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(FirstName))
                errors.Add("FirstName must be populated");

            if (string.IsNullOrEmpty(LastName))
                errors.Add("LastName must be populated");

            if (string.IsNullOrEmpty(Email))
                errors.Add("Email must be populated");

            EnsureEmailIsValid(errors);

            return errors;

        }

        private void EnsureEmailIsValid(List<string> errors)
        {
            try
            {
                var email = new MailAddress(Email);
            }
            catch (Exception)
            {
                errors.Add("Email must be a valid email address");
            }
        }
    }
}
