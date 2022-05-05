using TMPro;
using UnityEngine;

public class PlayerForm : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private GameObject savePresetButton;


    public void NextButton()
    {
        if (string.IsNullOrEmpty(input.text)) return;

        var playerName = input.text;
        var decider = new Player(playerName);
        GameManager.instance.AddPlayer(decider);
        if (GameManager.instance.everyoneJoined)
        {
            savePresetButton.SetActive(false);
            gameObject.SetActive(false);
        }

        ResetForm();
    }

    public void SavePresetButton()
    {
        GameManager.instance.savingPreset = true;
    }

    private void ResetForm()
    {
        if (GameManager.instance.players.Count == GameManager.instance.numberOfPlayers - 1)
        {
            buttonText.text = "Start Game";
            savePresetButton.SetActive(true);
        }

        input.text = "";
    }
}