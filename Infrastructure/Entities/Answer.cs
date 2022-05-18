namespace Infrastructure{

    public class Answer {
        public int Id {get; set;}
        public string[]? Answers{get; set;}
        public Quiz? Quiz {get; set;}

        //TODO: add some way of storing how many correct answers when initialized. A double or (int, int)
    }
}