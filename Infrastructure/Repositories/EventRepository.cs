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
            var e = await _context.Events.Include(e => e.Candidates).Where(e => e.Id == id).Select(e => new EventDTO(){
                Name = e.Name!,
                Id = e.Id,
                Date = e.Date.ToShortDateString(),
                Location = e.Location!,
                Rating = e.Rating!,
                Candidates = e.Candidates != null ? e.Candidates.Select(c => new CandidateDTO(){
                    Name = c.Name!,
                    Id = c.Id,
                    Email = c.Email!,
                    StudyProgram = c.StudyProgram!,
                    University = c.University.ToString(),
                    GraduationDate = c.GraduationDate.ToShortDateString()
                }).ToList() : new List<CandidateDTO>()           
            }).FirstOrDefaultAsync();

            if (e == default(EventDTO)) return (Status.NotFound, e);
            else return (Status.Found, e);
        }

        //Return a list of all events
         public async Task<IReadOnlyCollection<EventDTO>> ReadAll() =>
            await _context.Events.Select(e => new EventDTO(){
                Name = e.Name!,
                Id = e.Id,
                Date = e.Date.ToShortDateString(),
                Location = e.Location!,
                Rating = e.Rating!
            }).ToListAsync();

        //Return a name given the event id 
        public async Task<(Status, string?)> ReadNameFromId(int id) {
            string? name = await _context.Events.Where(e => e.Id == id).Select(e => e.Name).FirstOrDefaultAsync();
            return (name == null ? Status.NotFound : Status.Found, name);
        }

        //Updates an events name, date and location
        public async Task<Status> Update(int id, EventDTO eventDTO)
        {
            var e = await _context.Events.Where(e => e.Id == id).FirstOrDefaultAsync();

            if (e == default(Event)) return Status.NotFound;

            e.Name = eventDTO.Name;
            e.Date = DateTime.Parse(eventDTO.Date!);
            e.Location = eventDTO.Location;

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
            await _context.Events.Where(e => e.Date >= DateTime.Today)
                .OrderByDescending(e => e.Date)
                .Take(5).Select(e => new EventDTO()
                {
                    Name = e.Name!,
                    Id = e.Id,
                    Date = e.Date.ToShortDateString(),
                    Location = e.Location!,
                    Rating = e.Rating!
                })
                .ToListAsync();

        public async Task<IReadOnlyCollection<EventDTO>> ReadRecent() =>
            await _context.Events.Where(e => e.Date < DateTime.Today)
                .OrderBy(e => e.Date)
                .Take(5).Select(e => new EventDTO()
                {
                    Name = e.Name!,
                    Id = e.Id,
                    Date = e.Date.ToShortDateString(),
                    Location = e.Location!,
                    Rating = e.Rating!
                })
                .ToListAsync();
    }
}