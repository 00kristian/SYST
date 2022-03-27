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

            await SeedObjectsAsync(context);
        }
        return host;
    }

    private static async Task SeedObjectsAsync(SystematicContext context)
    {   
        //Admin seeding
        if (context.Candidates.Count() > 1) return;

        await context.Database.MigrateAsync();

        if (!await context.Admins.AnyAsync())
        {

            context.Admins.AddRange(
                new Admin() {Name = "Bob Bobsen", Email = "bob@bobsen.com", Events = new List<Event> {}}
            );

            var lukas = new Candidate() {Name = "Lukas Hjelmstrand", Email = "luhj@itu.dk", StudyProgram = "Bsc i Softwareudvikling", University = UniversityEnum.ITU};
            var rene = new Candidate() {Name = "Rene Dif", Email = "rene@dif.dk", StudyProgram = "Msc i Vand", University = UniversityEnum.CBS};
            var isabella = new Candidate() {Name = "Isabella Magnusdottir", Email = "isab3ll4@gmail.com", StudyProgram = "PHD i Sexologi", University = UniversityEnum.RUC};
            var camille = new Candidate() {Name = "Camille Gonnsen", Email = "camg@itu.dk", StudyProgram = "Bsc i Rødhårethed", University = UniversityEnum.ITU};
            var berlin = new Candidate() {Name = "Kristian Berlin Jensen", Email = "berlin@itu.dk", StudyProgram = "Bsc i Tysklandsstudier", University = UniversityEnum.KU};
            


            context.Candidates.AddRange(
                lukas,
                rene,
                isabella,
                camille,
                berlin
                );
          
            var date1 = new System.DateTime(2022,03,14);
            var date2 = new System.DateTime(2022,03,26);
            var date3 = new System.DateTime(2022,09,23);
            var date4 = new System.DateTime(2022,06,30);
            var date5 = new System.DateTime(2022,11,21);
            var date6 = new System.DateTime(2022,02,14);
            var date7 = new System.DateTime(2022,01,31);
            var date8 = new System.DateTime(2022,02,18);
            var date9 = new System.DateTime(2022,12,24);
            var date10 = new System.DateTime(2022,10,31);

            context.Events.AddRange(
                new Event() {Name = "IT Konference", Date = date1, Location = "Århus", Candidates = new List<Candidate> {}, Quiz = null, Rating = 3.7},
                new Event() {Name = "Match Making hos SDU", Date = date2, Location = "Odense", Candidates = new List<Candidate> {}, Quiz = null, Rating = 2.5},
                new Event() {Name = "Job messe på ITU", Date = date3, Location = "København", Candidates = new List<Candidate> {}, Quiz = null, Rating = 5.0},
                new Event() {Name = "IT-DAY København", Date = date4, Location = "København", Candidates = new List<Candidate> {}, Quiz = null, Rating = 4.6},
                new Event() {Name = "JOB BootCamp", Date = date5, Location = "Århus", Candidates = new List<Candidate> {}, Quiz = null, Rating = 3.8},
                new Event() {Name = "IT-DAY Odense", Date = date6, Location = "Odense", Candidates = new List<Candidate> {}, Quiz = null, Rating = 4.1},
                new Event() {Name = "Career Week hos DTU", Date = date7, Location = "Kgs. Lyngby", Candidates = new List<Candidate> {}, Quiz = null, Rating = 4.4},
                new Event() {Name = "IT week på RUC", Date = date8, Location = "Roskilde", Candidates = new List<Candidate> {}, Quiz = null, Rating = 3.1},
                new Event() {Name = "Job messe i Århus", Date = date9, Location = "Århus", Candidates = new List<Candidate> {}, Quiz = null, Rating = 3.6},
                new Event() {Name = "IT-DAY Aalborg", Date = date10, Location = "Aalborg", Candidates = new List<Candidate> {}, Quiz = null, Rating = 4.3}
            );

            await context.SaveChangesAsync();
        }
    }
}