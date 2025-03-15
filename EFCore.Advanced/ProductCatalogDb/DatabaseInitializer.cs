using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogDb;

public class DatabaseInitializer
{
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(ILogger<DatabaseInitializer> logger)
    {
        _logger = logger;
    }

}
