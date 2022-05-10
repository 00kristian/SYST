using System.ComponentModel.DataAnnotations;

namespace Core {

    //Candidate object that carries candiate data between processes
    public record struct CreateCandidateDTO(

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
        bool IsUpvoted,

        [Required]
        DateTime Created
    );
} 