namespace Syst{

    public class Question{
        public int Id {get; set;}
        public string? Representation {get; set;}
        public string? Answer {get; set;}
        public string? ImageURL {get; set;} //what to do? 
        public ICollection<string>? Options {get; set;}
    }
}