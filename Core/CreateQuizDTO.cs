using System.ComponentModel.DataAnnotations;

namespace Core {

    //Quiz object that carries quiz data between processes and is being used when creating a Quiz
    public record struct CreateQuizDTO(
        [Required]
        string Name
    );
}