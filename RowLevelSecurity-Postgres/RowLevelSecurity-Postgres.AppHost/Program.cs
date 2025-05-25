var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", "postgres");

var password = builder.AddParameter("password", "postgres");

var db = builder.AddPostgres("postgres", username, password, 5432)
                       .WithPgAdmin()
                      .WithDataVolume()
                      .AddDatabase("notes");


builder.AddProject<Projects.NotesApiService>("notesapiservice")
    .WithHttpEndpoint(5001, name: "extra-https")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();
