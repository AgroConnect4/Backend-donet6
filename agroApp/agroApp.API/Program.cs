using agroApp.API.Services;
using agroApp.Domain.Entities;
using agroApp.Infra.Data.Context;
using agroApp.Infra.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens; 
using System.Text;
using agroApp.Domain;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Configurar Entity Framework com SQL Server e definir a assembly de migrações
//builder.Services.AddDbContext<AppDbContext>(options =>
//options.UseSqlServer("Data Source = DESKTOP-TE317V2\\SQLEXPRESS; Initial Catalog = AgroConnect; User ID = sa; Password = JGM%W37472022; Connect Timeout = 30;  Encrypt = False; Integrated Security = False;  TrustServerCertificate=True;",
//    b => b.MigrationsAssembly("agroApp.Infra.Data")));

// Configurar Entity Framework com MySQL e definir a assembly de migrações
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")),
                    b => b.MigrationsAssembly("agroApp.Infra.Data")));

// Configurar ASP.NET Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<SignInManager<User>>();

// Adicionar o serviço de autenticação (AuthService)
builder.Services.AddScoped<IAuthService, AuthService>();

//builder.Services.AddSingleton<IConfiguration>(Configuration);

builder.Services.AddScoped<IRoleRepository, RoleRepository>();

builder.Services.AddScoped<IEventsService, EventsService>(); 
// Adicionar o serviço de PostService
builder.Services.AddScoped<IPostService, PostService>();

// Registar o repositório de PostRepository
builder.Services.AddScoped<IPostRepository, PostRepository>();

// Registar o repositório de UserRepository
builder.Services.AddScoped<IUserRepository, UserRepository>(); // Adição necessária

builder.Services.AddScoped<IEventRepository, EventRepository>();

builder.Services.AddScoped<IConnectionRepository, ConnectionRepository>();

builder.Services.AddScoped<IConnectionService, ConnectionService>();

// No Program.cs ou Startup.cs
builder.Services.AddScoped<INotificationRepository, NotificationRepository>(); 

builder.Services.AddScoped<INotificationService, NotificationService>(); 

// ... dentro do ConfigureServices()
//builder.Services.AddScoped<ICommentService, CommentService>();

//builder.Services.AddScoped<ICommentRepository, CommentRepository>();

//builder.Services.AddScoped<IShareService, ShareService>(); 

//builder.Services.AddScoped<IShareRepository, ShareRepository>(); 

builder.Services.AddScoped<IEventCommentService, EventCommentService>();

builder.Services.AddScoped<IEventCommentRepository, EventCommentRepository>();

builder.Services.AddScoped<IEventShareService, EventShareService>();

builder.Services.AddScoped<IEventShareRepository, EventShareRepository>();

builder.Services.AddScoped<IPostCommentService,PostCommentService>();

builder.Services.AddScoped<IPostCommentRepository, PostCommentRepository>();

builder.Services.AddScoped<IPostShareService, PostShareService>();

builder.Services.AddScoped<IPostShareRepository, PostShareRepository>();

// Adicionar serviços de controladores
builder.Services.AddControllers();

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddUserSecrets<Program>();

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//        };
//    });

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddSwaggerGen(c =>
{



    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = ""
    });


    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configuração do pipeline de requisição HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Ativar autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
