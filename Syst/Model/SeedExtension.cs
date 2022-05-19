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
    private static Random random = new Random();

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private static string[] Universities = new string[] {
            "Aalborg University",
            "Aarhus University",
            "Copenhagen Business School",
            "IT-University of Copenhagen",
            "Roskilde University",
            "Technical University of Denmark",
            "University of Copenhagen",
            "University of Southern Denmark",
            "Other"};

    private static async Task SeedObjectsAsync(SystematicContext context)
    {   
        //Admin seeding
        if (context.Candidates.Count() > 1) return;
        await context.Database.MigrateAsync();

        if (!await context.Candidates.AnyAsync())
        {

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

            var lukas = new Candidate() {Name = "Lukas Hjelmstrand", Email = "luhj@itu.dk", CurrentDegree = "BSc", StudyProgram = "Softwareudvikling", University = "IT-University of Copenhagen", GraduationDate = date0, Created = DateTime.Now, PercentageOfCorrectAnswers = 100, Answer = null};
            var maj = new Candidate() {Name = "Maj Frost Jensen", Email = "mfje@itu.dk", CurrentDegree = "BSc",StudyProgram = "Softwareudvikling", University = "IT-University of Copenhagen", GraduationDate = date0, Created = DateTime.Now, PercentageOfCorrectAnswers = 100, Answer = null};
            var isabella = new Candidate() {Name = "Isabella Magnusdottir", Email = "imag@itu.dk", CurrentDegree = "BSc", StudyProgram = "Softwareudvikling", University = "IT-University of Copenhagen", GraduationDate = date2, Created = DateTime.Now, PercentageOfCorrectAnswers = 100, Answer = null};
            var camille = new Candidate() {Name = "Camille Gonnsen", Email = "cgon@itu.dk", CurrentDegree = "BSc", StudyProgram = "Softwareudvikling", University = "IT-University of Copenhagen", GraduationDate = date1, Created = DateTime.Now, PercentageOfCorrectAnswers = 20, Answer = null};
            var kristian = new Candidate() {Name = "Kristian Berlin Jensen", Email = "krbj@itu.dk", CurrentDegree = "BSc", StudyProgram = "Softwareudvikling", University = "IT-University of Copenhagen", GraduationDate = date0, Created = DateTime.Now, PercentageOfCorrectAnswers = 20, Answer = null};
            var sarah = new Candidate() {Name = "Sarah Christiansen", Email = "sacc@itu.dk", CurrentDegree = "BSc", StudyProgram = "Softwareudvikling", University = "IT-University of Copenhagen", GraduationDate = date2, Created = DateTime.Now, PercentageOfCorrectAnswers = 20, Answer = null};
            var emma = new Candidate() {Name = "Emma Hansen", Email = "emha@ku.dk", CurrentDegree = "BSc", StudyProgram = "Datalogi", University = "University of Copenhagen", GraduationDate = date1, Created = DateTime.Now, PercentageOfCorrectAnswers = 0, Answer = null};
            var sebastian = new Candidate(){Name = "Sebastian Nielsen", Email = "seni@ruc.dk", CurrentDegree = "PhD", StudyProgram = "Humunistisk Teknologi", University= "Roskilde University", GraduationDate = date1, Created = DateTime.Now, PercentageOfCorrectAnswers = 40, Answer = null}; 
            var benjamin = new Candidate(){Name = "Benjamin Christensen", Email = "belc@itu.dk", CurrentDegree = "MSc", StudyProgram = "Computer Science", University = "IT-University of Copenhagen", GraduationDate = date2, Created = DateTime.Now, PercentageOfCorrectAnswers = 40, Answer = null};
            var peter = new Candidate(){Name = "Peter Hansen", Email = "peha@sdu.dk", CurrentDegree = "MSc", StudyProgram = "Webkommunikation", University = "University of Southern Denmark", GraduationDate = date2, Created = DateTime.Now, PercentageOfCorrectAnswers = 40, Answer = null};

            for (int i = 0; i < 1000; i++)
            {
                var c = new Candidate() {
                    Name = RandomString(5) + " " + RandomString(8),
                    Email = RandomString(8) + "@gmail.com",
                    CurrentDegree = "BSc",
                    StudyProgram = "Softwareudvikling",
                    University = Universities[random.Next(Universities.Length)],
                    GraduationDate = DateTime.Now.AddYears(1),
                    Created = DateTime.Now,
                    PercentageOfCorrectAnswers = random.Next(6) * 20,
                    Answer = null
                };
                context.Candidates.Add(c);
            }

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

            var question1 = new Question() {Representation = "What is the outcome of this code?", Answer = "I am a car", Options = new List<string>() {"I am a car", "I am a vehicle"}};
            var question2 = new Question() {Representation = "Is this good convention?", Answer = "Maybe", Options = new List<string>() {"Yes", "No", "Maybe"}};

            context.Questions.AddRange(question1, question2);

            var quiz1 = new Quiz() {Name = "IT Quiz 1", Questions = new List<Question>() {question1, question2}};

            context.Quizes.Add(quiz1);

            context.Events.AddRange(
                new Event() {Name = "IT Konference", Date = date3, Location = "Århus", Candidates = new List<Candidate> {maj, kristian, sarah, sebastian}, Quiz = quiz1, Rating = 3.7, Winners=null},
                new Event() {Name = "Match Making hos SDU", Date = date4, Location = "Odense", Candidates = new List<Candidate> {peter}, Quiz = null, Rating = 2.5, Winners=null},
                new Event() {Name = "Job messe på ITU", Date = date5, Location = "København", Candidates = new List<Candidate> {lukas, maj, isabella, camille, kristian, sarah, benjamin}, Quiz = null, Rating = 5.0, Winners=null},
                new Event() {Name = "IT-DAY København", Date = date6, Location = "København", Candidates = new List<Candidate> {emma, lukas, isabella, benjamin}, Quiz = null, Rating = 4.6, Winners=null},
                new Event() {Name = "JOB BootCamp", Date = date7, Location = "Århus", Candidates = new List<Candidate> {kristian, isabella, lukas}, Quiz = null, Rating = 3.8, Winners=null},
                new Event() {Name = "IT-DAY Odense", Date = date8, Location = "Odense", Candidates = new List<Candidate> {maj, camille, benjamin}, Quiz = null, Rating = 4.1, Winners=null},
                new Event() {Name = "Career Week hos DTU", Date = date9, Location = "Kgs. Lyngby", Candidates = new List<Candidate> {sarah, camille, kristian, peter}, Quiz = null, Rating = 4.4, Winners=null},
                new Event() {Name = "IT week på RUC", Date = date10, Location = "Roskilde", Candidates = new List<Candidate> {sebastian}, Quiz = null, Rating = 3.1, Winners=null},
                new Event() {Name = "Job messe i Århus", Date = date11, Location = "Århus", Candidates = new List<Candidate> {isabella, emma, maj}, Quiz = null, Rating = 3.6, Winners=null},
                new Event() {Name = "IT-DAY Aalborg", Date = date12, Location = "Aalborg", Candidates = new List<Candidate> {benjamin, lukas, sebastian, sarah}, Quiz = null, Rating = 4.3, Winners=null}
            );

            await context.SaveChangesAsync();
        }
    }
}