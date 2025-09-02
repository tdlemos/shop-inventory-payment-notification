using MassTransit;
using Refit;
using Shop.Notification.Service.Consumers;
using Shop.Notification.Service.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<NotificationConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddRefitClient<INotificationApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7181"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
