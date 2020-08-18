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

        public decimal GetTaxRate(string municipality, DateTime date)
        {
            var taxRate = this.muncipalityTaxes.Find(tax => true).ToList()
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
    }
}
