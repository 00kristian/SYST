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
          
            var date3 = new System.DateTime(2022,03,14);
            var date4 = new System.DateTime(2022,05,18);
            var date5 = new System.DateTime(2022,09,23);

            var quiz2 = new Quiz {Date = date2, Questions = new List<Question> {}, Events = new List<Event> {}, Candidates = new List<Candidate> {}};

            context.Events.AddRange(
                new Event() {Name = "IT Konference", Date = date3, Location = "Århus", Candidates = new List<Candidate> {maj, benjamin, emma}, Quiz = null, Rating = 3.7},
                new Event() {Name = "SDU - Match Making", Date = date4, Location = "Odense", Candidates = new List<Candidate> {isabella, kristian, benjamin}, Quiz = null, Rating = 2.5},
                new Event() {Name = "ITU - Job messe", Date = date5, Location = "København", Candidates = new List<Candidate> {lukas, isabella, camille, kristian, peter}, Quiz = quiz2, Rating = 5.0}
            );

            await context.SaveChangesAsync();
        }
    }
}