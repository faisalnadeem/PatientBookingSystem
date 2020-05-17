using PDR.PatientBooking.Data.Models;
using System;
using System.Collections.Generic;

namespace PDR.PatientBooking.Data.DataSeed
{
    /// <summary>
    /// You can ignore this class. Because we're using an in memory database we wanted to seed the db with data to enable you to test your application
    /// </summary>
    public class DatabaseSeed
    {
        private readonly PatientBookingContext _context;

        public DatabaseSeed(PatientBookingContext context)
        {
            _context = context;
        }

        public void SeedDatabase()
        {
            var clinics = AddClinics();
            var doctors = AddDoctors();
            var patients = AddPatients();
            var orders = AddOrders();

            LinkPatientsToClinics(clinics, patients);
            LinkOrdersToDoctors(orders, doctors);
            LinkOrdersToPatients(orders, patients);
        }


        private List<Order> AddOrders()
        {
            var orders = new List<Order>
            {
                new Order
                {
                    Id = Guid.Parse("683074b8-44c9-468b-9288-dfafa1e533c9"),
                    StartTime = new DateTime(2020, 1, 11, 12, 15, 00),
                    EndTime = new DateTime(2020, 1, 11, 12, 30, 00)
                },
                new Order
                {
                    Id = Guid.Parse("57706153-7600-41fd-8a5e-dc60a584e21c"),
                    StartTime = new DateTime(2020, 1, 11, 12, 30, 00),
                    EndTime = new DateTime(2020, 1, 11, 12, 45, 00)
                },
                new Order
                {
                    Id = Guid.Parse("b6aad474-5b5d-42b7-a274-1a94f74ff69a"),
                    StartTime = new DateTime(2020, 1, 11, 14, 15, 00),
                    EndTime = new DateTime(2020, 1, 11, 14, 30, 00)
                },
                new Order
                {
                    Id = Guid.Parse("b67c2730-0c12-4236-9b1a-d5b1d22db247"),
                    StartTime = new DateTime(2021, 1, 11, 12, 15, 00),
                    EndTime = new DateTime(2021, 1, 11, 12, 30, 00)
                },
                new Order
                {
                    Id = Guid.Parse("31924ff1-c64e-4e1e-977e-704abc062aa4"),
                    StartTime = new DateTime(2021, 1, 12, 12, 15, 00),
                    EndTime = new DateTime(2021, 1, 12, 12, 30, 00)
                }
            };

            _context.Order.AddRange(orders);
            _context.SaveChanges();

            return orders;
        }

        private List<Patient> AddPatients()
        {
            var patients = new List<Patient>
            {
                new Patient
                {
                    Id = 100,
                    Gender = 1,
                    FirstName = "Bill",
                    LastName = "Bagly",
                    Email = "BToTheB@gmail.com",
                    DateOfBirth = new DateTime(1912, 1, 17),
                    Created = DateTime.UnixEpoch
                },
                new Patient
                {
                    Id = 173,
                    Gender = 1,
                    FirstName = "Philbert",
                    LastName = "McPlop",
                    Email = "ThePIsSilent@gmail.com",
                    DateOfBirth = new DateTime(1968, 4, 7),
                    Created = DateTime.UnixEpoch
                },
                new Patient
                {
                    Id = 159,
                    Gender = 1,
                    FirstName = "Stephen",
                    LastName = "Fry",
                    Email = "TheRealStephenFry@gmail.com",
                    DateOfBirth = new DateTime(1957, 8, 24),
                    Created = DateTime.UnixEpoch
                }
            };

            _context.Patient.AddRange(patients);
            _context.SaveChanges();

            return patients;
        }

        private List<Doctor> AddDoctors()
        {
            var doctors = new List<Doctor>
            {
                new Doctor()
                {
                    Id = 1,
                    DateOfBirth = new DateTime(1980, 1, 1),
                    Email = "DrMg@docworld.com",
                    FirstName = "Mac",
                    LastName = "Guffin",
                    Gender = 1,
                    Created = DateTime.UtcNow
                },
                new Doctor()
                {
                    Id = 2,
                    DateOfBirth = new DateTime(1975, 5, 3),
                    Email = "DrBlop@docworld.com",
                    FirstName = "Betty",
                    LastName = "Blop",
                    Gender = 0,
                    Created = DateTime.UtcNow
                },
                new Doctor()
                {
                    Id = 3,
                    DateOfBirth = new DateTime(1990, 10, 12),
                    Email = "L33tFoosBallPlayer69@docworld.com",
                    FirstName = "Lindsay",
                    LastName = "Mcowat",
                    Gender = 0,
                    Created = DateTime.UtcNow
                }
            };

            _context.Doctor.AddRange(doctors);
            _context.SaveChanges();

            return doctors;
        }

        private List<Clinic> AddClinics()
        {
            var clinics = new List<Clinic>
            {
                new Clinic
                {
                    Id = 12,
                    Name = "Mr Docs Healthcare Bonanza",
                    SurgeryType = SurgeryType.SystemOne
                },
                new Clinic
                {
                    Id = 1324,
                    Name = "Dockity Doc Walk In Centre",
                    SurgeryType = SurgeryType.SystemTwo
                }
            };

            _context.Clinic.AddRange(clinics);
            _context.SaveChanges();

            return clinics;
        }

        private void LinkPatientsToClinics(List<Clinic> clinics, List<Patient> patients)
        {
            var count = 0;
            foreach (var patient in patients)
            {
                patient.ClinicId = clinics[count++ % clinics.Count].Id;
            }

            _context.SaveChanges();
        }

        private void LinkOrdersToDoctors(List<Order> orders, List<Doctor> doctors)
        {
            var count = 0;
            foreach (var order in orders)
            {
                order.DoctorId = doctors[count++ % doctors.Count].Id;
            }

            _context.SaveChanges();
        }

        private void LinkOrdersToPatients(List<Order> orders, List<Patient> patients)
        {
            var count = 0;
            foreach (var order in orders)
            {
                order.PatientId = patients[count++ % patients.Count].Id;
            }

            _context.SaveChanges();
        }
    }
}
