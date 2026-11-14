using NihonVending.Infrastructure.Mappers;
using NihonVending.Infrastructure.Persistence;
using NihonVending.Repositories;
using NihonVending.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHealthChecks();
// -- NHibernate --
builder.Services.AddScoped<NHibernate.ISession>(_ =>
    NHibernateSessionFactory.OpenSession()
);


// -- Automapper --
builder.Services.AddAutoMapper(
    config => { },
    typeof(VendingMachineProfile)
);

// -- Repositories --
builder.Services.AddScoped<IVendingMachineRepository, VendingMachineRepository>();

// -- Services --
builder.Services.AddScoped<IVendingMachineService, VendingMachineService>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "NihonVending API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
