using System.ComponentModel.DataAnnotations;

namespace Core {

    public record struct AdminDTO(

        [Required]
        int Id,

        [Required, StringLength(50)]
        string Name,

        [Required, EmailAddress]
        string Email,

        [Required]
        ICollection<EventDTO> Events

    );
}