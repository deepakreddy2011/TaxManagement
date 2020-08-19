using System;
using System.Collections.Generic;
using System.Text;

namespace TaxManagement.Core
{
    public interface ITaxService
    {
        List<MuncipalityTax> Get();
        decimal GetTaxRateByMunicipalityDate(string municipality, DateTime date);
        MuncipalityTax Insert(MuncipalityTax muncipalityTax);
        void ImportTaxData(List<MuncipalityTax> muncipalityTaxes);
    }
}
