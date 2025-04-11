using Microsoft.EntityFrameworkCore;
using WebApplication1;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ���������� ��������� ���� ������ � ��������� ������������
builder.Services.AddDbContext<WebApplication1.AppContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ���������� CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

// ���������� Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

var app = builder.Build();

// ��������� CORS
app.UseCors("AllowAllOrigins"); // ��������� �������� CORS

// ��������� Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // ������ Swagger UI ��������� �� ��������� URL
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// ������������� ���� ������ � ���������� ���������� �������
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<WebApplication1.AppContext>();
    context.Database.EnsureCreated(); // ���������, ��� ���� ������ �������
    context.SeedData(); // ����� ������ ��� ���������� ���������� �������
}

app.Run();
