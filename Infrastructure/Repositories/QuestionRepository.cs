using Core;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{

    public class QuestionRepository : IQuestionRepository
    {
        ISystematicContext _context;
        string _hostEnvPath;
        public QuestionRepository(ISystematicContext context, string hostEnvPath)
        {
            _context = context;
            //this is so we can clean up unused question images
            _hostEnvPath = hostEnvPath;
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
                Options = q.Options!
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
                Options = q.Options!
            }).ToListAsync();

        //Updates a representation, answer, imageURL and options
        public async Task<Status> Update(int id, CreateQuestionDTO questionDTO)
        {
            var q = await _context.Questions.Where(q => q.Id == id).FirstOrDefaultAsync();

            if (q == default(Question)) return Status.NotFound;

            q.Representation = questionDTO.Representation;
            q.Answer = questionDTO.Answer!;
            q.Options = questionDTO.Options;

            await _context.SaveChangesAsync();

            return Status.Updated;
        }
        
        //Deletes a question given the question id
        public async Task<Status> Delete(int id){

            var question = await _context.Questions.Where(q => q.Id == id).FirstOrDefaultAsync();
            
            if (question == default(Question)) return Status.NotFound;

            var quizzesCount = await _context.Questions.Where(q => q.Id != id && q.ImageURL == question.ImageURL).CountAsync();
            if (quizzesCount !> 0) question.CleanUpImage(_hostEnvPath);

            _context.Questions.Remove(question);

            await _context.SaveChangesAsync();
            return Status.Deleted;
        }

        public async Task<Status> UpdateImage(int id, string imageUrl)
        {
            var question = await _context.Questions.Where(q => q.Id == id).FirstOrDefaultAsync();

            if (question == default(Question)) return Status.NotFound;

            var quizzesCount = await _context.Questions.Where(q => q.Id != id && q.ImageURL == question.ImageURL).CountAsync();
            if (quizzesCount !> 0) question.CleanUpImage(_hostEnvPath);

            question.ImageURL = imageUrl;

            await _context.SaveChangesAsync();

            return Status.Updated;
        }
    }
}