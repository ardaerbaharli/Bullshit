using System;
using static GameManager;

[Serializable]
public class Player
{
    public string name;
    public float deciderRatio;
    public Decision latestDecision;
    private int _wins;
    private int _loses;
    public Answer answer;
    public bool playedAsPlayer1;
    public bool answeredAllQuestions;

    public Player(string name)
    {
        this.name = name;
        ResetData();
    }

    public void ResetData()
    {
        deciderRatio = 0;
        _wins = 0;
        _loses = 0;
        latestDecision = Decision.None;
        answer = Answer.None;
    }

    public void ResetPlayer1Data()
    {
        playedAsPlayer1 = false;
        answeredAllQuestions = false;
    }

    public void CorrectGuess()
    {
        _wins++;
        deciderRatio = _wins * 100f / (_wins + _loses);
    }

    public void IncorrectGuess()
    {
        _loses++;
        deciderRatio = _wins * 100f / (_wins + _loses);
    }
}