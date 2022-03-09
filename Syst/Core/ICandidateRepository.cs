
using System.Collections.Generic;


namespace Core {
public interface ICandidateRepository
    {
        Task<(Status, int id)> Create(CandidateDTO candidateDTO);
        Task<(Status, CandidateDTO)> Read(int id);
        Task<IReadOnlyCollection<CandidateDTO>> ReadAll();
        Task<(Status, int)> ReadIdFromName(string name);
        Task<Status> Update(int id, CandidateDTO candidateDTO);
        Task<Status> Delete(int id);

    }
}