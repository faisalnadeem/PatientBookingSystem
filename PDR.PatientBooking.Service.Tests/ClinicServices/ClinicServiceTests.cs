using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Data.Models;
using PDR.PatientBooking.Service.ClinicServices;
using PDR.PatientBooking.Service.ClinicServices.Requests;
using PDR.PatientBooking.Service.ClinicServices.Responses;
using PDR.PatientBooking.Service.ClinicServices.Validation;
using PDR.PatientBooking.Service.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDR.PatientBooking.Service.Tests.ClinicServices
{
    [TestFixture]
    public class ClinicServiceTests
    {
        private MockRepository _mockRepository;
        private IFixture _fixture;

        private PatientBookingContext _context;
        private Mock<IAddClinicRequestValidator> _validator;

        private ClinicService _clinicService;

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
            _validator = _mockRepository.Create<IAddClinicRequestValidator>();

            // Mock default
            SetupMockDefaults();

            // Sut instantiation
            _clinicService = new ClinicService(
                _context,
                _validator.Object
            );
        }

        private void SetupMockDefaults()
        {
            _validator.Setup(x => x.ValidateRequest(It.IsAny<AddClinicRequest>()))
                .Returns(new PdrValidationResult(true));
        }

        [Test]
        public void AddClinic_ValidatesRequest()
        {
            //arrange
            var request = _fixture.Create<AddClinicRequest>();

            //act
            _clinicService.AddClinic(request);

            //assert
            _validator.Verify(x => x.ValidateRequest(request), Times.Once);
        }

        [Test]
        public void AddClinic_ValidatorFails_ThrowsArgumentException()
        {
            //arrange
            var failedValidationResult = new PdrValidationResult(false, _fixture.Create<string>());

            _validator.Setup(x => x.ValidateRequest(It.IsAny<AddClinicRequest>())).Returns(failedValidationResult);

            //act
            var exception = Assert.Throws<ArgumentException>(() => _clinicService.AddClinic(_fixture.Create<AddClinicRequest>()));

            //assert
            exception.Message.Should().Be(failedValidationResult.Errors.First());
        }

        [Test]
        public void AddClinic_AddsClinicToContextWithGeneratedId()
        {
            //arrange
            var request = _fixture.Create<AddClinicRequest>();

            var expected = new Clinic
            {
                Name = request.Name,
                SurgeryType = request.SurgeryType,
                Patients = new List<Patient>()
            };

            //act
            _clinicService.AddClinic(request);

            //assert
            _context.Clinic.Should().ContainEquivalentOf(expected, options => options.Excluding(clinic => clinic.Id));
        }

        [Test]
        public void GetAllClinics_NoClinics_ReturnsEmptyList()
        {
            //arrange

            //act
            var res = _clinicService.GetAllClinics();

            //assert
            res.Clinics.Should().BeEmpty();
        }

        [Test]
        public void GetAllClinics_ReturnsMappedClinicList()
        {
            //arrange
            var clinic = _fixture.Create<Clinic>();
            _context.Clinic.Add(clinic);
            _context.SaveChanges();

            var expected = new GetAllClinicsResponse
            {
                Clinics = new List<GetAllClinicsResponse.Clinic>
                {
                    new GetAllClinicsResponse.Clinic
                    {
                        Id = clinic.Id,
                        Name = clinic.Name,
                        SurgeryType = clinic.SurgeryType,
                        Patients = clinic.Patients.Select(x => new GetAllClinicsResponse.Patient
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName
                        })
                    }
                }
            };

            //act
            var res = _clinicService.GetAllClinics();

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
