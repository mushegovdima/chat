using Chat.Api.Hubs;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
    builder =>
    {
        builder.AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials();
    }));

builder.Services.AddControllers();
builder.Services.AddSignalR()
    .AddStackExchangeRedis("host.docker.internal:6379", o => {
        o.Configuration.AllowAdmin = true;
        o.Configuration.ChannelPrefix = "Chat.Api";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");

app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/hubs/chat");
});

app.Run();
