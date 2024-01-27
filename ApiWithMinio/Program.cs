using Minio;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAntiforgery();

// Configure MinIO Client

// Configure MinIO Client
// example mc command done in container: ```mc admin user add myminio newAccessKey12345 newSecretKey54321```
// then needed to run : ```mc admin policy attach myminio readwrite --user newAccessKey12345```
// so that the new user can read and write to the bucket and is not the root user
var endpoint = @"localhost";
var accessKey = @"newAccessKey12345";
var secretKey = @"newSecretKey54321";

builder.Services.AddMinio(configureClient => configureClient
    .WithEndpoint(endpoint, 9000)
    .WithSSL(false)
    .WithCredentials(accessKey, secretKey));


builder.Services.AddControllers();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseAntiforgery();
app.Run();

