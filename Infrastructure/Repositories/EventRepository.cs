using Core;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{

    public class EventRepository : IEventRepository
    {
        ISystematicContext _context;
        public EventRepository(ISystematicContext context)
        {
            _context = context;
        }

        //Creates an event
        public async Task<(Status, int id)> Create(CreateEventDTO eventDTO) {

            
                var entity = new Event
                {
                    Name = eventDTO.Name!,
                    Date = DateTime.Parse(eventDTO.Date!),
                    Location = eventDTO.Location,
                };

                _context.Events.Add(entity);

                await _context.SaveChangesAsync();

                return (Status.Created, entity.Id);
        }

        //Return an event given the event id
        public async Task<(Status, EventDTO)> Read(int id)
        {
            var e = await _context.Events.Include(e => e.Quiz).Include(e => e.Winner).Include(e => e.Candidates).Where(e => e.Id == id).Select(e => new EventDTO(){
                Name = e.Name!,
                Id = e.Id,
                Date = e.Date.ToString("yyyy-MM-dd"),
                Location = e.Location!,
                Rating = e.Rating!,
                WinnerId = e.Winner ==  default(Candidate) ? -1 : e.Winner.Id,
                Candidates = e.Candidates != null ? e.Candidates.Select(c => new CandidateDTO(){
                    Name = c.Name!,
                    Id = c.Id,
                    Email = c.Email!,
                    CurrentDegree = c.CurrentDegree!,
                    StudyProgram = c.StudyProgram!,
                    University = c.University!,
                    GraduationDate = c.GraduationDate.ToString("yyyy-MM"),
                    IsUpvoted = c.IsUpvoted
                }).ToList() : new List<CandidateDTO>(),
                Quiz = (e.Quiz != default(Quiz)) ? new QuizDTO() {
                    Id = e.Quiz.Id,
                    Name = e.Quiz.Name!
                } : new QuizDTO() {
                    Id = -1
                }
            }).FirstOrDefaultAsync();

            if (e == default(EventDTO)) return (Status.NotFound, e);
            else return (Status.Found, e);
        }

        //Return a list of all events
         public async Task<IReadOnlyCollection<EventDTO>> ReadAll() =>
            await _context.Events.Include(e => e.Quiz).Select(e => new EventDTO(){
                Name = e.Name!,
                Id = e.Id,
                Date = e.Date.ToString("yyyy-MM-dd"),
                Location = e.Location!,
                Rating = e.Rating!,
                Quiz = (e.Quiz != default(Quiz)) ? new QuizDTO() {
                    Id = e.Quiz.Id,
                    Name = e.Quiz.Name!
                } : new QuizDTO() {
                    Id = -1
                }
            }).ToListAsync();

        //Return a name given the event id 
        public async Task<(Status, string?)> ReadNameFromId(int id) {
            string? name = await _context.Events.Where(e => e.Id == id).Select(e => e.Name).FirstOrDefaultAsync();
            return (name == null ? Status.NotFound : Status.Found, name);
        }

        //Updates an events name, date and location
        public async Task<Status> Update(int id, CreateEventDTO eventDTO)
        {
            var e = await _context.Events.Where(e => e.Id == id).FirstOrDefaultAsync();

            if (e == default(Event)) return Status.NotFound;

            e.Name = eventDTO.Name;
            e.Date = DateTime.Parse(eventDTO.Date!);
            e.Location = eventDTO.Location;

            await _context.SaveChangesAsync();

            return Status.Updated;
        }

        public async Task<Status> UpdateRating(int id, double rating)
        {
            var e = await _context.Events.Where(e => e.Id == id).FirstOrDefaultAsync();

            if (e == default(Event)) return Status.NotFound;

            e.Rating = rating;

            await _context.SaveChangesAsync();

            return Status.Updated;
        }

        //Deletes an event given the event id
        public async Task<Status> Delete(int id){

            var e = await _context.Events.Where(c => c.Id == id).FirstOrDefaultAsync();
            
            if (e == default(Event)) return Status.NotFound;

            _context.Events.Remove(e);

            await _context.SaveChangesAsync();
            return Status.Deleted;
        }
        
        public async Task<IReadOnlyCollection<EventDTO>> ReadUpcoming() =>
            await _context.Events.Include(e => e.Quiz).Where(e => e.Date >= DateTime.Today)
                .OrderBy(e => e.Date)
                .Take(5).Select(e => new EventDTO()
                {
                    Name = e.Name!,
                    Id = e.Id,
                    Date = e.Date.ToString("yyyy-MM-dd"),
                    Location = e.Location!,
                    Rating = e.Rating!,
                    Quiz = (e.Quiz != default(Quiz)) ? new QuizDTO() {
                        Id = e.Quiz.Id,
                        Name = e.Quiz.Name!
                    } : new QuizDTO() {
                        Id = -1
                    }
                })
                .ToListAsync();

        public async Task<IReadOnlyCollection<EventDTO>> ReadRecent() =>
            await _context.Events.Include(e => e.Quiz).Where(e => e.Date < DateTime.Today)
                .OrderByDescending(e => e.Date)
                .Take(5).Select(e => new EventDTO()
                {
                    Name = e.Name!,
                    Id = e.Id,
                    Date = e.Date.ToString("yyyy-MM-dd"),
                    Location = e.Location!,
                    Rating = e.Rating!,
                    Quiz = (e.Quiz != default(Quiz)) ? new QuizDTO() {
                        Id = e.Quiz.Id,
                        Name = e.Quiz.Name!
                    } : new QuizDTO() {
                        Id = -1
                    }
                })
                .ToListAsync();

        public async Task<Status> UpdateQuiz(int eventid, int quizid) {

            var e = await _context.Events.Include(e => e.Quiz).Where(c => c.Id == eventid).FirstOrDefaultAsync();
            if (e == default(Event)) return Status.NotFound;

            var q = await _context.Quizes.Where(c => c.Id == quizid).FirstOrDefaultAsync();
            if (q == default(Quiz)) return Status.NotFound;

            e.Quiz = q;

            await _context.SaveChangesAsync();
            return Status.Updated;
        }

        public async Task<(Status, CandidateDTO)> pickAWinner(int eventid) {
            var e = await _context.Events.Include(e => e.Candidates).Where(e => e.Id == eventid).FirstOrDefaultAsync();
            if (e == default(Event)) return (Status.NotFound, default(CandidateDTO));
            
            //if a candidate already exists it should not update but it should return status.found
            if(e.Winner != default(Candidate)) {
                return (Status.Found, default(CandidateDTO));
            }

            var c = e.Candidates?.OrderBy(c => Guid.NewGuid()).FirstOrDefault();
            if (c == default(Candidate)) {
                return (Status.NotFound, default(CandidateDTO));
            } else {e.Winner = c;}
            
            await _context.SaveChangesAsync();

            return (Status.Found, new CandidateDTO() {
                Name = c.Name!,
                Id = c.Id,
                Email = c.Email!,
                CurrentDegree = c.CurrentDegree!,
                StudyProgram = c.StudyProgram!,
                University = c.University!,
                GraduationDate = c.GraduationDate.ToString("yyyy-MM"),
            });
        }

        
    }
}