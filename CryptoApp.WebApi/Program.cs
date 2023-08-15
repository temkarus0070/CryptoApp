
using CryptoApp.database;
using CryptoApp.WebApi.services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("*");
    logging.ResponseHeaders.Add("*");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 0;
    logging.ResponseBodyLogLimit = 0;

});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4201", "https://localhost:4201")
     .SetIsOriginAllowedToAllowWildcardSubdomains()
     .AllowAnyHeader()
     .AllowCredentials()
     .WithMethods("GET", "PUT", "POST", "DELETE", "OPTIONS")
     .SetPreflightMaxAge(TimeSpan.FromSeconds(3600));
                      });
});


builder.Services.AddHttpClient("binance", options =>
{
    options.BaseAddress = new Uri("https://api.binance.com/api/v3/");
    options.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddCryptoAppDbContext();
builder.Services.AddHostedService<CryptoAssetsLoader>();
builder.Services.AddScoped<CryptoService>();
var configuration = builder.Configuration;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
            {
                o.MetadataAddress = "http://localhost:8080/realms/temkarus0070/.well-known/openid-configuration";
                o.Authority = "http://localhost:8080/realms/temkarus0070";
                o.Audience = "account";
                o.RequireHttpsMetadata = false;
            });

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseHttpLogging();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
