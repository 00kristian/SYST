using System.ComponentModel.DataAnnotations;

namespace Core {

    public record struct EventDTO(

        [Required]
        int Id,

        [Required, StringLength(50)]
        string Name,

        [Required]
        DateTime Date,

        [Required]
        string Location,

        [Required]
        ICollection<CandidateDTO> Candidates,

        [Required]
        QuizDTO Quiz,

        [Required]
        double Rating,

        [Required]
        ICollection<AdminDTO> Admins


    );
}