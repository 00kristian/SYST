using System.ComponentModel.DataAnnotations;

namespace Core {

    //Question object that carries question data between processes and is being used when creating a question
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