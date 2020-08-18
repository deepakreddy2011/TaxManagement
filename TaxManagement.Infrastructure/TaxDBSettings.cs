using System;
using System.Collections.Generic;
using System.Text;
using TaxManagement.Core;

namespace TaxManagement.Infrastructure
{

    public class TaxDBSettings : ITaxDBSettings
    {
        public string TaxCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
