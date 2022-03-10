namespace Infrastructure{

    public class Quiz {
        public int Id {get; set;}
        public DateTime Date {get; set;}
        public ICollection<Question>? Questions {get; set;}
        public ICollection<Event>? Events {get; set;}
        public ICollection<Candidate>? Candidates {get; set;}
    }
}