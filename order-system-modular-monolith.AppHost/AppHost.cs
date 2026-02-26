using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");
var redis = builder.AddRedis("redis");

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin(); // optional UI

var postgresDb = postgres.AddDatabase("ordersdb");

var mongo = builder.AddMongoDB("mongo");

var mongoDb = mongo.AddDatabase("orders-mongo");

var server = builder.AddProject<Projects.order_system_modular_monolith_Server>("server")
    .WithReference(cache)
    .WaitFor(cache)
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

builder.AddProject<Projects.order_system_modular_monolith_Api>("order-system-modular-monolith-api");

builder.Build().Run();
