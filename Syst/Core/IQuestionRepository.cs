
using System.Collections.Generic;


namespace Core {
public interface IQuestionRepository
    {
        Task<(Status, int id)> Create(QuestionDTO questionDTO);
        Task<(Status, QuestionDTO)> Read(int id);
        Task<Status> Update(int id, QuestionDTO questionDTO);
        Task<Status> Delete(int id);

    }
}