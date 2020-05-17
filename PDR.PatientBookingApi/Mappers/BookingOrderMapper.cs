using System.Linq;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Data.Models;
using PDR.PatientBookingApi.Models;

namespace PDR.PatientBookingApi.Mappers
{
    public class BookingOrderMapper : IBookingOrderMapper
    {
        private readonly IPatientBookingContext _context;
        public BookingOrderMapper(IPatientBookingContext context)
        {
            _context = context;
        }

        public Order Map(Booking booking)
        {
            var bookingSurgeryType = _context.Patient.FirstOrDefault(x => x.Id == booking.PatientId)?.Clinic.SurgeryType ?? SurgeryType.SystemOne;

            return new Order
            {
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                PatientId = booking.PatientId,
                DoctorId = booking.DoctorId,
                SurgeryType = (int)bookingSurgeryType
            };

        }
    }
}