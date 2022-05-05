using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MovingColor : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private List<Color> colors;
    [SerializeField] private float speed;

    private Vector3 _nextPos;
    private float _sh, _sw;

    private void Awake()
    {
        _sh = Screen.height;
        _sw = Screen.width;
        image.color = GetRandomColor();
        _nextPos = GetRandomPos();
    }

    private Vector3 GetRandomPos()
    {
        // return random position between screen borders
        return new Vector3(Random.Range(0, _sw), Random.Range(0, _sh), 0);
    }

    private Color GetRandomColor()
    {
        return colors[Random.Range(0, colors.Count)];
    }

    private void Update()
    {
        // move to next position
        transform.position = Vector3.MoveTowards(transform.position, _nextPos, Time.deltaTime * speed);
        // if reached next position, set new position and color
        if (Vector3.Distance(transform.position, _nextPos) > 0.1f) return;
        _nextPos = GetRandomPos();
        StartCoroutine(FadeColorTo(GetRandomColor()));
    }

    private IEnumerator FadeColorTo(Color getRandomColor)
    {
        // fade to new color
        var startColor = image.color;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime;
            image.color = Color.Lerp(startColor, getRandomColor, t);
            yield return null;
        }
    }
}