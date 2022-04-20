using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builderDB = new DbContextOptionsBuilder<SystematicContext>();
        
using (var db = new SystematicContextFactory().CreateDbContext(new string[0]))
{
    db.Database.EnsureCreated();
    var repoCandidate = new CandidateRepository(db);
    var repoAdmin = new AdminRepository(db);
    var repoEvent = new EventRepository(db);
    var repoQuestions = new QuestionRepository(db, "");
    var repoQuiz = new QuizRepository(db, "");
}