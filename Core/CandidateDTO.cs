using System.ComponentModel.DataAnnotations;

namespace Core {

    //Candidate object that carries candiate data between processes
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
        ICollection<EventDTO> Events,

        [Required]
        QuizDTO Quiz,

        [Required]
        bool IsUpvoted


    );
}