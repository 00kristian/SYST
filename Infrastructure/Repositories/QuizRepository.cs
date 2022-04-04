using Core;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{

    public class QuizRepository : IQuizRepository
    {
        ISystematicContext _context;
        public QuizRepository(ISystematicContext context)
        {
            _context = context;
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

            var c = await _context.Quizes.Where(c => c.Id == id).FirstOrDefaultAsync();
            
            if (c == default(Quiz)) return Status.NotFound;

            _context.Quizes.Remove(c);

            await _context.SaveChangesAsync();
            return Status.Deleted;
        }

        //Return a quiz given the quiz id
        public async Task<(Status, QuizDTO)> Read(int id)
        {
            
            var quiz = await _context.Quizes.Where(q => q.Id == id).Select(q => new QuizDTO(){
                Id = q.Id,
              
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
    }
}