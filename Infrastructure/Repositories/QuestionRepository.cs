using Core;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{

    public class QuestionRepository : IQuestionRepository
    {
        ISystematicContext _context;
        public QuestionRepository(ISystematicContext context)
        {
            _context = context;
        }

        //Creates a question
        public async Task<(Status, int id)> Create(CreateQuestionDTO questionDTO) {
            
                var entity = new Question
                {
                    Representation = questionDTO.Representation!,
                    Answer = questionDTO.Answer!,
                    ImageURL = questionDTO.ImageURl,
                    Options = questionDTO.Options
                };

                _context.Questions.Add(entity);

                await _context.SaveChangesAsync();

                return (Status.Created, entity.Id);
        }

        //Return a question given the question id
        public async Task<(Status, QuestionDTO)> Read(int id)
        {
            var q = await _context.Questions.Where(q => q.Id == id).Select(q => new QuestionDTO(){
                Representation = q.Representation!,
                Id = q.Id,
                Answer = q.Answer!,
                ImageURl = q.ImageURL!,
                //Options = q.Options!
            }).FirstOrDefaultAsync();

            if (q == default(QuestionDTO)) return (Status.NotFound, q);
            else return (Status.Found, q);
        }

        //Return a list of all questions
         public async Task<IReadOnlyCollection<QuestionDTO>> ReadAll() =>
            await _context.Questions.Select(q => new QuestionDTO(){
                Representation = q.Representation!,
                Id = q.Id,
                Answer = q.Answer!,
                ImageURl = q.ImageURL!,
                //Options = q.Options!
            }).ToListAsync();

        //Updates a representation, answer, imageURL and options
        public async Task<Status> Update(int id, QuestionDTO questionDTO)
        {
            var q = await _context.Questions.Where(q => q.Id == id).FirstOrDefaultAsync();

            if (q == default(Question)) return Status.NotFound;

            q.Representation = questionDTO.Representation;
            q.Answer = questionDTO.Answer!;
            q.ImageURL = questionDTO.ImageURl;
            q.Options = questionDTO.Options;

            await _context.SaveChangesAsync();

            return Status.Updated;
        }
        
        //Deletes a question given the question id
        public async Task<Status> Delete(int id){

            var q = await _context.Questions.Where(q => q.Id == id).FirstOrDefaultAsync();
            
            if (q == default(Question)) return Status.NotFound;

            _context.Questions.Remove(q);

            await _context.SaveChangesAsync();
            return Status.Deleted;
        }
        
    }
}