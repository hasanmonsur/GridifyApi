using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using WebApiGridify.Contacts;
using WebApiGridify.Helpers;
using WebApiGridify.Repositorys;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();


builder.Services.AddCors();

//builder.Host.UseSerilog((hostContext, services, configuration) =>
//{

//     hostContext.Configuration.GetSection("Logging")   ;


//});

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//})

//builder.Services.AddSwaggerGen();

//builder.Services.AddAuthentication("BasicAuthentication")
//                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
//                ("BasicAuthentication", null);


// Adding Jwt Bearer
//.AddJwtBearer(options =>
//{
//    options.SaveToken = true;
//    options.RequireHttpsMetadata = false;
//    options.TokenValidationParameters = new TokenValidationParameters()
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidAudience = configuration["JWT:ValidAudience"],
//        ValidIssuer = configuration["JWT:ValidIssuer"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
//    };
//});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OTP API",
        Version = "v1",
        Description = "SBL API Services.",
        Contact = new OpenApiContact
        {
            Name = "Sonali Bank Limited"
        },
    });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
    {
        Description = "Basic auth added to authorization header",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "basic",
        Type = SecuritySchemeType.Http
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Basic" }
                    },
                    new List<string>()
                }
            });
});

builder.Services.AddAuthorization();

builder.Services.AddControllers().AddJsonOptions(x =>
{
    // serialize enums as strings in api responses (e.g. Role)
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
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



app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

// global error handler
//app.UseMiddleware<ErrorHandlerMiddleware>();

// custom jwt auth middleware
//app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
