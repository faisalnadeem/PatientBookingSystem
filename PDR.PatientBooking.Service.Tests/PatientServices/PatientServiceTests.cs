using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Data.Models;
using PDR.PatientBooking.Service.Enums;
using PDR.PatientBooking.Service.PatientServices;
using PDR.PatientBooking.Service.PatientServices.Requests;
using PDR.PatientBooking.Service.PatientServices.Responses;
using PDR.PatientBooking.Service.PatientServices.Validation;
using PDR.PatientBooking.Service.Validation;

namespace PDR.PatientBooking.Service.Tests.PatientServices
{
    [TestFixture]
    public class PatientServiceTests
    {
        private MockRepository _mockRepository;
        private IFixture _fixture;

        private PatientBookingContext _context;
        private Mock<IAddPatientRequestValidator> _validator;

        private PatientService _patientService;

        [SetUp]
        public void SetUp()
        {
            // Boilerplate
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _fixture = new Fixture();

            //Prevent fixture from generating from entity circular references
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

            // Mock setup
            _context = new PatientBookingContext(new DbContextOptionsBuilder<PatientBookingContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            _validator = _mockRepository.Create<IAddPatientRequestValidator>();

            // Mock default
            SetupMockDefaults();

            // Sut instantiation
            _patientService = new PatientService(
                _context,
                _validator.Object
            );
        }

        private void SetupMockDefaults()
        {
            _validator.Setup(x => x.ValidateRequest(It.IsAny<AddPatientRequest>()))
                .Returns(new PdrValidationResult(true));
        }

        [Test]
        public void AddPatient_ValidatesRequest()
        {
            //arrange
            var request = _fixture.Create<AddPatientRequest>();

            //act
            _patientService.AddPatient(request);

            //assert
            _validator.Verify(x => x.ValidateRequest(request), Times.Once);
        }

        [Test]
        public void AddPatient_ValidatorFails_ThrowsArgumentException()
        {
            //arrange
            var failedValidationResult = new PdrValidationResult(false, _fixture.Create<string>());

            _validator.Setup(x => x.ValidateRequest(It.IsAny<AddPatientRequest>())).Returns(failedValidationResult);

            //act
            var exception = Assert.Throws<ArgumentException>(() =>_patientService.AddPatient(_fixture.Create<AddPatientRequest>()));

            //assert
            exception.Message.Should().Be(failedValidationResult.Errors.First());
        }

        [Test]
        public void AddPatient_AddsPatientToContextWithGeneratedId()
        {
            //arrange
            var request = _fixture.Create<AddPatientRequest>();

            var expected = new Patient
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = (int)request.Gender,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
                Orders = new List<Order>(),
                ClinicId = request.ClinicId,
                Created = DateTime.UtcNow
            };

            //act
            _patientService.AddPatient(request);

            //assert
            _context.Patient.Should().ContainEquivalentOf(expected, options => options.Excluding(patient => patient.Id));
        }

        [Test]
        public void GetAllPatients_NoPatients_ReturnsEmptyList()
        {
            //arrange

            //act
            var res  = _patientService.GetAllPatients();

            //assert
            res.Patients.Should().BeEmpty();
        }

        [Test]
        public void GetAllPatients_ReturnsMappedPatientList()
        {
            //arrange
            var patient = _fixture.Create<Patient>();
            _context.Patient.Add(patient);
            _context.SaveChanges();

            var expected = new GetAllPatientsResponse
            {
                Patients = new List<GetAllPatientsResponse.Patient>
                {
                    new GetAllPatientsResponse.Patient
                    {
                        Id = patient.Id,
                        FirstName = patient.FirstName,
                        LastName = patient.LastName,
                        Gender = (Gender)patient.Gender,
                        DateOfBirth = patient.DateOfBirth,
                        Email = patient.Email,
                        Clinic = new GetAllPatientsResponse.Clinic
                        {
                            Id = patient.Clinic.Id,
                            Name = patient.Clinic.Name
                        }
                    }
                }
            };

            //act
            var res = _patientService.GetAllPatients();

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
