using System.ComponentModel.DataAnnotations;

namespace Core {

    public record struct QuizDTO(

        [Required]
        int Id,

        [Required]
        DateTime Date

    );
}