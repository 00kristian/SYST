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

            foreach (Candidate c in _context.Candidates) {
                if (c.Email == candidateDTO.Email) return (Status.Conflict, c.Id); 
                //Is this function necessary? Ask Iulia about multiple email entries in the db
            }
                var entity = new Candidate
                {
                    Name = candidateDTO.Name!,
                    Email = candidateDTO.Email!,
                    CurrentDegree = candidateDTO.CurrentDegree, 
                    StudyProgram = candidateDTO.StudyProgram,
                    University = candidateDTO.University,
                    GraduationDate = DateTime.Parse(candidateDTO.GraduationDate),
                    IsUpvoted = candidateDTO.IsUpvoted
                    
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
            var c = await _context.Candidates.Where(c => c.Id == id).Select(c => new CandidateDTO(){
                Name = c.Name!,
                Id = c.Id,
                Email = c.Email!,
                CurrentDegree = c.CurrentDegree!, 
                StudyProgram = c.StudyProgram!,
                University = c.University!,
                GraduationDate = c.GraduationDate.ToString("yyyy-MM-dd"),
                IsUpvoted = c.IsUpvoted
            }).FirstOrDefaultAsync();

            if (c == default(CandidateDTO)) return (Status.NotFound, c);
            else return (Status.Found, c);
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
                GraduationDate = c.GraduationDate.ToString("yyyy-MM-dd"),
                IsUpvoted = c.IsUpvoted
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

            await _context.SaveChangesAsync();

            return Status.Updated;
        }

        //Deletes a candidates given the candidate id
        public async Task<Status> Delete(int id){

            var c = await _context.Candidates.Where(c => c.Id == id).FirstOrDefaultAsync();
            
            if (c == default(Candidate)) return Status.NotFound;

            _context.Candidates.Remove(c);

            await _context.SaveChangesAsync();
            return Status.Deleted;
        }

        
    }
}