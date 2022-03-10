namespace Infrastructure{

    public class Question{
        public int Id {get; set;}
        public string? Representation {get; set;}
        public string? Answer {get; set;}
        public string? ImageURL {get; set;}  
        public ICollection<string>? Options {get; set;}
        public Quiz? Quiz {get; set;}
    }
}