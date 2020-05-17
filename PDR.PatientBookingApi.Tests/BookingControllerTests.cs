using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
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
    public class GivenARequestToAddNewBooking : BookingControllerContext
    {
        [SetUp]
        public void Setup()
        {
            BookingOrderMapperMock.Setup(x => x.Map(It.IsAny<Booking>())).Returns(new Order { StartTime = DateTime.Now.AddDays(1) });
            AddBooking(new Booking());
        }

        [Test]
        public void ThenBookingShouldBeAddedSuccessfully()
        {
            Assert.AreEqual(StatusCodes.Status200OK, Result.StatusCode);
        }
    }

    public class GivenARequestToAddNewBookingInThePast : BookingControllerContext
    {

        [SetUp]
        public void Setup()
        {
            BookingOrderMapperMock.Setup(x => x.Map(It.IsAny<Booking>())).Returns(new Order { StartTime = DateTime.Now.AddDays(-1) });
            AddBooking(new Booking());
        }

        [Test]
        public void ThenBookingShouldNotBeAddedSuccessfully()
        {
            Assert.AreEqual(StatusCodes.Status406NotAcceptable, Result.StatusCode);
        }
    }

    public class GivenARequestToAddDoubleBookingWithSameDoctorAtSameTime : BookingControllerContext
    {
        [SetUp]
        public void Setup()
        {
            BookingOrderMapperMock.Setup(x => x.Map(It.IsAny<Booking>())).Returns(ExistingOrder);
            AddBooking(new Booking());
        }

        [Test]
        public void ThenBookingShouldNotBeAddedSuccessfully()
        {
            Assert.AreEqual(StatusCodes.Status406NotAcceptable, Result.StatusCode);
        }
    }

    public class BookingControllerContext
    {
        protected Mock<IPatientBookingContext> ContextMock;
        protected Mock<IBookingOrderMapper> BookingOrderMapperMock;
        protected Mock<DbSet<Order>> OrdersDbSetMock;
        protected StatusCodeResult Result;
        protected Order ExistingOrder;

        public BookingControllerContext()
        {
            ContextMock = new Mock<IPatientBookingContext>();
            BookingOrderMapperMock = new Mock<IBookingOrderMapper>();
            OrdersDbSetMock = new Mock<DbSet<Order>>();

            ExistingOrder = new Order { StartTime = DateTime.Now.AddDays(1), DoctorId = 1 };
            var orders = new List<Order> {ExistingOrder}.AsQueryable();

            OrdersDbSetMock.As<IQueryable<Order>>().Setup(x => x.Provider).Returns(orders.Provider);
            OrdersDbSetMock.As<IQueryable<Order>>().Setup(x => x.Expression).Returns(orders.Expression);
            OrdersDbSetMock.As<IQueryable<Order>>().Setup(x => x.ElementType).Returns(orders.ElementType);
            OrdersDbSetMock.As<IQueryable<Order>>().Setup(x => x.GetEnumerator()).Returns(orders.GetEnumerator());
            ContextMock.Setup(x => x.Order).Returns(OrdersDbSetMock.Object);


        }

        public void AddBooking(Booking booking)
        {
            var controller = new BookingController(ContextMock.Object, BookingOrderMapperMock.Object);
            Result = (StatusCodeResult)controller.AddBooking(booking);
        }
    }

}