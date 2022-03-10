using System.ComponentModel.DataAnnotations;
using Syst;

namespace Core {

    public record struct AdminDTO(

        [Required]
        int Id,

        [Required, StringLength(50)]
        string Name,

        [Required, EmailAddress]
        string Email,

        [Required]
        ICollection<Event> Events

    );
}