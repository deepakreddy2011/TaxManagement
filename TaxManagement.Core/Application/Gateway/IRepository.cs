using System;
using System.Collections.Generic;
using System.Text;

namespace TaxManagement.Core
{
    public interface IRepository
    {
        List<MuncipalityTax> Get();

        decimal GetTaxRate(string municipality, DateTime date);

        MuncipalityTax Insert(MuncipalityTax muncipalityTax);

        void ImportTaxData(List<MuncipalityTax> muncipalityTaxes);
    }
}
