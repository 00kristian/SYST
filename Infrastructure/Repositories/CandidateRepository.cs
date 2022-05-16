using Core;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{

    public class CandidateRepository : ICandidateRepository
    {
        ISystematicContext _context;
        public CandidateRepository(ISystematicContext context)
        {
            _context = context;
        }

        //Creates an candidate
        public async Task<(Status, int id)> Create(CreateCandidateDTO candidateDTO) {

            DateTime currentDate = DateTime.Today;
            var dupe = _context.Candidates.Where(c => c.Email == candidateDTO.Email).FirstOrDefault();
            if (dupe != default(Candidate)) return (Status.Conflict, dupe.Id);
                //Is this function necessary? Ask Iulia about multiple email entries in the db
            
                var entity = new Candidate
                {
                    Name = candidateDTO.Name!,
                    Email = candidateDTO.Email!,
                    CurrentDegree = candidateDTO.CurrentDegree,     
                    StudyProgram = candidateDTO.StudyProgram,
                    University = candidateDTO.University,
                    GraduationDate = DateTime.Parse(candidateDTO.GraduationDate),
                    IsUpvoted = candidateDTO.IsUpvoted,
                    Created = currentDate
                };

                _context.Candidates.Add(entity);

                await _context.SaveChangesAsync();

                return (Status.Created, entity.Id);
        }

        //Return a name given the candidate id 
        public async Task<(Status, string?)> ReadNameFromId(int id) {
            string? name = await _context.Candidates.Where(c => c.Id == id).Select(c => c.Name).FirstOrDefaultAsync();
            return (name == null ? Status.NotFound : Status.Found, name);
        }

        //Return an candidate given the candidate id
        public async Task<(Status, CandidateDTO)> Read(int id)
        {
            var c = await _context.Candidates.Include(c => c.Answers).Where(c => c.Id == id).Select(c => new CandidateDTO(){
                Name = c.Name!,
                Id = c.Id,
                Email = c.Email!,
                CurrentDegree = c.CurrentDegree!, 
                StudyProgram = c.StudyProgram!,
                University = c.University!,
                GraduationDate = c.GraduationDate.ToString("yyyy-MM"),
                IsUpvoted = c.IsUpvoted,
                Created = c.Created
            }).FirstOrDefaultAsync();

            if (c == default(CandidateDTO)) return (Status.NotFound, c);
            else return (Status.Found, c);
        }

        //Delete candidates from the system who have been there for 2 years and over
        public async Task DeleteOldCandidates(){
            var c = await _context.Candidates.Where(c => c.Created.AddYears(2) < DateTime.Today).ToListAsync();

            foreach (var candi in c){
                _context.Candidates.Remove(candi);
            }

            await _context.SaveChangesAsync();
        }

        //Return a list of all canidates 
         public async Task<IReadOnlyCollection<CandidateDTO>> ReadAll() =>
            await _context.Candidates.Select(c => new CandidateDTO(){
                Name = c.Name!,
                Id = c.Id,
                Email = c.Email!,
                CurrentDegree = c.CurrentDegree!, 
                StudyProgram = c.StudyProgram!,
                University = c.University!,
                GraduationDate = c.GraduationDate.ToString("yyyy-MM"),
                IsUpvoted = c.IsUpvoted,
                Created = c.Created
            }).ToListAsync();
        
        //Updates an candidate name, email, university and study program values
        public async Task<Status> Update(int id, CandidateDTO candidateDTO)
        {
            var c = await _context.Candidates.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (c == default(Candidate)) return Status.NotFound;

            c.Name = candidateDTO.Name;
            c.Email = candidateDTO.Email!;
            c.University = candidateDTO.University;
            c.CurrentDegree = candidateDTO.CurrentDegree;
            c.StudyProgram = candidateDTO.StudyProgram;
            c.GraduationDate = DateTime.Parse(candidateDTO.GraduationDate);
            c.IsUpvoted = candidateDTO.IsUpvoted;
            c.Created = candidateDTO.Created;

            await _context.SaveChangesAsync();

            return Status.Updated;
        }
        
        //Upvotes a candidate given the candidate id
        public async Task<Status> UpdateUpVote(int id)
        {
            var c = await _context.Candidates.Where(c => c.Id == id).FirstOrDefaultAsync();

            c.IsUpvoted = true;

            await _context.SaveChangesAsync();

            return Status.Updated;
        }

        //Deletes a candidates given the candidate id
        public async Task<Status> Delete(int id){

            var c = await _context.Candidates.Include(c => c.Answers).Where(c => c.Id == id).FirstOrDefaultAsync();
            
            if (c == default(Candidate)) return Status.NotFound;

            _context.Candidates.Remove(c);

            await _context.SaveChangesAsync();
            return Status.Deleted;
        }

        //Adds quiz answers to the candidate given the candidate id and the answers 
        public async Task<Status> AddAnswer(int candidateId, AnswersDTO answer) {
            var c = await _context.Candidates.Include(c => c.Answers).Include(c => c.EventsParticipatedIn).Where(c => c.Id == candidateId).FirstOrDefaultAsync();
            
            if (c == default(Candidate)) return Status.NotFound;

            var q = await _context.Quizes.Where(q => q.Id == answer.QuizId).FirstOrDefaultAsync();
            
            if (q == default(Quiz)) return Status.NotFound;

            var ans = new Answer() {
                Quiz = q,
                Answers = answer.Answers
            };
            if (c.Answers == null) c.Answers = new List<Answer>();
            c.Answers.Add(ans); //Add the answer to the candidate

            var e = await _context.Events.Where(e => e.Id == answer.EventId).FirstOrDefaultAsync();
            if (e != default(Event)) c.EventsParticipatedIn!.Add(e); //Add the candidate to the event

            await _context.SaveChangesAsync();
            return Status.Updated;
        }

        
    }
}