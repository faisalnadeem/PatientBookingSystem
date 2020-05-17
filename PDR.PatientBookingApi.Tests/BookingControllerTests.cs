using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Data.Models;
using PDR.PatientBookingApi.Controllers;
using PDR.PatientBookingApi.Mappers;
using PDR.PatientBookingApi.Models;

namespace PDR.PatientBookingApi.Tests
{
    public class GivenARequestToAddNewBooking
    {
        Mock<IPatientBookingContext> _contextMock;
        Mock<IBookingOrderMapper> _bookingOrderMapperMock;
        Mock<DbSet<Order>> _ordersDbSetMock;
        StatusCodeResult _result;

        [SetUp]
        public void Setup()
        {
            _contextMock = new Mock<IPatientBookingContext>();
            _bookingOrderMapperMock = new Mock<IBookingOrderMapper>();
            _ordersDbSetMock = new Mock<DbSet<Order>>();

            _contextMock.Setup(x => x.Order).Returns(_ordersDbSetMock.Object);
            _bookingOrderMapperMock.Setup(x => x.Map(It.IsAny<Booking>())).Returns(new Order { StartTime = DateTime.Now.AddDays(1) });

            var controller = new BookingController(_contextMock.Object, _bookingOrderMapperMock.Object);
            _result = (StatusCodeResult)controller.AddBooking(new Booking());
        }

        [Test]
        public void ThenBookingShouldBeAddedSuccessfully()
        {
            Assert.AreEqual(StatusCodes.Status200OK, _result.StatusCode);
        }
    }

    public class GivenARequestToAddNewBookingInThePast
    {
        Mock<IPatientBookingContext> _contextMock;
        Mock<IBookingOrderMapper> _bookingOrderMapperMock;
        Mock<DbSet<Order>> _ordersDbSetMock;
        StatusCodeResult _result;

        [SetUp]
        public void Setup()
        {
            _contextMock = new Mock<IPatientBookingContext>();
            _bookingOrderMapperMock = new Mock<IBookingOrderMapper>();
            _ordersDbSetMock = new Mock<DbSet<Order>>();

            _contextMock.Setup(x => x.Order).Returns(_ordersDbSetMock.Object);
            _bookingOrderMapperMock.Setup(x => x.Map(It.IsAny<Booking>())).Returns(new Order());

            var controller = new BookingController(_contextMock.Object, _bookingOrderMapperMock.Object);
            _result = (StatusCodeResult)controller.AddBooking(new Booking());
        }

        [Test]
        public void ThenBookingShouldNotBeAddedSuccessfully()
        {
            Assert.AreEqual(StatusCodes.Status406NotAcceptable, _result.StatusCode);
        }
    }
}