using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis")
                   .WithDataVolume()
                   .WithLifetime(ContainerLifetime.Persistent);


var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin().WithDataVolume().WithLifetime(ContainerLifetime.Persistent); ;

var postgresDb = postgres.AddDatabase("ordersdb")
    ;

var mongo = builder.AddMongoDB("mongo")
                   .WithDataVolume()
                   .WithLifetime(ContainerLifetime.Persistent); ;

var mongoDb = mongo.AddDatabase("orders-mongo");

var server = builder.AddProject<Projects.order_system_modular_monolith_Server>("server")
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints();

var webfrontend = builder.AddViteApp("webfrontend", "../frontend")
    .WithReference(server)
    .WaitFor(server);

server.PublishWithContainerFiles(webfrontend, "wwwroot");

var api = builder.AddProject<Projects.order_system_modular_monolith_Api>("api")
    .WithReference(redis)
    .WithReference(postgresDb)
    .WithReference(mongoDb)
    .WaitFor(redis)
    .WaitFor(postgres)
    .WaitFor(mongo)
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints();

builder.Build().Run();
