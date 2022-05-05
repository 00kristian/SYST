
using System.Collections.Generic;


namespace Core {
public interface IQuizRepository
    {
        Task<(Status, int id)> Create(QuizCreateDTO quizDTO);
        Task<(Status, QuizDTO)> Read(int id);
        Task<Status> Update(int id, QuizCreateDTO quizDTO);
        Task<Status> Delete(int id);
        Task<(Status, int id)> AddQuestion(int id, CreateQuestionDTO question);
        Task<Status> RemoveQuestion(int quizId, int questionId);
        Task<IReadOnlyCollection<QuizDTO>> ReadAll();
        Task<Status> Clone(int quizId, int origianlId);
    }
}