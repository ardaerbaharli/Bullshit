using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QuestionPhasePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private ChoiceUI choiceUIA;
    [SerializeField] private ChoiceUI choiceUIB;
    [SerializeField] private ChoiceUI choiceUIC;
    [SerializeField] private ChoiceUI choiceUID;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private GameObject nextButton;

    public Player currentPlayer;
    public TriviaQuestion triviaQuestion;
    private ChoiceUI _correctAnswer, _playerAnswer;


    [SerializeField] private List<ChoiceUI> choices;


    public void SetPlayer(Player p)
    {
        currentPlayer = p;
        p.latestDecision = GameManager.Decision.None;
        playerName.text = p.name;
        choices = new List<ChoiceUI>() {choiceUIA, choiceUIB, choiceUIC, choiceUID};
    }

    public void SetQuestion(TriviaQuestion question)
    {
        nextButton.SetActive(false);
        triviaQuestion = question;
        questionText.text = question.Question;
        choices.ForEach(c => c.SetText(question.Choices[c.choiceIndex]));
        _correctAnswer = choices.First(c => c.choiceText == question.CorrectAnswer);
    }

    public void ResetPanel() => choices.ForEach(c => c.ResetChoice());

    private void RevealCorrectAnswer()
    {
        currentPlayer.answer =
            _playerAnswer == _correctAnswer ? GameManager.Answer.Correct : GameManager.Answer.Bullshit;
        GameManager.instance._result = (GameManager.Decision) currentPlayer.answer;
        RemoveChoiceListeners();
        _correctAnswer.CorrectHighlight();
        nextButton.SetActive(true);
    }

    private void RemoveChoiceListeners() => choices.ForEach(c => c.RemoveListener());


    private void OnChoiceClicked(ChoiceUI choice)
    {
        if (_correctAnswer != choice)
            choice.IncorrectHighlight();

        _playerAnswer = choice;
        RevealCorrectAnswer();
    }

    public void Selected_A() => OnChoiceClicked(choiceUIA);

    public void Selected_B() => OnChoiceClicked(choiceUIB);

    public void Selected_C() => OnChoiceClicked(choiceUIC);

    public void Selected_D() => OnChoiceClicked(choiceUID);

    public void NextButton() => GameManager.instance.QuestionAnswered();
}