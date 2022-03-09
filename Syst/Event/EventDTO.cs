using System.ComponentModel.DataAnnotations;

namespace Core {

    public record struct EventDTO(

        [Required]
        int Id,

        [Required, StringLength(50)]
        string Name,

        [Required]
        DateTime Date,

        [Required]
        string Location
    );
}