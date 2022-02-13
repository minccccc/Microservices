using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Grpc;
using PlatformService.SyncDataServices.Http;
using MassTransit;
using PlatformService.MassTransit;
using IHost = Microsoft.Extensions.Hosting.IHost;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using MsSql Server DB");
    builder.Services.AddDbContext<AppDbContext>(opt =>
    {
        opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn"));
    });
}
else
{
    Console.WriteLine("--> Using InMemory DB");
    builder.Services.AddDbContext<AppDbContext>(opt =>
    {
        opt.UseInMemoryDatabase("InMem");
    });
}

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddGrpc();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services
    .AddMassTransit(x =>
        {
            x.AddConsumer<MessageConsumer>();

            x.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        })
    .AddMassTransitHostedService(true)
    .AddHostedService<MessagePublisher>();

Console.WriteLine($"--> CommandService Configuration {builder.Configuration["CommandService"]}");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<GrpcPlatformService>();


//IHost host = Host.CreateDefaultBuilder(args)
//    .ConfigureServices((hostContext, services) =>
//    {
//        services.AddMassTransit(x =>
//        {
//            x.AddConsumer<MessageConsumer>();

//            x.UsingInMemory((context,cfg) =>
//            {
//                cfg.ConfigureEndpoints(context);
//            });
//        });
//        services.AddMassTransitHostedService(true);

//        services.AddHostedService<MessagePublisher>();
//    })
//    .Build();

//await host.RunAsync();


PrepDb.PredPopulation(app, builder.Environment.IsProduction());
app.Run();

