using System.ComponentModel.DataAnnotations;

namespace Core {

    //Event object that carries event data between processes
    public record struct CreateEventDTO(

        [Required, StringLength(50)]
        string Name,

        [Required]
        string Date,

        [Required]
        string Location
    );
}