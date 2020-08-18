using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TaxManagement.Core;

namespace TaxManagement.UnitTests
{
    public class CoreTests
    {
        private Mock<IRepository> mockRepository;

        [SetUp]
        public void setup()
        {
            this.mockRepository = new Mock<IRepository>();
        }

        [TestCase]
        public void GetTaxRateYearlyTest()
        {
            this.mockRepository.Setup(x => x.GetTaxRate("Copenhagen", new DateTime(2016, 7, 10))).Returns(0.2m);

            var taxService = new TaxService(this.mockRepository.Object);
            var taxRate = taxService.GetTaxRateByMunicipalityDate("Copenhagen", new DateTime(2016, 7, 10));

            Assert.AreEqual(taxRate, 0.2m);
        }
    }
}
