using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaxManagement.Core;

namespace TaxManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxManagementController : ControllerBase
    {
        private ITaxService taxService;

        public TaxManagementController(ITaxService taxService)
        {
            this.taxService = taxService;
        }

        // GET: api/TaxManagement
        [HttpGet]
        public decimal GetTaxRateByMunicipalityDate(string municipality, DateTime date)
        {
            return this.taxService.GetTaxRateByMunicipalityDate(municipality,date);
        }

        [HttpPost]
        public MuncipalityTax Insert(MuncipalityTax muncipalityTax) 
        {
            var tax = this.Insert(muncipalityTax);
            return tax;
        }

    }
}
