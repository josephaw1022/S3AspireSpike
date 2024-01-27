var builder = DistributedApplication.CreateBuilder(args);

var minioContainer = builder.AddContainer("minio", "minio/minio")
    .WithServiceBinding(containerPort: 9000, name: "minio-container-port", hostPort: 9000)
    .WithServiceBinding(containerPort: 9001, name: "minio-console-port", hostPort:9001)
    .WithEnvironment("MINIO_ROOT_USER", "minio")
    .WithEnvironment("MINIO_ROOT_PASSWORD", "RunningZebraMan32332#")
    .WithEnvironment("MINIO_ADDRESS", ":9000")
    .WithEnvironment("MINIO_CONSOLE_ADDRESS", ":9001")
    .WithVolumeMount("minio-volume-mount", "/data", VolumeMountType.Named)
    .WithArgs("server", "/data");

builder.AddProject<Projects.ApiWithS3>("apiwiths3");


builder.AddProject<Projects.ApiWithMinio>("apiwithminio");


builder.Build().Run();