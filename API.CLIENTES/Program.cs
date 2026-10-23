using API.DAPPER;
using API.LOGICA.NEGOCIO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Capas de la API (BLL y DAL)
builder.Services.AddScoped<InicioSesionBLL>();
builder.Services.AddScoped<InicioSesionDAL>();
builder.Services.AddScoped<ClientesBLL>();
builder.Services.AddScoped<ClientesDAL>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddCors(options =>
{

    options.AddPolicy("PermitirTodos", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.

// OpenAPI y Swagger quedan activos también en la versión publicada,
// así se pueden probar los endpoints desde el navegador en /swagger
app.MapOpenApi();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "API.CLIENTES v1");
    options.DocumentTitle = "API.CLIENTES";
});

app.UseCors("PermitirTodos");

app.UseAuthorization();

app.MapControllers();

// Página principal (index.html): panel para iniciar sesión y administrar
// los clientes usando los mismos endpoints de la API
app.MapGet("/", (IWebHostEnvironment env) =>
    Results.File(Path.Combine(env.ContentRootPath, "index.html"), "text/html; charset=utf-8"))
   .ExcludeFromDescription();

app.Run();
