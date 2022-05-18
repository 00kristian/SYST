using System.ComponentModel.DataAnnotations;

namespace Core {

    //Event object that carries event data between processes and is being used when creating an event
    public record struct CreateEventDTO(

        [Required, StringLength(50)]
        string Name,

        [Required]
        string Date,

        [Required]
        string Location
    );
}