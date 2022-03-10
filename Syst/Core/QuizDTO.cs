using System.ComponentModel.DataAnnotations;
using Syst;

namespace Core {

    public record struct QuizDTO(

        [Required]
        int Id,

        [Required]
        DateTime Date,

        [Required]
        ICollection<Question> Questions,

        [Required]
        ICollection<Event> Events,

        [Required]
        ICollection<Candidate> Candidates

    );
}