using Microsoft.AspNetCore.Mvc;
using PDR.PatientBooking.Service.DoctorServices;
using PDR.PatientBooking.Service.DoctorServices.Requests;
using System;

namespace PDR.PatientBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet()]
        public IActionResult GetAllDoctors()
        {
            try
            {
                return Ok(_doctorService.GetAllDoctors());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost()]
        public IActionResult AddDoctor(AddDoctorRequest request)
        {
            try
            {
                _doctorService.AddDoctor(request);
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