using System.ComponentModel.DataAnnotations;

namespace Core {
    public record struct UniversityAnswerDistributionDTO(
        [Required]
        string Name,
        [Required]
        double[] answerRates,
        [Required]
        int[] distribution
    );
}