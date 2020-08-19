using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExcelMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration configuration;

        public TaxManagementController(ITaxService taxService, IMapper mapper, IConfiguration configuration)
        {
            this.taxService = taxService;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpGet]
        [Route("GetTaxes")]
        public List<MuncipalityTax> GetTaxes()
        {
            return this.taxService.Get();
        }

        // GET: api/TaxManagement
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetTaxRateByMunicipalityDate(string municipality, DateTime date)
        {
            if (string.IsNullOrWhiteSpace(municipality) || date == null || date == DateTime.MinValue || date == DateTime.MaxValue)
            {
                return this.BadRequest(this.configuration.GetSection("MuncipalityAndDateErrorMessage").Value);
            }
            var taxRate = this.taxService.GetTaxRateByMunicipalityDate(municipality, date);
            if (taxRate == default)
            {
                return this.NotFound(this.configuration.GetSection("NoDataFoundErrorMessage").Value);
            }
            else
            {
                return this.Ok(taxRate);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Insert(MuncipalityTax muncipalityTax)
        {
            var message = this.ValidateTaxModel(muncipalityTax);
            if (!string.IsNullOrWhiteSpace(message))
            {
                return this.BadRequest(message);
            }
            int priority = this.taxService.GetMaxPriority(muncipalityTax.Muncipality);
            if (priority != default && muncipalityTax.TaxPriority < priority)
            {
                return this.BadRequest(string.Format(this.configuration.GetSection("NoDataFoundErrorMessage").Value, muncipalityTax.Muncipality));
            }
            var tax = this.taxService.Insert(muncipalityTax);
            return this.CreatedAtAction(nameof(Insert), tax);
        }

        [HttpPost]
        [Route("upload")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostFile(IFormFile file)
        {

            var fileLocalPath = this.SaveFileToLocal(file);
            var extension = Path.GetExtension(fileLocalPath);
            var allowedextensions = this.configuration.GetSection("AllowdFileExtensionstoUpload").Value.Split(',');
            if (!allowedextensions.Contains(extension))
            {
                return this.BadRequest(this.configuration.GetSection("FileNotSupportedMessage").Value);
            }
            var taxesDtos = this.ReadTaxesFromLocalFile(fileLocalPath);
            var taxes = this.mapper.Map<List<MuncipalityTax>>(taxesDtos);
            var isValid = true;
            taxes.ForEach(tax =>
            {
                var message = ValidateTaxModel(tax);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    isValid = false;
                }
            });
            if (!isValid)
            {
                return this.BadRequest(this.configuration.GetSection("UploadInvalidDataMessage").Value);
            }


            this.taxService.ImportTaxData(taxes);
            return this.NoContent();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(MuncipalityTax muncipalityTax)
        {
            var message = this.ValidateTaxModel(muncipalityTax);
            if (!string.IsNullOrWhiteSpace(message))
            {
                return this.BadRequest(message);
            }
            var tax = this.taxService.GetById(muncipalityTax.Id);
            var isRecordExists = tax != null && tax.Id != null;
            if (!isRecordExists)
            {
                return this.NotFound(string.Format(this.configuration.GetSection("UpdateRecordNotFoundMessage").Value, muncipalityTax.Id));
            }

            int priority = this.taxService.GetMaxPriority(muncipalityTax.Muncipality);
            if (priority != default && muncipalityTax.TaxPriority < priority)
            {
                return this.BadRequest(string.Format(this.configuration.GetSection("NoDataFoundErrorMessage").Value, muncipalityTax.Muncipality));
            }

            this.taxService.Update(muncipalityTax);
            return NoContent();
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

        private string ValidateTaxModel(MuncipalityTax muncipalityTax)
        {
            var validationMessages = new List<string>();
            if (string.IsNullOrWhiteSpace(muncipalityTax.Muncipality))
            {
                validationMessages.Add(this.configuration.GetSection("MuncipalityRequired").Value);
            }
            if (string.IsNullOrWhiteSpace(muncipalityTax.Duration))
            {
                validationMessages.Add(this.configuration.GetSection("DurationRequired").Value);
            }
            if (muncipalityTax.TaxPriority == default)
            {
                validationMessages.Add(this.configuration.GetSection("TaxPriorityRequired").Value);
            }
            if (muncipalityTax.StartDate == null || muncipalityTax.StartDate == DateTime.MinValue || muncipalityTax.StartDate == DateTime.MaxValue)
            {
                validationMessages.Add(this.configuration.GetSection("StartDateRequired").Value);
            }
            if (muncipalityTax.EndDate == null || muncipalityTax.EndDate == DateTime.MinValue || muncipalityTax.EndDate == DateTime.MaxValue)
            {
                validationMessages.Add(this.configuration.GetSection("EndDateRequired").Value);
            }
            if (muncipalityTax.TaxRate == default)
            {
                validationMessages.Add(this.configuration.GetSection("TaxRateRequired").Value);
            }
            if (muncipalityTax.StartDate > muncipalityTax.EndDate)
            {
                validationMessages.Add(this.configuration.GetSection("StarAndEndDateErrorMessage").Value);
            }

            var message = string.Join('\n', validationMessages);
            return message;
        }
    }
}
