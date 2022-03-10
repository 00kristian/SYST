using System.ComponentModel.DataAnnotations;
using Syst;

namespace Core {

    public record struct EventDTO(

        [Required]
        int Id,

        [Required, StringLength(50)]
        string Name,

        [Required]
        DateTime Date,

        [Required]
        string Location,

        [Required]
        ICollection<Candidate> Candidates,

        [Required]
        Quiz Quiz,

        [Required]
        double Rating,

        [Required]
        ICollection<Admin> Admins


    );
}