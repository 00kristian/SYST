using Core;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{

    public class QuizRepository : IQuizRepository
    {
        ISystematicContext _context;

        string _hostEnvPath;
        public QuizRepository(ISystematicContext context, string hostEnvPath)
        {
            _context = context;

            _hostEnvPath = hostEnvPath;
        }

        //Creates a quiz
        public async Task<(Status, int id)> Create(QuizCreateDTO quizDTO)
        {
            if (quizDTO == default(QuizCreateDTO)) return (Status.Conflict, 0);
            //TODO: test if dto is null
            var entity = new Quiz
                {
                    Name = quizDTO.Name,
                    // Questions = quizDTO.Questions.Select(qs => new Question {
                    //     Representation = qs.Representation,
                    //     Answer = qs.Answer,
                    //     ImageURL = qs.ImageURl,
                    //     Options = qs.Options
                    // }).ToList()
                };

            _context.Quizes.Add(entity);

            await _context.SaveChangesAsync();

            return (Status.Created, entity.Id);
        }

        //Return a list of all quizes
         public async Task<IReadOnlyCollection<QuizDTO>> ReadAll() =>
            await _context.Quizes.Select(q => new QuizDTO(){
                Id = q.Id,
                Name = q.Name!
            }).ToListAsync();
            //TODO: Do we need this if we can get the quizes via events? 


        //Deletes a quiz given the quiz id
        public async Task<Status> Delete(int id){

            var c = await _context.Quizes.Include(q => q.Events).Include(s => s.Questions).Include(c => c.Candidates).Where(c => c.Id == id).FirstOrDefaultAsync();
           
            if (c == default(Quiz)) return Status.NotFound;

            if (c.Questions != null) {
                foreach (var question in c.Questions)
                {
                    question.CleanUpImage(_hostEnvPath);
                    _context.Questions.Remove(question);
                }
            }
                
            _context.Quizes.Remove(c);

            await _context.SaveChangesAsync();
            return Status.Deleted;
        }

        //Return a quiz given the quiz id
        public async Task<(Status, QuizDTO)> Read(int id)
        {
            
            var quiz = await _context.Quizes.Include(q => q.Questions).Where(q => q.Id == id).Select(q => new QuizDTO(){
                Id = q.Id,
                Name = q.Name!,
                Questions = q.Questions!.Select(qs => new QuestionDTO {
                    Id = qs.Id,
                    Representation = qs.Representation!
                }).ToList()
                
            }).FirstOrDefaultAsync();

            if (quiz == default(QuizDTO)) return (Status.NotFound, quiz);
            else return (Status.Found, quiz);
        }
        
        //Updates a quiz date value
        public async Task<Status> Update(int id, QuizCreateDTO quizDTO)
        {
            var q = await _context.Quizes.Where(q => q.Id == id).FirstOrDefaultAsync();

            if (q == default(Quiz)) return Status.NotFound;

            q.Name = quizDTO.Name;
            // q.Questions = quizDTO.Questions.Select(qs => new Question {
            //     Representation = qs.Representation,
            //     Answer = qs.Answer,
            //     ImageURL = qs.ImageURl,
            //     Options = qs.Options
            // }).ToList();
    

            await _context.SaveChangesAsync();

            return Status.Updated;
        }

        //TODO: Test this!
        public async Task<(Status, int id)> AddQuestion(int id, CreateQuestionDTO question)
        {
            var q = await _context.Quizes.Include(q => q.Questions).Where(q => q.Id == id).FirstOrDefaultAsync();

            if (q == default(Quiz)) return (Status.NotFound, -1);

            if (q.Questions == null) {
                q.Questions = new List<Question>();
            }

            var ques = new Question() {
                Representation = question.Representation,
                Answer = question.Answer,
                ImageURL = question.ImageURl,
                Options = question.Options
            };

            q.Questions.Add(ques);

            await _context.SaveChangesAsync();

            return (Status.Updated, ques.Id);
        }

        //TODO: Test this!
        public async Task<Status> RemoveQuestion(int quizId, int questionId)
        {
            var quiz = await _context.Quizes.Include(q => q.Questions).Where(q => q.Id == quizId).FirstOrDefaultAsync();

            if (quiz == default(Quiz)) return Status.NotFound;

            var ques = quiz.Questions!.Where(q => q.Id == questionId).FirstOrDefault();

            if (ques == default(Question)) return Status.NotFound;

            quiz.Questions!.Remove(ques);

            return(Status.Updated);
        }
    }
}