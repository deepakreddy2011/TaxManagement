using System;
using System.Collections.Generic;
using System.Text;

namespace TaxManagement.Core
{
    public interface ITaxService
    {
        decimal GetTaxRateByMunicipalityDate(string municipality, DateTime date);
    }
}
