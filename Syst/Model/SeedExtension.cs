using Infrastructure;
using Microsoft.EntityFrameworkCore;

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

            var date0 = new System.DateTime(2023,05,30);
            var date1 = new System.DateTime(2022,06,01);
            var date2 = new System.DateTime(2024,05,30);
            var date3 = new System.DateTime(2022,03,14);
            var date4 = new System.DateTime(2022,05,18);
            var date5 = new System.DateTime(2022,09,23);
            var date6 = new System.DateTime(2022,06,30);
            var date7 = new System.DateTime(2022,11,21);
            var date8 = new System.DateTime(2022,02,14);
            var date9 = new System.DateTime(2022,01,31);
            var date10 = new System.DateTime(2022,02,18);
            var date11 = new System.DateTime(2022,03,24);
            var date12 = new System.DateTime(2022,10,31);

            var lukas = new Candidate() {Name = "Lukas Hjelmstrand", Email = "luhj@itu.dk", StudyProgram = "Bsc i Softwareudvikling", University = UniversityEnum.ITU, GraduationDate = date0};
            var maj = new Candidate() {Name = "Maj Frost Jensen", Email = "mfje@itu.dk", StudyProgram = "Bsc i Softwareudvikling", University = UniversityEnum.ITU, GraduationDate = date0};
            var isabella = new Candidate() {Name = "Isabella Magnusdottir", Email = "imag@itu.dk", StudyProgram = "Bsc i Softwareudvikling", University = UniversityEnum.ITU, GraduationDate = date2};
            var camille = new Candidate() {Name = "Camille Gonnsen", Email = "cgon@itu.dk", StudyProgram = "Bsc i Softwareudvikling", University = UniversityEnum.ITU, GraduationDate = date1};
            var kristian = new Candidate() {Name = "Kristian Berlin Jensen", Email = "krbj@itu.dk", StudyProgram = "Bsc i Softwareudvikling", University = UniversityEnum.ITU, GraduationDate = date0};
            var sarah = new Candidate() {Name = "Sarah Christiansen", Email = "sacc@itu.dk", StudyProgram = "Bsc i Softwareudvikling", University = UniversityEnum.ITU, GraduationDate = date2};
            var emma = new Candidate() {Name = "Emma Hansen", Email = "emha@ku.dk", StudyProgram = "Bsc i Datalogi", University = UniversityEnum.KU, GraduationDate = date1};
            var sebastian = new Candidate(){Name = "Sebastian Nielsen", Email = "seni@ruc.dk", StudyProgram = "Bsc i Humunistisk Teknologi", University= UniversityEnum.RUC, GraduationDate = date1}; 
            var benjamin = new Candidate(){Name = "Benjamin Christensen", Email = "belc@itu.dk", StudyProgram = "Msc i Computer Science", University = UniversityEnum.ITU, GraduationDate = date2};
            var peter = new Candidate(){Name = "Peter Hansen", Email = "peha@sdu.dk", StudyProgram = "Msc i Webkommunikation", University = UniversityEnum.SDU, GraduationDate = date2};

            context.Candidates.AddRange(
                lukas,
                maj,
                isabella,
                camille,
                kristian,
                sarah,
                emma,
                sebastian,
                benjamin,
                peter
                );

            context.Events.AddRange(
                new Event() {Name = "IT Konference", Date = date3, Location = "Århus", Candidates = new List<Candidate> {maj, kristian, sarah, sebastian}, Quiz = null, Rating = 3.7},
                new Event() {Name = "Match Making hos SDU", Date = date4, Location = "Odense", Candidates = new List<Candidate> {peter}, Quiz = null, Rating = 2.5},
                new Event() {Name = "Job messe på ITU", Date = date5, Location = "København", Candidates = new List<Candidate> {lukas, maj, isabella, camille, kristian, sarah, benjamin}, Quiz = null, Rating = 5.0},
                new Event() {Name = "IT-DAY København", Date = date6, Location = "København", Candidates = new List<Candidate> {emma, lukas, isabella, benjamin}, Quiz = null, Rating = 4.6},
                new Event() {Name = "JOB BootCamp", Date = date7, Location = "Århus", Candidates = new List<Candidate> {kristian, isabella, lukas}, Quiz = null, Rating = 3.8},
                new Event() {Name = "IT-DAY Odense", Date = date8, Location = "Odense", Candidates = new List<Candidate> {maj, camille, benjamin}, Quiz = null, Rating = 4.1},
                new Event() {Name = "Career Week hos DTU", Date = date9, Location = "Kgs. Lyngby", Candidates = new List<Candidate> {sarah, camille, kristian, peter}, Quiz = null, Rating = 4.4},
                new Event() {Name = "IT week på RUC", Date = date10, Location = "Roskilde", Candidates = new List<Candidate> {sebastian}, Quiz = null, Rating = 3.1},
                new Event() {Name = "Job messe i Århus", Date = date11, Location = "Århus", Candidates = new List<Candidate> {isabella, emma, maj}, Quiz = null, Rating = 3.6},
                new Event() {Name = "IT-DAY Aalborg", Date = date12, Location = "Aalborg", Candidates = new List<Candidate> {benjamin, lukas, sebastian, sarah}, Quiz = null, Rating = 4.3}
            );

            await context.SaveChangesAsync();
        }
    }
}