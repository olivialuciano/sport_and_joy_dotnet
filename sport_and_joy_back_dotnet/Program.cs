using AutoMapper;
using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using sport_and_joy_back_dotnet.Data;
using sport_and_joy_back_dotnet.Data.Repository.Implementations;
using sport_and_joy_back_dotnet.Data.Repository.Interfaces;
using sport_and_joy_back_dotnet.Profiles;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
//paraque no tire el error
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddControllers();

// Configuraci�n para la inyecci�n de dependencias de DinkToPdf -tema pdf
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
    //Esto va a permitir usar swagger con el token.
    setupAction.AddSecurityDefinition("SportAndJoyBearerAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Ac� pegar el token generado al loguearse."
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "SportAndJoyBearerAuth" } //Tiene que coincidir con el id seteado arriba en la definici�n
                }, new List<string>() }
    });
});

builder.Services.AddDbContext<SportContext>(dbContextOptions => dbContextOptions.UseSqlite(
    builder.Configuration["ConnectionStrings:SportAndJoyDBConnectionString"]));


builder.Services.AddAuthentication("Bearer") //"Bearer" es el tipo de auntenticaci�n que tenemos que elegir despu�s en PostMan para pasarle el token
    .AddJwtBearer(options => //Ac� definimos la configuraci�n de la autenticaci�n. le decimos qu� cosas queremos comprobar. La fecha de expiraci�n se valida por defecto.
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
        };


        /////////////////////////////////ac� para agregar el rol de usuario para poder ver luego de agarrarlo del front /////////////
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                // Agregar el rol como claim adicional al principal del usuario
                var identity = context.Principal.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    var roleClaim = context.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                    if (roleClaim != null)
                    {
                        // Obtener el rol del token y agregarlo como claim adicional
                        var role = roleClaim.Value;
                        identity.AddClaim(new Claim(ClaimTypes.Role, role));
                    }
                }

                return Task.CompletedTask;
            }
        };
        //////////////////////////////////////////////////

    }
);

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new FieldProfile());
    cfg.AddProfile(new UserProfile());
    cfg.AddProfile(new ReservationProfile());
});
var mapper = config.CreateMapper();

//#region DependencyInjections
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IFieldRepository, FieldRepository>();
//#endregion

//  CORS sino nunca iba a andar reina -para react 3000
builder.Services.AddCors(options => options.AddPolicy(name: "SportAndJoy",
    policy => {
        policy.WithOrigins("http://localhost:3000")
               .AllowAnyMethod()
               .AllowAnyHeader();
    }));

//esto despu�s del cors!
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("SportAndJoy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
