using TMPro;
using UnityEngine;
using static GameManager;


public class DeciderPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject correctGlow;
    [SerializeField] private GameObject bsGlow;
    [SerializeField] private TextMeshProUGUI questionButtonText;
    [SerializeField] private GameObject rest;

    public Player decider;
    private Decision _decision = Decision.None;

    private bool _isShowingQuestion;

    public void SetDecider(Player d)
    {
        decider = d;
        nameText.text = decider.name;
        correctGlow.SetActive(false);
        bsGlow.SetActive(false);
        _decision = Decision.None;
    }

    public void Bullshit()
    {
        _decision = Decision.Bullshit;
        bsGlow.SetActive(true);
        correctGlow.SetActive(false);
    }

    public void Correct()
    {
        _decision = Decision.Correct;
        correctGlow.SetActive(true);
        bsGlow.SetActive(false);
    }

    public void ShowQuestion()
    {
        if (!_isShowingQuestion)
        {
            _isShowingQuestion = true;
            instance.ToggleQuestionPanel(_isShowingQuestion);
            questionButtonText.text = "Hide Question";
            rest.SetActive(false);
        }
        else
        {
            _isShowingQuestion = false;
            instance.ToggleQuestionPanel(_isShowingQuestion);
            questionButtonText.text = "Show Question";
            rest.SetActive(true);
        }
    }

    public void Lock()
    {
        if (_decision == Decision.None) return;
        decider.latestDecision = _decision;
        instance.NextDecider();
    }
}