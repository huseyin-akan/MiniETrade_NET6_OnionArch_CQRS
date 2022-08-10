using FluentValidation.AspNetCore;
using MiniETrade.Application.Validators.Products;
using MiniETrade.Infrastructure;
using MiniETrade.Infrastructure.Enums;
using MiniETrade.Infrastructure.Filters;
using MiniETrade.Infrastructure.Services.Storage.Local;
using MiniETrade.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();

//builder.Services.AddStorage<LocalStorage>(); aşağıdaki yapı daha güzel oldu.
builder.Services.AddStorage(StorageType.Local);

builder.Services.AddCors(options => options.AddDefaultPolicy(
    //policy => policy.AllowAnyHeader().AllowAnyOrigin()  //her s.a diyen siteye girebilir þeklinde bir ayarlama.
    policy => policy.WithOrigins("http://localhost:4200/", "https://localhost:4200/", "http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod() //böylece sadece burdaki arkadaþlar istek atabilirler API'ye.
)) ;

builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>()) //Kendi yazdığımız custom filter'ı devreye sokuyoruz.
    .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>()) //Böylece Application assembly'sindeki tüm Validator
//sınıflarını otomatik tarar ve bulur. Burda CreateProductValidator demiş olmamızında bir önemi yok. Aslında o assemblideki tüm sınıfları tarıyor.
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);    //Bu ayarlama ile default Filter'ın çalışmasını engellemiş oluyoruz
//Default filter ile validasyon hatası oluştuğunda .Net otomatik kendisi bize sormadan ErrorResponse üretip döndürüyor.

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

app.UseStaticFiles();
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
