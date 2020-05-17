using Microsoft.EntityFrameworkCore;
using PDR.PatientBooking.Data.Models;

namespace PDR.PatientBooking.Data
{
    public interface IPatientBookingContext
    {
        DbSet<Order> Order { get; set; }
        DbSet<Patient> Patient { get; set; }
        DbSet<Doctor> Doctor { get; set; }
        DbSet<Clinic> Clinic { get; set; }
        int SaveChanges();
    }

    public class PatientBookingContext : DbContext, IPatientBookingContext
    {


        public PatientBookingContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Order> Order { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<Clinic> Clinic { get; set; }
    }
}
