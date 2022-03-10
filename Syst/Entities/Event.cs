namespace Syst{

    public class Event {
        public int Id {get; set;}
        public string? Name {get; set;}
        public DateTime Date {get; set;}
        public string? Location {get; set;}
        public ICollection<Candidate>? Candidates {get; set;}
        public Quiz? Quiz {get; set;}
        public double Rating {get; set;}

        public ICollection<Admin>? Admins {get; set;}
    }
}