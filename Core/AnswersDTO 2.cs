using System.ComponentModel.DataAnnotations;
namespace Core {
    //Answer object that carries admin data between processes 
    public record struct AnswersDTO(

        int Id,

        [Required]
        int QuizId,

        int EventId,

        string[]? Answers
    );
}        