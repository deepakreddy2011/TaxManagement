using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TaxManagement.Api.Controllers;
using TaxManagement.Core;

namespace TaxManagement.UnitTests
{
    [TestFixture]
    public class ControllerTests
    {
        private Mock<ITaxService> mockTaxService;

        [SetUp]
        public void setup()
        {
            this.mockTaxService = new Mock<ITaxService>();
        }

        [TestCase]
        public void GetTaxRateYearlyTest()
        {
            this.mockTaxService.Setup(x => x.GetTaxRateByMunicipalityDate("Copenhagen", new DateTime(2016, 7, 10))).Returns(0.2m);

            var controller = new TaxManagementController(this.mockTaxService.Object);
            var taxRate = controller.GetTaxRateByMunicipalityDate("Copenhagen", new DateTime(2016, 7, 10));

            Assert.AreEqual(taxRate, 0.2m);
        }

    }
}
