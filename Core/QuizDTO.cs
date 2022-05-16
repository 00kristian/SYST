using System.ComponentModel.DataAnnotations;

namespace Core {

    //Quiz object that carries quiz data between processes and is being used when fetching the data
    public record struct QuizDTO(

        [Required]
        int Id,

        [Required]
        string Name,

        [Required]
        ICollection<QuestionDTO> Questions,

        [Required]
        ICollection<EventDTO> Events,

        [Required]
        ICollection<CandidateDTO> Candidates

    );
}