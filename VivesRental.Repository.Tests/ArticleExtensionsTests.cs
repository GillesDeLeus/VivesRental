using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VivesRental.Model;
using VivesRental.Repository.Extensions;
using VivesRental.Tests.Data.Factories;

namespace VivesRental.Repository.Tests
{
    [TestClass]
    public class ArticleExtensionsTests
    {
        [TestMethod]
        public void IsValid_Returns_True_When_Normal()
        {
            //Arrange
            var product = ProductFactory.CreateValidEntity();
            var article = ArticleFactory.CreateValidEntity(product);
            var articleList = new List<Article> { article };

            //Act
            var result = articleList.Where(ArticleExtensions.IsAvailable(new DateTime(2019, 1, 1))).ToList();

            //Assert
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void IsValid_Returns_False_When_Not_Normal()
        {
            //Arrange
            var product = ProductFactory.CreateValidEntity();
            var article = ArticleFactory.CreateValidEntity(product);
            article.Status = ArticleStatus.Broken;
            var articleList = new List<Article> { article };

            //Act
            var result = articleList.Where(ArticleExtensions.IsAvailable(new DateTime(2019, 1, 1))).ToList();

            //Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void IsValid_Returns_False_When_Reserved()
        {
            //Arrange
            var reservationFromDate = new DateTime(2019, 1, 1);
            var reservationUntilDate = new DateTime(2019, 1, 10);
            var customer = CustomerFactory.CreateValidEntity();
            var product = ProductFactory.CreateValidEntity();
            var article = ArticleFactory.CreateValidEntity(product);
            var articleReservation = ArticleReservationFactory.CreateValidEntity(customer, article, reservationFromDate, reservationUntilDate);
            var articleList = new List<Article> { article };

            //Act
            var result = articleList.Where(ArticleExtensions.IsAvailable(new DateTime(2019, 1, 3))).ToList();

            //Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void IsValid_Returns_False_When_Rented()
        {
            //Arrange
            var rentalExpiryDate = new DateTime(2019, 1, 2);
            var customer = CustomerFactory.CreateValidEntity();
            var product = ProductFactory.CreateValidEntity();
            var article = ArticleFactory.CreateValidEntity(product);
            var order = OrderFactory.CreateValidEntity(customer);
            var orderLine = OrderLineFactory.CreateValidEntity(order, article);
            orderLine.ExpiresAt = new DateTime(2019, 1, 2);
            var articleList = new List<Article> { article };

            //Act
            var result = articleList.Where(ArticleExtensions.IsAvailable(new DateTime(2019, 1, 1))).ToList();

            //Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void IsReserved_Returns_False_When_No_Reservations()
        {
            //Arrange
            var reservationFromDate = new DateTime(2019, 1, 1);
            var reservationUntilDate = new DateTime(2019, 1, 2);
            var customer = CustomerFactory.CreateValidEntity();
            var product = ProductFactory.CreateValidEntity();
            var article = ArticleFactory.CreateValidEntity(product);
            var articleReservation = ArticleReservationFactory.CreateValidEntity(customer, article,
                reservationFromDate, reservationUntilDate);
            var articleReservationList = new List<ArticleReservation> { articleReservation };

            //Act
            var result = articleReservationList.Where(ArticleExtensions.IsReserved(new DateTime(2019,1,10))).ToList();

            //Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void IsReserved_Returns_False_With_One_Reservations_Within_Product_Expiration_Period()
        {
            //Arrange
            var reservationFromDate = new DateTime(2019, 1, 1);
            var reservationUntilDate = new DateTime(2019, 1, 2);
            var customer = CustomerFactory.CreateValidEntity();
            var product = ProductFactory.CreateValidEntity();
            var article = ArticleFactory.CreateValidEntity(product);
            var articleReservation = ArticleReservationFactory.CreateValidEntity(customer, article, reservationFromDate, reservationUntilDate);
            var articleReservationList = new List<ArticleReservation> { articleReservation };

            //Act
            var result = articleReservationList.Where(ArticleExtensions.IsReserved(reservationFromDate)).ToList();

            //Assert
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void IsReserved_Returns_False_With_One_Reservations_Within_Product_Expiration_Period_Starting_Before_Reservation()
        {
            //Arrange
            var reservationFromDate = new DateTime(2019, 1, 1);
            var reservationUntilDate = new DateTime(2019, 1, 2);
            var customer = CustomerFactory.CreateValidEntity();
            var product = ProductFactory.CreateValidEntity();
            var article = ArticleFactory.CreateValidEntity(product);
            var articleReservation = ArticleReservationFactory.CreateValidEntity(customer, article, reservationFromDate, reservationUntilDate);
            var articleReservationList = new List<ArticleReservation> { articleReservation };

            //Act
            var result = articleReservationList.Where(ArticleExtensions.IsReserved(reservationFromDate.AddDays(-5))).ToList();

            //Assert
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void IsReserved_Returns_False_With_One_Reservations_Within_Product_Expiration_Period_Requesting_Specific_Period_Ending_Withing_Range()
        {
            //Arrange
            var reservationFromDate = new DateTime(2019, 1, 1);
            var reservationUntilDate = new DateTime(2019, 1, 10);
            var customer = CustomerFactory.CreateValidEntity();
            var product = ProductFactory.CreateValidEntity();
            var article = ArticleFactory.CreateValidEntity(product);
            var articleReservation = ArticleReservationFactory.CreateValidEntity(customer, article, reservationFromDate, reservationUntilDate);
            var articleReservationList = new List<ArticleReservation> { articleReservation };

            //Act
            var result = articleReservationList.Where(ArticleExtensions.IsReserved(reservationFromDate.AddDays(-5), reservationFromDate.AddDays(2))).ToList();

            //Assert
            Assert.AreEqual(1, result.Count);
        }
    }
}
