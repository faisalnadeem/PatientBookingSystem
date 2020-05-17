using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Data.Models;
using PDR.PatientBooking.Service.ClinicServices.Requests;
using PDR.PatientBooking.Service.ClinicServices.Validation;
using System;

namespace PDR.PatientBooking.Service.Tests.ClinicServices.Validation
{
    [TestFixture]
    public class AddClinicRequestValidatorTests
    {
        private IFixture _fixture;

        private PatientBookingContext _context;

        private AddClinicRequestValidator _addClinicRequestValidator;

        [SetUp]
        public void SetUp()
        {
            // Boilerplate
            _fixture = new Fixture();

            //Prevent fixture from generating from entity circular references 
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));

            // Mock setup
            _context = new PatientBookingContext(new DbContextOptionsBuilder<PatientBookingContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

            // Mock default
            SetupMockDefaults();

            // Sut instantiation
            _addClinicRequestValidator = new AddClinicRequestValidator(
                _context
            );
        }

        private void SetupMockDefaults()
        {

        }

        [Test]
        public void ValidateRequest_AllChecksPass_ReturnsPassedValidationResult()
        {
            //arrange
            var request = GetValidRequest();

            //act
            var res = _addClinicRequestValidator.ValidateRequest(request);

            //assert
            res.PassedValidation.Should().BeTrue();
        }

        [TestCase("")]
        [TestCase(null)]
        public void ValidateRequest_NameNullOrEmpty_ReturnsFailedValidationResult(string name)
        {
            //arrange
            var request = GetValidRequest();
            request.Name = name;

            //act
            var res = _addClinicRequestValidator.ValidateRequest(request);

            //assert
            res.PassedValidation.Should().BeFalse();
            res.Errors.Should().Contain("Name must be populated");
        }

        [Test]
        public void ValidateRequest_ClinicWithNameAlreadyExists_ReturnsFailedValidationResult()
        {
            //arrange
            var request = GetValidRequest();

            var existingClinic = _fixture
                .Build<Clinic>()
                .With(x => x.Name, request.Name)
                .Create();

            _context.Add(existingClinic);
            _context.SaveChanges();

            //act
            var res = _addClinicRequestValidator.ValidateRequest(request);

            //assert
            res.PassedValidation.Should().BeFalse();
            res.Errors.Should().Contain("A clinic with that name already exists");
        }

        private AddClinicRequest GetValidRequest()
        {
            var request = _fixture.Create<AddClinicRequest>();
            return request;
        }
    }
}
