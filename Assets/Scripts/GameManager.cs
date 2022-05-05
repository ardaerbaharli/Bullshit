using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public enum Decision
    {
        Bullshit,
        Correct,
        None
    }

    public enum Answer
    {
        Bullshit,
        Correct,
        None
    }

    public static GameManager instance;
    [SerializeField] private ConfigForm configForm;

    [SerializeField] public List<TriviaQuestion> questions;
    [SerializeField] private QuestionPhasePanel questionPhasePanel;
    [SerializeField] public int numberOfPlayers;
    [SerializeField] private DeciderPanel deciderPanel;
    [SerializeField] private ResultPanel resultPanel;
    [SerializeField] private PlayerForm playerForm;
    [SerializeField] private LoadPresetPanel _loadPresetPanel;
    [SerializeField] private int numberOfQuestionsToRequestOnStart = 10;

    public List<TriviaQuestion> answeredQuestions;

    public bool everyoneJoined;
    public Decision _result;
    public int numberOfQuestions, numberOfQuestionsPerPlayer;
    public List<Player> players;
    private List<Player> _deciders;
    private Player _currentPlayer;
    private int _decidingDeciderIndex = -1;
    private QuestionsApi _questionsApi;

    public bool savingPreset;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _questionsApi = new QuestionsApi(this);
        StartCoroutine(_questionsApi.GetQuestions(numberOfQuestionsToRequestOnStart));
        players = new List<Player>();
        answeredQuestions = new List<TriviaQuestion>();
        questions = new List<TriviaQuestion>();
        _deciders = new List<Player>();
        SetConfigForm(true);
    }

    public void SetConfigForm(bool value)
    {
        configForm.gameObject.SetActive(value);
    }

    public void Configure(int numPlayers, int numQuestions)
    {
        numberOfPlayers = numPlayers;
        numberOfQuestionsPerPlayer = numQuestions;
        numberOfQuestions = numQuestions * numPlayers;
        if (numberOfQuestions > numberOfQuestionsToRequestOnStart)
            StartCoroutine(_questionsApi.GetQuestions(numberOfQuestions - numberOfQuestionsToRequestOnStart));

        playerForm.gameObject.SetActive(true);
    }


    public void AddPlayer(Player p)
    {
        players.Add(p);

        if (numberOfPlayers != players.Count) return;

        if (savingPreset)
        {
            savingPreset = false;
            var preset = new Preset(numberOfPlayers, numberOfQuestionsPerPlayer, players);
            SavePreset(preset);
        }

        everyoneJoined = true;
        _currentPlayer = players[Range(0, players.Count)];
        _currentPlayer.playedAsPlayer1 = true;

        StartGame();
    }

    private void SavePreset(Preset preset)
    {
        var lastSavedPresetID = PlayerPrefs.GetInt("lastSavedPreset", 0);
        var presetName = "preset" + (lastSavedPresetID + 1);
        preset.presetID = lastSavedPresetID + 1;

        var json = JsonUtility.ToJson(preset);
        PlayerPrefs.SetString(presetName, json);
        PlayerPrefs.SetInt("lastSavedPreset", lastSavedPresetID + 1);
    }

    public void LoadPreset(int presetID)
    {
        var presetName = "preset" + presetID;
        var json = PlayerPrefs.GetString(presetName);
        var preset = JsonUtility.FromJson<Preset>(json);

        numberOfPlayers = preset.NumberOfPlayers;
        numberOfQuestionsPerPlayer = preset.NumberOfQuestionsPerPlayer;
        players = preset.Players;
    }

    public void LoadPreset(Preset preset)
    {
        _loadPresetPanel.gameObject.SetActive(false);
        numberOfPlayers = preset.NumberOfPlayers;
        numberOfQuestionsPerPlayer = preset.NumberOfQuestionsPerPlayer;
        players = preset.Players;
        numberOfQuestions = numberOfQuestionsPerPlayer * numberOfPlayers;

        everyoneJoined = true;
        _currentPlayer = players[Range(0, players.Count)];
        _currentPlayer.playedAsPlayer1 = true;

        StartGame();
    }

    public List<Preset> GetAllPresets()
    {
        var presets = new List<Preset>();
        var lastSavedPresetID = PlayerPrefs.GetInt("lastSavedPreset");
        for (var i = 1; i <= lastSavedPresetID; i++)
        {
            var presetName = "preset" + i;
            var json = PlayerPrefs.GetString(presetName);
            var preset = JsonUtility.FromJson<Preset>(json);
            presets.Add(preset);
        }

        return presets;
    }

    private void StartGame()
    {
        questionPhasePanel.SetPlayer(_currentPlayer);
        _deciders = players.Where(p => p != _currentPlayer).ToList();
        StartCoroutine(NextQuestionC());
    }

    public void NextQuestion()
    {
        StartCoroutine(NextQuestionC());
    }

    public IEnumerator NextQuestionC()
    {
        if (questions.Count <= 0)
            yield return StartCoroutine(_questionsApi.GetQuestions(numberOfQuestions));

        resultPanel.gameObject.SetActive(false);
        questionPhasePanel.ResetPanel();
        questionPhasePanel.SetQuestion(questions.First());
        ToggleQuestionPanel(true);
    }

    public void QuestionAnswered()
    {
        answeredQuestions.Add(questions.First());
        questions.RemoveAt(0);
        if (answeredQuestions.Count == numberOfQuestionsPerPlayer)
            _currentPlayer.answeredAllQuestions = true;
        ToggleQuestionPanel(false);
        deciderPanel.gameObject.SetActive(true);
        NextDecider();
    }

    public void NextDecider()
    {
        _decidingDeciderIndex++;
        if (_deciders.Count > _decidingDeciderIndex)
        {
            deciderPanel.SetDecider(_deciders[_decidingDeciderIndex]);
        }
        else
        {
            foreach (var decider in _deciders)
            {
                if (decider.latestDecision == _result)
                {
                    decider.CorrectGuess();
                }
                else
                {
                    decider.IncorrectGuess();
                }
            }

            // _deciders.ForEach(decider =>
            //     ((UnityAction) (decider.latestDecision == _result ? decider.CorrectGuess : decider.IncorrectGuess))
            //     .Invoke());

            deciderPanel.gameObject.SetActive(false);
            _decidingDeciderIndex = -1;
            ShowResults();
        }
    }

    private void ShowResults()
    {
        resultPanel.gameObject.SetActive(true);
        resultPanel.SetResult(_deciders, _currentPlayer);
    }

    public void ToggleQuestionPanel(bool value)
    {
        questionPhasePanel.gameObject.SetActive(value);
    }

    public void ChangePlayer()
    {
        resultPanel.gameObject.SetActive(false);

        players.ForEach(x => x.ResetData());
        _currentPlayer = players.OrderByDescending(player => player.deciderRatio).Where(x => !x.playedAsPlayer1)
            .ToList().First();
        _currentPlayer.playedAsPlayer1 = true;

        _deciders = new List<Player>();
        answeredQuestions.Clear();
        StartGame();
    }

    public bool IsPlayerTurnFinished()
    {
        return _currentPlayer.answeredAllQuestions;
    }

    public bool IsGameFinished()
    {
        return players.All(player => player.answeredAllQuestions);
    }

    public void RestartGame()
    {
        resultPanel.gameObject.SetActive(false);
        players.ForEach(x => x.ResetData());
        players.ForEach(x => x.ResetPlayer1Data());
        _currentPlayer = players.OrderBy(_ => Guid.NewGuid()).First();
        _currentPlayer.playedAsPlayer1 = true;

        _deciders = new List<Player>();
        answeredQuestions.Clear();

        StartGame();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main");
    }

    public void ShowLoadPresetPanel()
    {
        _loadPresetPanel.gameObject.SetActive(true);
        _loadPresetPanel.SetPresets(GetAllPresets());
    }

   
}