using System.ComponentModel.DataAnnotations;

namespace Core {

    //Event object that carries event data between processes and is being used when fetching the data
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

        QuizDTO Quiz,

        [Required]
        double Rating,

       
        ICollection<int> WinnersId
    );
}