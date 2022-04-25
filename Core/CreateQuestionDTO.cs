using System.ComponentModel.DataAnnotations;

namespace Core {

    //question object that carries question data between processes
    public record struct CreateQuestionDTO(

        [Required]
        string Representation,

        [Required]
        string Answer,

        [Required]
        ICollection<string> Options,

        [Required]
        string ImageURl
    );
}