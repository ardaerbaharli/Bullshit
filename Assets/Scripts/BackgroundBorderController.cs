using UnityEngine;

public class BackgroundBorderController : MonoBehaviour
{
    [SerializeField] private GameObject movingColorPrefab;
    [SerializeField] private int numberOfMovingColors;

    private void Start()
    {
        for (var i = 0; i < numberOfMovingColors; i++)
        {
            var movingColor = Instantiate(movingColorPrefab, transform);
            movingColor.transform.position = GetRandomPos();
        }
    }

    private float _sh, _sw;

    private void Awake()
    {
        _sh = Screen.height;
        _sw = Screen.width;
    }

    private Vector3 GetRandomPos()
    {
        // return random position between screen borders
        return new Vector3(Random.Range(0, _sw), Random.Range(0, _sh), 0);
    }
}