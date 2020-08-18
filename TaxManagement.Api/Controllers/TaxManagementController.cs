using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaxManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxManagementController : ControllerBase
    {
        // GET: api/TaxManagement
        [HttpGet]
        public decimal GetPercentage(string muncipality, DateTime date)
        {
            return 0.1m;
        }

    }
}
