using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Data.Models;
using PDR.PatientBooking.Service.DoctorServices;
using PDR.PatientBooking.Service.DoctorServices.Requests;
using PDR.PatientBooking.Service.DoctorServices.Responses;
using PDR.PatientBooking.Service.DoctorServices.Validation;
using PDR.PatientBooking.Service.Enums;
using PDR.PatientBooking.Service.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDR.PatientBooking.Service.Tests.DoctorServices
{
    [TestFixture]
    public class DoctorServiceTests
    {
        private MockRepository _mockRepository;
        private IFixture _fixture;

        private PatientBookingContext _context;
        private Mock<IAddDoctorRequestValidator> _validator;

        private DoctorService _doctorService;

        [SetUp]
        public void SetUp()
        {
            // Boilerplate
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _fixture = new Fixture();

            //Prevent fixture from generating circular references
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

            // Mock setup
            _context = new PatientBookingContext(new DbContextOptionsBuilder<PatientBookingContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            _validator = _mockRepository.Create<IAddDoctorRequestValidator>();

            // Mock default
            SetupMockDefaults();

            // Sut instantiation
            _doctorService = new DoctorService(
                _context,
                _validator.Object
            );
        }

        private void SetupMockDefaults()
        {
            _validator.Setup(x => x.ValidateRequest(It.IsAny<AddDoctorRequest>()))
                .Returns(new PdrValidationResult(true));
        }

        [Test]
        public void AddDoctor_ValidatesRequest()
        {
            //arrange
            var request = _fixture.Create<AddDoctorRequest>();

            //act
            _doctorService.AddDoctor(request);

            //assert
            _validator.Verify(x => x.ValidateRequest(request), Times.Once);
        }

        [Test]
        public void AddDoctor_ValidatorFails_ThrowsArgumentException()
        {
            //arrange
            var failedValidationResult = new PdrValidationResult(false, _fixture.Create<string>());

            _validator.Setup(x => x.ValidateRequest(It.IsAny<AddDoctorRequest>())).Returns(failedValidationResult);

            //act
            var exception = Assert.Throws<ArgumentException>(() => _doctorService.AddDoctor(_fixture.Create<AddDoctorRequest>()));

            //assert
            exception.Message.Should().Be(failedValidationResult.Errors.First());
        }

        [Test]
        public void AddDoctor_AddsDoctorToContextWithGeneratedId()
        {
            //arrange
            var request = _fixture.Create<AddDoctorRequest>();

            var expected = new Doctor
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = (int)request.Gender,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
                Orders = new List<Order>(),
                Created = DateTime.UtcNow
            };

            //act
            _doctorService.AddDoctor(request);

            //assert
            _context.Doctor.Should().ContainEquivalentOf(expected, options => options.Excluding(doctor => doctor.Id));
        }

        [Test]
        public void GetAllDoctors_NoDoctors_ReturnsEmptyList()
        {
            //arrange

            //act
            var res = _doctorService.GetAllDoctors();

            //assert
            res.Doctors.Should().BeEmpty();
        }

        [Test]
        public void GetAllDoctors_ReturnsMappedDoctorList()
        {
            //arrange
            var doctor = _fixture.Create<Doctor>();
            _context.Doctor.Add(doctor);
            _context.SaveChanges();

            var expected = new GetAllDoctorsResponse
            {
                Doctors = new List<GetAllDoctorsResponse.Doctor>
                {
                    new GetAllDoctorsResponse.Doctor
                    {
                        Id = doctor.Id,
                        FirstName = doctor.FirstName,
                        LastName = doctor.LastName,
                        Gender = (Gender)doctor.Gender,
                        DateOfBirth = doctor.DateOfBirth,
                        Email = doctor.Email
                    }
                }
            };

            //act
            var res = _doctorService.GetAllDoctors();

            //assert
            res.Should().BeEquivalentTo(expected);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
