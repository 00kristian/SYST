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

         public async Task<IReadOnlyCollection<QuizDTO>> ReadAll() =>
            await _context.Candidates.Select(c => new QuizDTO(){
                Id = c.Id
            }).ToListAsync();


        public async Task<Status> Delete(int id){

            var c = await _context.Quizes.Where(c => c.Id == id).FirstOrDefaultAsync();
            
            if (c == default(Quiz)) return Status.NotFound;

            _context.Quizes.Remove(c);

            await _context.SaveChangesAsync();
            return Status.Deleted;
        }

        public async Task<(Status, int id)> Create(QuizDTO quizDTO)
        {
             var entity = new Quiz
                {
                    Date = quizDTO.Date
                };

                _context.Quizes.Add(entity);

                await _context.SaveChangesAsync();

                return (Status.Created, entity.Id);
        }

        public async Task<(Status, QuizDTO)> Read(int id)
        {
            
            var quiz = await _context.Quizes.Where(q => q.Id == id).Select(q => new QuizDTO(){
                Id = q.Id,
                Date = q.Date
              
            }).FirstOrDefaultAsync();

            if (quiz == default(QuizDTO)) return (Status.NotFound, quiz);
            else return (Status.Found, quiz);
        }

        public async Task<Status> Update(int id, QuizDTO quizDTO)
        {
            var q = await _context.Quizes.Where(q => q.Id == id).FirstOrDefaultAsync();

            if (q == default(Quiz)) return Status.NotFound;

            q.Date = quizDTO.Date;

            await _context.SaveChangesAsync();

            return Status.Updated;
        }
    }
}