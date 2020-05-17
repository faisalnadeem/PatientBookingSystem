using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDR.PatientBookingApi.Models
{
    public class Booking
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public long PatientId { get; set; }
        public long DoctorId { get; set; }
    }
}
