using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

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
            var e = await _context.Events.Include(e => e.Quiz).Include(e => e.Winners).Include(e => e.Candidates).Where(e => e.Id == id).Select(e => new EventDTO(){
                Name = e.Name!,
                Id = e.Id,
                Date = e.Date.ToString("yyyy-MM-dd"),
                Location = e.Location!,
                Rating = e.Rating!,
                WinnersId = e.Winners.Count() == 0 ? new List<int>() : e.Winners.Select(w => w.Id).ToList(),
                Candidates = e.Candidates != null ? e.Candidates.Select(c => new CandidateDTO(){
                    Name = c.Name!,
                    Id = c.Id,
                    Email = c.Email!,
                    CurrentDegree = c.CurrentDegree!,
                    StudyProgram = c.StudyProgram!,
                    University = c.University!,
                    GraduationDate = c.GraduationDate.ToString("yyyy-MM"),
                    IsUpvoted = c.IsUpvoted, 
                    PercentageOfCorrectAnswers = c.PercentageOfCorrectAnswers
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

        //Updates an events rating 
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

            var e = await _context.Events.Include(c => c.Candidates).Where(c => c.Id == id).FirstOrDefaultAsync();
            
            if (e == default(Event)) return Status.NotFound;

            _context.Events.Remove(e);

            await _context.SaveChangesAsync();
            return Status.Deleted;
        }

        //Returns the upcoming events   
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

        //Returns the recent events  
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

        //Updates the quiz connected to the event
        public async Task<Status> UpdateQuiz(int eventid, int quizid) {

            var e = await _context.Events.Include(e => e.Quiz).Where(c => c.Id == eventid).FirstOrDefaultAsync();
            if (e == default(Event)) return Status.NotFound;

            var q = await _context.Quizes.Where(c => c.Id == quizid).FirstOrDefaultAsync();
            if (q == default(Quiz)) return Status.NotFound;

            e.Quiz = q;

            await _context.SaveChangesAsync();
            return Status.Updated;
        }

        //Returns multiple winners connected to the event 
        public async Task<(Status, IEnumerable<CandidateDTO>)> PickMultipleWinners(int eventid,
            int numOfWinners){
            var e = await _context.Events.Include(e => e.Candidates).Where(e => e.Id == eventid).FirstOrDefaultAsync();
            if (e == default(Event)) return (Status.NotFound, new List<CandidateDTO>());
            
            e.Winners ??= new List<Candidate>();
            
            if (e.Winners.Count() != 0)
            {
                
                return (Status.Found, e.Winners.Select(w => new CandidateDTO()
                {
                    Name = w.Name!,
                    Id = w.Id,
                    Email = w.Email!,
                    CurrentDegree = w.CurrentDegree!,
                    StudyProgram = w.StudyProgram!,
                    University = w.University!,
                    GraduationDate = w.GraduationDate.ToString("yyyy-MM-dd"),
                }));
            }

            var winners = e.Candidates?.OrderBy(c => Guid.NewGuid()).Take(numOfWinners).ToList();
            
            if (winners == null || winners.Count() == 0) 
            {
                return (Status.NotFound, new List<CandidateDTO>());
            }
            e.Winners = winners;
            
            await _context.SaveChangesAsync();

            var dtoWinnerList = new List<CandidateDTO>();
            
            foreach (var w in winners)
            {
                dtoWinnerList.Add(new CandidateDTO()
                {
                    Name = w.Name!,
                    Id = w.Id,
                    Email = w.Email!,
                    CurrentDegree = w.CurrentDegree!,
                    StudyProgram = w.StudyProgram!,
                    University = w.University!,
                    GraduationDate = w.GraduationDate.ToString("yyyy-MM-dd"),
                });
            }

            return (Status.Found, dtoWinnerList);
        }

    }
}