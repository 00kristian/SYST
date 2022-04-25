using System.ComponentModel.DataAnnotations;

namespace Core {

    //Question object that carries question data between processes
    public record struct QuestionDTO(

        [Required]
        int Id,

        [Required, StringLength(50)]
        string Representation,

        [Required, StringLength(50)]
        string Answer,

        [Required, StringLength(100)]
        string ImageURl,

        [Required]
        ICollection<string> Options

    );
}