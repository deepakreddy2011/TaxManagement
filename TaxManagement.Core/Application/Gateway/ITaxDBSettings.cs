using System;
using System.Collections.Generic;
using System.Text;

namespace TaxManagement.Core
{
    public interface ITaxDBSettings
    {
        string TaxCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
