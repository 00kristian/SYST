using System.Threading;
using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Core;

namespace Server.Model;

public static class SeedExtensions
{
    public static async Task<IHost> SeedAsync(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<SystematicContext>();

           // await SeedObjectsAsync(context);
        }
        return host;
    }

    private static async Task SeedObjectsAsync(SystematicContext context)
    {   
        //Admin seeding
        if (context.Admins.Count() > 1) return;

        //await context.Database.MigrateAsync();

        if (!await context.Admins.AnyAsync())
        {

            context.Admins.AddRange(
                new Admin() {Name = "Bob Bobsen", Email = "bob@bobsen.com", Events = new List<Event> {}}
            );

            context.Candidates.AddRange(
                new Candidate() {Name = "Lukas Hjelmstrand", Email = "luhj@itu.dk", StudyProgram = "Bsc i Softwareudvikling", University = UniversityEnum.ITU},
                new Candidate() {Name = "Rene Dif", Email = "rene@dif.dk", StudyProgram = "Msc i Vand", University = UniversityEnum.CBS},
                new Candidate() {Name = "Isabella Magnusdottir", Email = "isab3ll4@gmail.com", StudyProgram = "PHD i Sexologi", University = UniversityEnum.RUC},
                new Candidate() {Name = "Camille Gonnsen", Email = "camg@itu.dk", StudyProgram = "Bsc i Rødhårethed", University = UniversityEnum.ITU},
                new Candidate() {Name = "Kristian Berlin Jensen", Email = "berlin@itu.dk", StudyProgram = "Bsc i Tysklandsstudier", University = UniversityEnum.KU}
            );
          
            //var date0 = new System.DateTime(2022,03,14);
            //var date1 = new System.DateTime(2022,05,18);
            var date2 = new System.DateTime(2022,09,23);

            var quiz2 = new Quiz {Date = date2, Questions = new List<Question> {}, Events = new List<Event> {}, Candidates = new List<Candidate> {}};

            context.Events.AddRange(
                //new Event() {Name = "IT Konference", Date = date0, Location = "Århus", Candidates = new List<Candidate> {}, Quiz = null, Rating = 3.7},
                //new Event() {Name = "SDU - Match Making", Date = date1, Location = "Odense", Candidates = new List<Candidate> {}, Quiz = null, Rating = 2.5},
                new Event() {Name = "ITU - Job messe", Date = date2, Location = "København", Candidates = new List<Candidate> {}, Quiz = quiz2, Rating = 5.0}
            );

            await context.SaveChangesAsync();
        }
    }
}