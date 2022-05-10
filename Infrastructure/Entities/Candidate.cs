
namespace Infrastructure{

    public class Candidate{

        public int Id {get; set;}
        public string? Name {get; set;}
        public string? Email {get; set;}
        public string? CurrentDegree { get; set; }
        public string? StudyProgram {get; set;}
        public string? University {get; set;}
        public DateTime GraduationDate {get; set;}
        public ICollection<Event>? EventsParticipatedIn {get; set;}
        public Quiz? Quiz {get; set;}
        public bool IsUpvoted {get; set;}
        public virtual ICollection<Answer>? Answers {get; set;}
        public DateTime Created {get; set;}
    }
}