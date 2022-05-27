using Core;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Server.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SystematicContext>(options => options.UseSqlServer("Server=localhost;Database=EventToolDB;User Id=sa;Password=TilJuleBalINisseLand10000;Trusted_Connection=False;Encrypt=False",
builder => {
    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
}));
builder.Services.AddScoped<ISystematicContext, SystematicContext>();
builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
//add environment path so we can delete images
var envPath = builder.Environment.ContentRootPath;
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>(r => new QuestionRepository(r.GetRequiredService<ISystematicContext>(), envPath));
builder.Services.AddScoped<IQuizRepository, QuizRepository>(r => new QuizRepository(r.GetRequiredService<ISystematicContext>(), envPath));
builder.Services.AddHostedService<CandidateRemover>();

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));


builder.Services.Configure<JwtBearerOptions>(
    JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters.NameClaimType = "name";
    });

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {  
        Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows() {
                Implicit = new OpenApiOAuthFlow() {
                    AuthorizationUrl = new Uri("https://login.microsoftonline.com/55f0e7d6-df83-4287-a2bc-b7c2a11f8243/oauth2/v2.0/authorize"),  
                        TokenUrl = new Uri("https://login.microsoftonline.com/55f0e7d6-df83-4287-a2bc-b7c2a11f8243/oauth2/v2.0/token"),  
                        Scopes = new Dictionary < string, string > {  
                            {  
                                "api://18686055-5912-4c57-a1c9-0bb76dde9d96/API.Access", "Event Tool API"
                            }
                        }  
                }
            }
    });

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement() {  
        {  
            new OpenApiSecurityScheme {  
                Reference = new OpenApiReference {  
                        Type = ReferenceType.SecurityScheme,  
                            Id = "oauth2"  
                    },  
                    Scheme = "oauth2",  
                    Name = "oauth2",  
                    In = ParameterLocation.Header  
            },  
            new List < string > ()  
        }  
    }); 
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c => {  
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AzureAD_OAuth_API v1");  
    //c.RoutePrefix = string.Empty;    
    c.OAuthClientId("18686055-5912-4c57-a1c9-0bb76dde9d96");  
    c.OAuthClientSecret("Random");  
    c.OAuthUseBasicAuthenticationWithAccessCodeGrant();  
}); 

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");;


/*To add seedings, uncomment the line below.*/
//await app.SeedAsync();

app.Run();
