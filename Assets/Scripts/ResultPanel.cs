using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] private GameObject resultItemPrefab;
    [SerializeField] private Transform contentPanel;
    [SerializeField] private TextMeshProUGUI result;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Button dynamicButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button playAgainButton;

    private float _itemHeight;

    private void Awake()
    {
        _itemHeight = resultItemPrefab.GetComponent<RectTransform>().rect.height;
    }

    public void SetResult(List<Player> deciders, Player currentPlayer)
    {
        ClearResultsPanel();

        var resultT = currentPlayer.answer.ToString();
        result.text = $"Answer of {currentPlayer.name} was {resultT}!";

        FillResults(deciders);

        if (GameManager.instance.IsPlayerTurnFinished())
        {
            // GameManager.instance.FinishPlayerTurn();
            if (GameManager.instance.IsGameFinished())
            {
                playAgainButton.gameObject.SetActive(true);
                mainMenuButton.gameObject.SetActive(true);
                dynamicButton.gameObject.SetActive(false);
                return;
            }

            buttonText.text = "Next Player";
            dynamicButton.onClick.RemoveAllListeners();
            dynamicButton.onClick.AddListener(GameManager.instance.ChangePlayer);
        }
        else
        {
            buttonText.text = "Next Question";
            dynamicButton.onClick.RemoveAllListeners();
            dynamicButton.onClick.AddListener(GameManager.instance.NextQuestion);
        }
    }

    private void FillResults(List<Player> deciders)
    {
        contentPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, _itemHeight * deciders.Count);
        foreach (var decider in deciders)
        {
            var item = Instantiate(resultItemPrefab, contentPanel);
            item.GetComponent<ResultItem>().SetData(decider.name, decider.deciderRatio);
        }
    }

    private void ClearResultsPanel()
    {
        var childCount = contentPanel.childCount;
        for (var i = 0; i < childCount; i++)
        {
            Destroy(contentPanel.GetChild(i).gameObject);
        }
        playAgainButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
        dynamicButton.gameObject.SetActive(true);
    }

    public void DynamicButton()
    {
        StartCoroutine(GameManager.instance.NextQuestionC());
    }

    public void MainMenuButton()
    {
        GameManager.instance.LoadMainMenu();
    }

    public void PlayAgainButton()
    {
        GameManager.instance.RestartGame();
    }
}