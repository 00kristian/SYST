using System.ComponentModel.DataAnnotations;

namespace Core {

    //Candidate object that carries candiate data between processes and is being used when fetching the data
    public record struct CandidateDTO(

        [Required]
        int Id,

        [Required, StringLength(50)]
        string Name,

        [Required, EmailAddress]
        string Email,
    
        [Required]
        string CurrentDegree, 
        
        [Required]
        string StudyProgram,

        [Required]
        string University,

        [Required]
        string GraduationDate,
        
        [Required] 
        double PercentageOfCorrectAnswers,

        [Required]
        ICollection<EventDTO> Events,

        [Required]
        QuizDTO Quiz,

        [Required]
        bool IsUpvoted,

        [Required]
        DateTime Created

    );
}