using System.ComponentModel.DataAnnotations;
namespace Core {

    public record struct AnswerDTO(

        int Id,

        [Required]
        int QuizId,

        int EventId,

        string[]? Answers
    );
}        