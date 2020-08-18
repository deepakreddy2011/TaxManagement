using System;
using System.Collections.Generic;
using System.Text;

namespace TaxManagement.Core
{
    public class MuncipalityTax
    {
        public string Id { get; set; }

        public string Muncipality { get; set; }

        public string Duration { get; set; }

        public int TaxPriority { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal TaxRate { get; set; }
    }
}
