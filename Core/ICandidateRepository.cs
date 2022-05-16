
namespace Core {
public interface ICandidateRepository
    {
        Task<(Status, int id)> Create(CreateCandidateDTO candidateDTO);
        Task<(Status, CandidateDTO)> Read(int id);
        Task<IReadOnlyCollection<CandidateDTO>> ReadAll();
        Task<(Status, string?)> ReadNameFromId(int id);
        Task<Status> Update(int id, CandidateDTO candidateDTO);

        Task<Status> UpdateUpVote (int id);
        Task<Status> Delete(int id);
        Task<Status> AddAnswer(int candidateId, AnswerDTO answer);
        Task DeleteOldCandidates();
        Task<int[]> GraphData(string[] universities);
    }
}