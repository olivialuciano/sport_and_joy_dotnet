using AutoMapper;
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




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
    //Esto va a permitir usar swagger con el token.
    setupAction.AddSecurityDefinition("SportAndJoyBearerAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Token:"
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "SportAndJoyBearerAuth" } //Tiene que coincidir con el id seteado arriba en la definicion
                }, new List<string>() }
    });
});

builder.Services.AddDbContext<SportContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SportAndJoyDBConnectionString"));

});


builder.Services.AddAuthentication("Bearer") //"Bearer" es el tipo de auntenticacion que tenemos que elegir despues en PostMan para pasarle el token
    .AddJwtBearer(options => //Aca definimos la configuracion de la autenticacion. le decimos que cosas queremos comprobar. La fecha de expiracion se valida por defecto.
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


        /////////////////////////////////aca para agregar el rol de usuario para poder ver luego de agarrarlo del front /////////////
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
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
});


//esto despues del cors!
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//app.UseCors("SportAndJoy");

app.UseCors("AllowAll");


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();