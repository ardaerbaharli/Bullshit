using System.Collections.Generic;

[System.Serializable]
public class TriviaQuestion
{
    public string Question { get; set; }
    public string CorrectAnswer { get; set; }
    public List<string> Choices { get; set; }
}


public class Question
{
    public string category { get; set; }
    public string type { get; set; }
    public string difficulty { get; set; }
    public string question { get; set; }
    public string correct_answer { get; set; }
    public List<string> incorrect_answers { get; set; }
}

public class Questions
{
    public List<Question> questions { get; set; }
}