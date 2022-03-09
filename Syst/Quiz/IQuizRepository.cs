
using System.Collections.Generic;


namespace Core {
public interface IQuizRepository
    {
        Task<(Status, int id)> Create(QuizDTO quizDTO);
        Task<(Status, QuizDTO)> Read(int id);
        Task<Status> Update(int id, QuizDTO quizDTO);
        Task<Status> Delete(int id);
         Task<IReadOnlyCollection<QuizDTO>> ReadAll();

    }
}