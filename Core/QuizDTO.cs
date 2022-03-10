using System.ComponentModel.DataAnnotations;

namespace Core {

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