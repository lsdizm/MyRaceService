using Microsoft.AspNetCore.HttpOverrides;
using my.race.connect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(7031);
    serverOptions.ListenAnyIP(7030, (httpsOpt) => {
        httpsOpt.UseHttps();
    });
});

builder.Services.AddSingleton<IConfigurations, Configurations>();
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<IDataBases, DataBases>();
builder.Services.AddScoped<IDataAPI, DataAPI>();

builder.Services.AddCors(options =>{
    options.AddDefaultPolicy(
        policyBuilder => policyBuilder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(new string[]{
                "http://localhost:8080",
                "http://10.0.0.185",
                "http://10.0.0.185:8080",
                "http://152.70.232.248/:8080",
                "http://152.70.232.248",                
            })
    );

    options.AddPolicy("MjServicePolicy",
        policyBuilder => policyBuilder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin()
    );
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseCertificateForwarding();
app.UseAuthorization();
app.MapControllers();

app.UseCors();


app.UseForwardedHeaders(new ForwardedHeadersOptions 
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.Run();
