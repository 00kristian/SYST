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
          
<<<<<<< HEAD
            var date3 = new System.DateTime(2022,03,14);
            var date4 = new System.DateTime(2022,05,18);
            var date5 = new System.DateTime(2022,09,23);

            var quiz2 = new Quiz {Date = date2, Questions = new List<Question> {}, Events = new List<Event> {}, Candidates = new List<Candidate> {}};

            context.Events.AddRange(
                new Event() {Name = "IT Konference", Date = date3, Location = "Århus", Candidates = new List<Candidate> {maj, benjamin, emma}, Quiz = null, Rating = 3.7},
                new Event() {Name = "SDU - Match Making", Date = date4, Location = "Odense", Candidates = new List<Candidate> {isabella, kristian, benjamin}, Quiz = null, Rating = 2.5},
                new Event() {Name = "ITU - Job messe", Date = date5, Location = "København", Candidates = new List<Candidate> {lukas, isabella, camille, kristian, peter}, Quiz = quiz2, Rating = 5.0}
=======
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
>>>>>>> 75c8900c60d7aee516b8f731a2276314acb50163
            );

            await context.SaveChangesAsync();
        }
    }
}