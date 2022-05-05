using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textElement;
    [SerializeField] private GameObject correctHighlight;
    [SerializeField] private GameObject incorrectHighlight;
    [SerializeField] private Button button;

    public string choiceText;
    public int choiceIndex;

    public void SetText(string questionChoice)
    {
        choiceText = questionChoice;
        textElement.text = choiceText;
    }

    public void CorrectHighlight()
    {
        correctHighlight.SetActive(true);
    }

    public void ResetChoice()
    {
        correctHighlight.SetActive(false);
        incorrectHighlight.SetActive(false);
        button.enabled = true;
    }

    public void RemoveListener()
    {
        button.enabled = false;
    }

    public void IncorrectHighlight()
    {
        incorrectHighlight.SetActive(true);
    }
}