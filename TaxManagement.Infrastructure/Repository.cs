using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TaxManagement.Core;

namespace TaxManagement.Infrastructure
{
    public class Repository : IRepository
    {
        private IMongoCollection<MuncipalityTax> muncipalityTaxes;
        public Repository(ITaxDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            this.muncipalityTaxes = database.GetCollection<MuncipalityTax>(settings.TaxCollectionName);
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
    }
}
