using MiniETrade.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddPersistenceServices();
builder.Services.AddCors(options => options.AddDefaultPolicy(
    //policy => policy.AllowAnyHeader().AllowAnyOrigin()  //her s.a diyen siteye girebilir �eklinde bir ayarlama.
    policy => policy.WithOrigins("http://localhost:4200/", "https://localhost:4200/", "http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod() //b�ylece sadece burdaki arkada�lar istek atabilirler API'ye.
));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
