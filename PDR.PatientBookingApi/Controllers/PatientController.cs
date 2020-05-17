using Microsoft.AspNetCore.Mvc;
using PDR.PatientBooking.Service.PatientServices;
using PDR.PatientBooking.Service.PatientServices.Requests;
using System;

namespace PDR.PatientBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet()]
        public IActionResult GetAllPatients()
        {
            try
            {
                return Ok(_patientService.GetAllPatients());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost()]
        public IActionResult AddPatient(AddPatientRequest request)
        {
            try
            {
                _patientService.AddPatient(request);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}