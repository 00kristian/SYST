using Core;
using Microsoft.EntityFrameworkCore;

namespace Syst
{

    public class QuestionRepository : IQuestionRepository
    {
        ISystematicContext _context;
        public QuestionRepository(ISystematicContext context)
        {
            _context = context;
        }

        public async Task<(Status, int id)> Create(QuestionDTO questionDTO) {

            foreach (Question q in _context.questions) {
                if (q.Representation == q.Representation) return (Status.Conflict, q.Id);
            }
                var entity = new Question
                {
                    Representation = questionDTO.Representation!,
                    Answer = questionDTO.Answer!,
                    ImageURL = questionDTO.ImageURl,
                    Options = questionDTO.Options
                    
                };

                _context.questions.Add(entity);

                await _context.SaveChangesAsync();

                return (Status.Created, entity.Id);
        }


        public async Task<(Status, QuestionDTO)> Read(int id)
        {
            var q = await _context.questions.Where(q => q.Id == id).Select(q => new QuestionDTO(){
                Representation = q.Representation!,
                Id = q.Id,
                Answer = q.Answer!,
                ImageURl = q.ImageURL!,
                Options = q.Options!
            }).FirstOrDefaultAsync();

            if (q == default(QuestionDTO)) return (Status.NotFound, q);
            else return (Status.Found, q);
        }

         public async Task<IReadOnlyCollection<QuestionDTO>> ReadAll() =>
            await _context.questions.Select(q => new QuestionDTO(){
                Representation = q.Representation!,
                Id = q.Id,
                Answer = q.Answer!,
                ImageURl = q.ImageURL!,
                Options = q.Options!
            }).ToListAsync();

        public async Task<Status> Update(int id, QuestionDTO questionDTO)
        {
            var q = await _context.questions.Where(q => q.Id == id).FirstOrDefaultAsync();

            if (q == default(Question)) return Status.NotFound;

            q.Representation = questionDTO.Representation;
            q.Answer = questionDTO.Answer!;
            q.ImageURL = questionDTO.ImageURl;
            q.Options = questionDTO.Options;

            await _context.SaveChangesAsync();

            return Status.Updated;
        }

        public async Task<Status> Delete(int id){

            var q = await _context.questions.Where(q => q.Id == id).FirstOrDefaultAsync();
            
            if (q == default(Question)) return Status.NotFound;

            _context.questions.Remove(q);

            await _context.SaveChangesAsync();
            return Status.Deleted;
        }
    }
}