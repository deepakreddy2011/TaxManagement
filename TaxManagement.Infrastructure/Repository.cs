using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TaxManagement.Core;
using AutoMapper;

namespace TaxManagement.Infrastructure
{
    public class Repository : IRepository
    {
        private IMongoCollection<MuncipalityTaxDto> muncipalityTaxes;

        private readonly IMapper mapper;

        public Repository(ITaxDBSettings settings, IMapper mapper)
        {
            this.mapper = mapper;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            this.muncipalityTaxes = database.GetCollection<MuncipalityTaxDto>(settings.TaxCollectionName);
        }

        public List<MuncipalityTax> Get()
        {
            var taxDtos = this.muncipalityTaxes.Find(tax => true).ToList();
            var taxes = this.mapper.Map<List<MuncipalityTax>>(taxDtos);
            return taxes;
        }

        public MuncipalityTax GetById(string Id)
        {
            var taxDto = this.muncipalityTaxes.Find(tax => tax.Id == Id).FirstOrDefault();
            var tax = this.mapper.Map<MuncipalityTax>(taxDto);
            return tax;
        }

        public decimal GetTaxRate(string municipality, DateTime date)
        {
            var taxRate = this.muncipalityTaxes.Find(tax => tax.Muncipality.ToLower() == municipality.ToLower().Trim()).ToList()
                .Where(tax => date >= tax.StartDate && date <= tax.EndDate)
                .OrderBy(tax => tax.TaxPriority)
                .Select(tax => tax.TaxRate)
                .FirstOrDefault();

            return taxRate;
        }

        public MuncipalityTax Insert(MuncipalityTax muncipalityTax)
        {
            var dto = this.mapper.Map<MuncipalityTaxDto>(muncipalityTax);
            this.muncipalityTaxes.InsertOne(dto);
            return muncipalityTax;
        }

        public void ImportTaxData(List<MuncipalityTax> muncipalityTaxes)
        {
            var dtos = this.mapper.Map<List<MuncipalityTaxDto>>(muncipalityTaxes);
            var taxDtos = this.muncipalityTaxes.Find(tax => true).ToList();
            this.muncipalityTaxes.DeleteMany<MuncipalityTaxDto>(taxDtos => true);
            this.muncipalityTaxes.InsertMany(dtos);
        }

        public void Update(MuncipalityTax muncipalityTax)
        {
            var dto = this.mapper.Map<MuncipalityTaxDto>(muncipalityTax);
            this.muncipalityTaxes.ReplaceOne<MuncipalityTaxDto>(tax => tax.Id == dto.Id, dto);
        }

        public int GetMaxPriority(string muncipality)
        {
            var priority = this.muncipalityTaxes.Find(tax => tax.Muncipality == muncipality)
                .ToList()
                .OrderByDescending(tax => tax.TaxPriority)
                .Select(tax => tax.TaxPriority).FirstOrDefault();
            return priority;
        }
    }
}
