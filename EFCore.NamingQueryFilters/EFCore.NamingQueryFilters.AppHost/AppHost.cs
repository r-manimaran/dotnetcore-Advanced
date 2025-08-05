var builder = DistributedApplication.CreateBuilder(args);

var sqlserverPassword = builder.AddParameter("sqlserverPassword", secret: true);
var sqlserver = builder.AddSqlServer("sqldb", sqlserverPassword, 40796)
                       .WithDataVolume()
                       .AddDatabase("ecommerce");

builder.AddProject<Projects.EFCore_NamingQueryFilters>("efcore-namingqueryfilters")
       .WithReference(sqlserver)
       .WaitFor(sqlserver);

builder.Build().Run();
