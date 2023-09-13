using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MiniETrade.API.Middlewares.ExceptionHandling;
using MiniETrade.Application;
using MiniETrade.Application.Validators.Products;
using MiniETrade.Infrastructure;
using MiniETrade.Infrastructure.Filters;
using MiniETrade.Persistence;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//MassTransit Registration
//builder.Services.AddMassTransitRegistration(builder.Configuration); TODO-HUS şirket internetinde olunca hata veriyor. Kapattık şimdilik.

// Add services to the container.
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddCors(options => options.AddDefaultPolicy(
    //policy => policy.AllowAnyHeader().AllowAnyOrigin()  //her s.a diyen siteye girebilir şeklinde bir ayarlama.
    policy => policy.WithOrigins("http://localhost:4200/", "https://localhost:4200/").AllowAnyHeader().AllowAnyMethod() //böylece sadece burdaki arkadaþlar istek atabilirler API'ye.
));

builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>()) //Kendi yazdığımız custom filter'ı devreye sokuyoruz.
    .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>()) //Böylece Application assembly'sindeki tüm Validator
//sınıflarını otomatik tarar ve bulur. Burda CreateProductValidator demiş olmamızında bir önemi yok. Aslında o assemblideki tüm sınıfları tarıyor.
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);    //Bu ayarlama ile default Filter'ın çalışmasını engellemiş oluyoruz
//Default filter ile validasyon hatası oluştuğunda .Net otomatik kendisi bize sormadan ErrorResponse üretip döndürüyor.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, //Oluþturulacak token deðerini kimlerin/hangi originlerin/sitelerin kullanýcý belirlediðimiz deðerdir. -> www.bilmemne.com
            ValidateIssuer = true, //Oluþturulacak token deðerini kimin daðýttýný ifade edeceðimiz alandýr. -> www.myapi.com
            ValidateLifetime = true, //Oluþturulan token deðerinin süresini kontrol edecek olan doðrulamadýr.
            ValidateIssuerSigningKey = true, //Üretilecek token deðerinin uygulamamýza ait bir deðer olduðunu ifade eden suciry key verisinin doðrulanmasýdýr.

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]!)),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null && expires > DateTime.UtcNow,

            NameClaimType = ClaimTypes.Name //JWT üzerinde Name claimne karşılık gelen değeri User.Identity.Name propertysinden elde edebiliriz.
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
 
if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage(); //TODO-HUS Bunu da bir test edelim bakam
else app.UseMiddleware<GlobalExceptionHandler>(); //TODO-HUS bakalım çalışıyor mu kardeşimiz.

app.UseStaticFiles();
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();