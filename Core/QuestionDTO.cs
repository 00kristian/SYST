using System.ComponentModel.DataAnnotations;

namespace Core {

    //Question object that carries question data between processes and is being used when fetching the data
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