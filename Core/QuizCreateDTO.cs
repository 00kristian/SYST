using System.ComponentModel.DataAnnotations;

namespace Core {

    //Quiz object that carries quiz data between processes
    public record struct QuizCreateDTO(
        [Required]
        string Name

        // [Required]
        // ICollection<CreateQuestionDTO> Questions
    );
}