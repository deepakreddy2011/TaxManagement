using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TaxManagement.Api.Controllers;
using TaxManagement.Core;

namespace TaxManagement.UnitTests
{
    [TestFixture]
    public class ControllerTests
    {
        private Mock<ITaxService> mockTaxService;
        private Mock<IMapper> mockMapper;
        private Mock<IConfiguration> mockConfiguration;

        [SetUp]
        public void setup()
        {
            this.mockTaxService = new Mock<ITaxService>();
            this.mockMapper = new Mock<IMapper>();
            this.mockConfiguration = new Mock<IConfiguration>();
        }

        [TestCase]
        public void GetTaxRateYearlyTest()
        {
            this.mockTaxService.Setup(x => x.GetTaxRateByMunicipalityDate("Copenhagen", new DateTime(2016, 7, 10))).Returns(0.2m);

            var controller = new TaxManagementController(this.mockTaxService.Object, this.mockMapper.Object, this.mockConfiguration.Object);
            var taxRate = controller.GetTaxRateByMunicipalityDate("Copenhagen", new DateTime(2016, 7, 10));
            var okResult = taxRate as OkObjectResult;
            Assert.AreEqual(okResult.StatusCode, 200);
        }

    }
}
