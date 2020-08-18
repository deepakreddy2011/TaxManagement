using System;
using System.Collections.Generic;
using System.Text;
using TaxManagement.Core;

namespace TaxManagement.Infrastructure
{
    public class Repository : IRepository
    {
        public decimal GetTaxRate(string municipality, DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
