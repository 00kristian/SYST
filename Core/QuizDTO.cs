using System.ComponentModel.DataAnnotations;

namespace Core {

    //Quiz object that carries quiz data between processes
    public record struct QuizDTO(

        [Required]
        int Id,

        [Required]
        DateTime Date,

        [Required]
        ICollection<QuestionDTO> Questions,

        [Required]
        ICollection<EventDTO> Events,

        [Required]
        ICollection<CandidateDTO> Candidates

    );
}