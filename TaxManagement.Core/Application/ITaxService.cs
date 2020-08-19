using System;
using System.Collections.Generic;
using System.Text;

namespace TaxManagement.Core
{
    public interface ITaxService
    {
        List<MuncipalityTax> Get();
        MuncipalityTax GetById(string Id);
        decimal GetTaxRateByMunicipalityDate(string municipality, DateTime date);
        MuncipalityTax Insert(MuncipalityTax muncipalityTax);
        void ImportTaxData(List<MuncipalityTax> muncipalityTaxes);
        void Update(MuncipalityTax muncipalityTax);
        int GetMaxPriority(string muncipality);
    }
}
