using System;
using System.Collections;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class QuestionsApi
{
    private readonly GameManager _gameManager;

    public QuestionsApi(GameManager gameManager)
    {
        _gameManager = gameManager;
    }


    public IEnumerator GetQuestions(int numberOfQuestions)
    {
        var uri = $"https://opentdb.com/api.php?amount={numberOfQuestions}&type=multiple";
        using var request = UnityWebRequest.Get(uri);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            var json = request.downloadHandler.text;
            json = json.Replace("\"response_code\": 0,", "");
            json = json.Replace("results", "questions");

            json = ParseSpecialCharacters(json);
            var myDeserializedClass = JsonConvert.DeserializeObject<Questions>(json);

            foreach (var q in myDeserializedClass.questions)
            {
                var question = new TriviaQuestion();
                question.Question = q.question;
                question.CorrectAnswer = q.correct_answer;
                question.Choices = q.incorrect_answers;
                question.Choices.Add(q.correct_answer);
                // shuffle the choices
                question.Choices = question.Choices.OrderBy(_ => Guid.NewGuid()).ToList();
                _gameManager.questions.Add(question);
            }
        }
        else
            MonoBehaviour.print(request.error);
    }

    private string ParseSpecialCharacters(string json)
    {
        json = json.Replace("&#039;", "'");
        json = json.Replace("&quot;", "\\\"");
        json = json.Replace("&amp;", "&");
        json = json.Replace("&rdquo;", "\\\"");
        json = json.Replace("&ouml;", "ö");
        json = json.Replace("&uuml;", "ü");
        json = json.Replace("&ntilde;", "ñ");
        json = json.Replace("&aacute;", "á");
        return json;
    }
}