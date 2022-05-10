

namespace Core {
public interface IEventRepository
    {
        Task<(Status, int id)> Create(CreateEventDTO eventDTO);
        Task<(Status, EventDTO)> Read(int id);
        Task<IReadOnlyCollection<EventDTO>> ReadAll();
        Task<(Status, string?)> ReadNameFromId(int id);
        Task<Status> Update(int id, CreateEventDTO eventDTO);
        Task<Status> Delete(int id);

        Task<IReadOnlyCollection<EventDTO>> ReadUpcoming();

        Task<IReadOnlyCollection<EventDTO>> ReadRecent();

        Task<Status> UpdateQuiz(int eventid, int quizid);

        Task<(Status, CandidateDTO)> pickAWinner(int eventid);

        Task<Status> UpdateRating(int id, double rating);

    }
}