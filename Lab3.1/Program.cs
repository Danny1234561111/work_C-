using Microsoft.EntityFrameworkCore;
using WebApplication1;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Добавление контекста базы данных в контейнер зависимостей
builder.Services.AddDbContext<WebApplication1.AppContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавление CORS
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

// Добавление Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

var app = builder.Build();

// Настройка CORS
app.UseCors("AllowAllOrigins"); // Применяем политику CORS

// Настройка Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Делаем Swagger UI доступным по корневому URL
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Инициализация базы данных и заполнение начальными данными
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<WebApplication1.AppContext>();
    context.Database.EnsureCreated(); // Убедитесь, что база данных создана
    context.SeedData(); // Вызов метода для заполнения начальными данными
}

app.Run();
