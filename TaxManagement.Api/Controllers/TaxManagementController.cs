using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExcelMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaxManagement.Core;
using TaxManagement.Infrastructure;

namespace TaxManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxManagementController : ControllerBase
    {
        private ITaxService taxService;
        private IMapper mapper;

        public TaxManagementController(ITaxService taxService, IMapper mapper)
        {
            this.taxService = taxService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("GetTaxes")]
        public List<MuncipalityTax> GetTaxes()
        {
            return this.taxService.Get();
        }

        // GET: api/TaxManagement
        [HttpGet]
        public decimal GetTaxRateByMunicipalityDate(string municipality, DateTime date)
        {
            return this.taxService.GetTaxRateByMunicipalityDate(municipality, date);
        }

        [HttpPost]
        public MuncipalityTax Insert(MuncipalityTax muncipalityTax)
        {
            var tax = this.taxService.Insert(muncipalityTax);
            return tax;
        }

        [HttpPost]
        [Route("upload")]
        public void PostFile(IFormFile file)
        {
            var fileLocalPath = this.SaveFileToLocal(file);
            var taxesDtos = this.ReadTaxesFromLocalFile(fileLocalPath);
            var taxes = this.mapper.Map<List<MuncipalityTax>>(taxesDtos);
            this.taxService.ImportTaxData(taxes);
        }
        private string SaveFileToLocal(IFormFile file)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            filePath = Path.Combine(filePath, $"{DateTime.Now.ToLongDateString()}", $"Local_Time_{DateTime.Now.ToLongTimeString()}".Replace(":", "-"));
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath = Path.Combine(filePath, Path.GetFileName(file.FileName));

            using (var stream = System.IO.File.Create(filePath))
            {
                file.CopyTo(stream);
            }

            return filePath;
        }

        private List<MuncipalityTaxDto> ReadTaxesFromLocalFile(string fileLocalPath)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using var stream = System.IO.File.OpenRead(fileLocalPath);
            using var importer = new ExcelImporter(stream);
            ExcelSheet sheet = importer.ReadSheet();
            var taxes = sheet.ReadRows<MuncipalityTaxDto>().ToArray().ToList();
            return taxes;
        }
    }
}
