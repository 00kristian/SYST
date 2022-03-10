using System.ComponentModel.DataAnnotations;

namespace Core {

    public record struct CandidateDTO(

        [Required]
        int Id,

        [Required, StringLength(50)]
        string Name,

        [Required, EmailAddress]
        string Email,

        [Required]
        string StudyProgram,

        [Required]
        UniversityEnum University,

        [Required]
        ICollection<EventDTO> Events,

        [Required]
        QuizDTO Quiz


    );
}