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

        public async Task<(Status, int id)> Create(CandidateDTO candidateDTO) {

            foreach (Candidate c in _context.Candidates) {
                if (c.Name == c.Name) return (Status.Conflict, c.Id);
            }
                var entity = new Candidate
                {
                    Name = candidateDTO.Name!,
                    Email = candidateDTO.Email!,
                    StudyProgram = candidateDTO.StudyProgram,
                    University = candidateDTO.University,
                    
                };

                _context.Candidates.Add(entity);

                await _context.SaveChangesAsync();

                return (Status.Created, entity.Id);
        }

        public async Task<(Status, int)> ReadIdFromName(string name) {
            int id = await _context.Candidates.Where(c => c.Name == name).Select(c => c.Id).FirstOrDefaultAsync();
            return (id == 0 ? Status.NotFound : Status.Found, id);
        }

        public async Task<(Status, CandidateDTO)> Read(int id)
        {
            var c = await _context.Candidates.Where(c => c.Id == id).Select(c => new CandidateDTO(){
                Name = c.Name!,
                Id = c.Id,
                Email = c.Email!,
                StudyProgram = c.StudyProgram!,
                University = c.University
            }).FirstOrDefaultAsync();

            if (c == default(CandidateDTO)) return (Status.NotFound, c);
            else return (Status.Found, c);
        }

         public async Task<IReadOnlyCollection<CandidateDTO>> ReadAll() =>
            await _context.Candidates.Select(c => new CandidateDTO(){
                Name = c.Name!,
                Id = c.Id,
                Email = c.Email!,
                StudyProgram = c.StudyProgram!,
                University = c.University
            }).ToListAsync();

        public async Task<Status> Update(int id, CandidateDTO candidateDTO)
        {
            var c = await _context.Candidates.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (c == default(Candidate)) return Status.NotFound;

            c.Name = candidateDTO.Name;
            c.Email = candidateDTO.Email!;
            c.University = candidateDTO.University;
            c.StudyProgram = candidateDTO.StudyProgram;

            await _context.SaveChangesAsync();

            return Status.Updated;
        }

        public async Task<Status> Delete(int id){

            var c = await _context.Candidates.Where(c => c.Id == id).FirstOrDefaultAsync();
            
            if (c == default(Candidate)) return Status.NotFound;

            _context.Candidates.Remove(c);

            await _context.SaveChangesAsync();
            return Status.Deleted;
        }
    }
}