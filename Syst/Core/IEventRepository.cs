

namespace Core {
public interface IEventRepository
    {
        Task<(Status, int id)> Create(EventDTO eventDTO);
        Task<(Status, EventDTO)> Read(int id);
        Task<IReadOnlyCollection<EventDTO>> ReadAll();
        Task<(Status, int)> ReadIdFromName(string name);
        Task<Status> Update(int id, EventDTO eventDTO);
        Task<Status> Delete(int id);

    }
}