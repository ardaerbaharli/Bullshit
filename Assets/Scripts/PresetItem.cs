using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PresetItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberOfPlayersText;
    [SerializeField] private TextMeshProUGUI numberOfQuestionsPerPlayerText;
    [SerializeField] private TextMeshProUGUI listOfPlayersText;
    [SerializeField] private Image background;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color regularColor;

    public LoadPresetPanel loadPresetPanel;
    private Preset _preset;


    public void SetData(Preset p)
    {
        _preset = p;
        numberOfPlayersText.text = $"Number of players: {p.NumberOfPlayers}";
        numberOfQuestionsPerPlayerText.text = $"Number of questions per player: {p.NumberOfQuestionsPerPlayer}";
        var players = string.Join(", ", _preset.Players.Select(x => x.name).ToArray());
        listOfPlayersText.text = $"List of players: {players}";
    }

    public void OnClick()
    {
        loadPresetPanel.SelectedPreset = _preset;
        background.color = selectedColor;

        var c = transform.parent.childCount;
        var index = transform.GetSiblingIndex();
        for (var i = 0; i < c; i++)
        {
            if (i == index) continue;
            transform.parent.GetChild(i).GetComponent<PresetItem>().ResetColor();
        }
    }

    public void ResetColor()
    {
        background.color = regularColor;
    }
}