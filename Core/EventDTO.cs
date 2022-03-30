using System.ComponentModel.DataAnnotations;

namespace Core {

    //Event object that carries event data between processes
    public record struct EventDTO(

        [Required]
        int Id,

        [Required, StringLength(50)]
        string Name,

        [Required]
        string Date,

        [Required]
        string Location,
        
        ICollection<CandidateDTO> Candidates,

        [Required]
        QuizDTO Quiz,

        [Required]
        double Rating,

        [Required]
        ICollection<AdminDTO> Admins


    );
}