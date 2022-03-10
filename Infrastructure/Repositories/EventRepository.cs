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

        public async Task<(Status, int id)> Create(EventDTO eventDTO) {

            foreach (Event e in _context.Events) {
                if (e.Name == e.Name) return (Status.Conflict, e.Id);
            }
                var entity = new Event
                {
                    Name = eventDTO.Name!,
                    Date = eventDTO.Date!,
                    Location = eventDTO.Location, 
                };

                _context.Events.Add(entity);

                await _context.SaveChangesAsync();

                return (Status.Created, entity.Id);
        }
        public async Task<(Status, EventDTO)> Read(int id)
        {
            var e = await _context.Events.Where(e => e.Id == id).Select(e => new EventDTO(){
                Name = e.Name!,
                Id = e.Id,
                Date = e.Date,
                Location = e.Location!
            }).FirstOrDefaultAsync();

            if (e == default(EventDTO)) return (Status.NotFound, e);
            else return (Status.Found, e);
        }

         public async Task<IReadOnlyCollection<EventDTO>> ReadAll() =>
            await _context.Events.Select(e => new EventDTO(){
                Name = e.Name!,
                Id = e.Id,
                Date = e.Date,
                Location = e.Location!
            }).ToListAsync();

        public async Task<(Status, int)> ReadIdFromName(string name) {
            int id = await _context.Events.Where(e => e.Name == name).Select(e => e.Id).FirstOrDefaultAsync();
            return (id == 0 ? Status.NotFound : Status.Found, id);
        }

        public async Task<Status> Update(int id, EventDTO eventDTO)
        {
            var e = await _context.Events.Where(e => e.Id == id).FirstOrDefaultAsync();

            if (e == default(Event)) return Status.NotFound;

            e.Name = eventDTO.Name;
            e.Date = eventDTO.Date!;
            e.Location = eventDTO.Location;

            await _context.SaveChangesAsync();

            return Status.Updated;
        }

        public async Task<Status> Delete(int id){

            var e = await _context.Events.Where(c => c.Id == id).FirstOrDefaultAsync();
            
            if (e == default(Event)) return Status.NotFound;

            _context.Events.Remove(e);

            await _context.SaveChangesAsync();
            return Status.Deleted;
        }
    }
}