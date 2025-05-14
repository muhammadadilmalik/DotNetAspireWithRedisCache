var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("my-redis-cache");

builder.AddProject<Projects.DotNetAspireWithRedisCache>("dotnetaspirewithrediscache")
    .WithReference(redis)
    .WaitFor(redis);

builder.Build().Run();
