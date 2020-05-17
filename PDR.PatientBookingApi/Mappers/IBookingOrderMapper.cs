using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PDR.PatientBooking.Data.Models;
using PDR.PatientBookingApi.Models;

namespace PDR.PatientBookingApi.Mappers
{
    public interface IBookingOrderMapper
    {
        Order Map(Booking booking);
    }
}
