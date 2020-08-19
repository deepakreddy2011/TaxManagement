using System;
using System.Collections.Generic;
using System.Text;

namespace TaxManagement.Core
{
    public interface IRepository
    {
        List<MuncipalityTax> Get();
        MuncipalityTax GetById(string Id);
        decimal GetTaxRate(string municipality, DateTime date);

        MuncipalityTax Insert(MuncipalityTax muncipalityTax);

        void ImportTaxData(List<MuncipalityTax> muncipalityTaxes);
        void Update(MuncipalityTax muncipalityTax);
    }
}
