using System;
using System.Collections.Generic;
using System.Text;

namespace TaxManagement.Core
{
    public class TaxService : ITaxService
    {
        private IRepository repository;

        public TaxService(IRepository repository)
        {
            this.repository = repository;
        }

        public decimal GetTaxRateByMunicipalityDate(string municipality, DateTime date)
        {
           return this.repository.GetTaxRate(municipality,date);
        }
    }
}
