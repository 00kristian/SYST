using System.ComponentModel.DataAnnotations;

namespace Core {

    public record struct QuestionDTO(

        [Required]
        int Id,

        [Required, StringLength(50)]
        string Representation,

        [Required, StringLength(50)]
        string Answer,

        [Required, StringLength(50)]
        string ImageURl,

        [Required]
        ICollection<string> Options,

        [Required]
        QuizDTO Quiz

    );
}