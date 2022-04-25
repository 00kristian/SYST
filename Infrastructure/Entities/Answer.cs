namespace Infrastructure{

    public class Answer {
        public int Id {get; set;}
        public int QuizId {get; set;}
        public string[]? Answers{get; set;}
        public virtual Quiz? Quiz {get; set;}
    }
}