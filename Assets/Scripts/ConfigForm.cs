using TMPro;
using UnityEngine;

public class ConfigForm : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerInput;
    [SerializeField] private TMP_InputField questionInput;

    public void NextButton()
    {
        if (string.IsNullOrEmpty(questionInput.text)) return;
        if (string.IsNullOrEmpty(playerInput.text)) return;
        GameManager.instance.Configure(int.Parse(playerInput.text), int.Parse(questionInput.text));
        gameObject.SetActive(false);
    }

    public void LoadPresetButton()
    {
        gameObject.SetActive(false);
        GameManager.instance.ShowLoadPresetPanel();
    }
}