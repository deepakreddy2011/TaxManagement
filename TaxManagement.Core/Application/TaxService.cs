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

        public List<MuncipalityTax> Get()
        {
            return this.repository.Get();
        }

        public MuncipalityTax GetById(string Id)
        {
            return this.repository.GetById(Id);
        }

        public decimal GetTaxRateByMunicipalityDate(string municipality, DateTime date)
        {
            return this.repository.GetTaxRate(municipality, date);
        }

        public MuncipalityTax Insert(MuncipalityTax muncipalityTax)
        {
            var tax = this.repository.Insert(muncipalityTax);
            return tax;
        }

        public void ImportTaxData(List<MuncipalityTax> muncipalityTaxes)
        {
            this.repository.ImportTaxData(muncipalityTaxes);
        }

        public void Update(MuncipalityTax muncipalityTax)
        {
            this.repository.Update(muncipalityTax);
        }

        public List<MuncipalityTax> GetMuncipalityByName(string muncipality)
        {
            return this.repository.GetMuncipalityByName(muncipality);
        }
    }
}
