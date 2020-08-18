using System;
using System.Collections.Generic;
using System.Text;

namespace TaxManagement.Core
{
    public interface IRepository
    {
        decimal GetTaxRate(string municipality, DateTime date);

        MuncipalityTax Insert(MuncipalityTax muncipalityTax);
    }
}
