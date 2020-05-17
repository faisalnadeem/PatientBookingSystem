using Microsoft.AspNetCore.Mvc;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using PDR.PatientBookingApi.Mappers;
using PDR.PatientBookingApi.Models;

namespace PDR.PatientBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IPatientBookingContext _context;
        private readonly IBookingOrderMapper _bookingOrderMapper;

        public BookingController(IPatientBookingContext context, IBookingOrderMapper bookingOrderMapper)
        {
            _context = context;
            _bookingOrderMapper = bookingOrderMapper;
        }

        [HttpGet("patient/{identificationNumber}/next")]
        public IActionResult GetPatientNextAppointment(long identificationNumber)
        {
            var bookings = _context.Order.Where(x => x.PatientId == identificationNumber && x.StartTime > DateTime.Now).OrderBy(x => x.StartTime).ToList();

            if (!bookings.Any())
                return StatusCode(404);

            var booking = bookings.FirstOrDefault(x => x.StartTime > DateTime.Now);
            return Ok(new
            {
                booking.Id,
                booking.DoctorId,
                booking.StartTime,
                booking.EndTime
            });
        }

        [HttpPost()]
        public IActionResult AddBooking(Booking booking)
        {
            var myBooking = _bookingOrderMapper.Map(booking);

            _context.Order.AddRange(new List<Order> { myBooking });
            _context.SaveChanges();

            return StatusCode(200);
        }

        private static MyOrderResult UpdateLatestBooking(List<Order> bookings2, int i)
        {
            MyOrderResult latestBooking;
            latestBooking = new MyOrderResult();
            latestBooking.Id = bookings2[i].Id;
            latestBooking.DoctorId = bookings2[i].DoctorId;
            latestBooking.StartTime = bookings2[i].StartTime;
            latestBooking.EndTime = bookings2[i].EndTime;
            latestBooking.PatientId = bookings2[i].PatientId;
            latestBooking.SurgeryType = (int)bookings2[i].GetSurgeryType();

            return latestBooking;
        }

        private class MyOrderResult
        {
            public Guid Id { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public long PatientId { get; set; }
            public long DoctorId { get; set; }
            public int SurgeryType { get; set; }
        }
    }
}