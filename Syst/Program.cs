using Microsoft.EntityFrameworkCore;
using Syst;

var builder = WebApplication.CreateBuilder(args);

var builderDB = new DbContextOptionsBuilder<SystematicContext>();
        
using (var db = new SystematicContextFactory().CreateDbContext(new string[0]))
{
    db.Database.EnsureCreated();
    var repoCandidate = new CandidateRepository(db);
    var repoAdmin = new AdminRepository(db);
    var repoEvent = new EventRepository(db);
    var repoQuestions = new QuestionRepository(db);
    var repoQuiz = new QuizRepository(db);
}

// Add services to the container.

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");;

app.Run();
