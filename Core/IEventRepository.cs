

namespace Core {
public interface IEventRepository
    {
        Task<(Status, int id)> Create(EventDTO eventDTO);
        Task<(Status, EventDTO)> Read(int id);
        Task<IReadOnlyCollection<EventDTO>> ReadAll();
        Task<(Status, string?)> ReadNameFromId(int id);
        Task<Status> Update(int id, EventDTO eventDTO);
        Task<Status> Delete(int id);

        Task<IReadOnlyCollection<EventDTO>> ReadUpcoming();

        Task<IReadOnlyCollection<EventDTO>> ReadRecent();

    }
}